using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using AUS_Semestralna_Praca_1.BackEnd.Tests;

namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

public class KdTree<K, T> : IEnumerable where K : IKey
{
    private Node<K, T>? _root;
    private int _k;
    private int _size = 0;


    public KdTree(int pK)
    {
        if (pK < 1)
        {
            throw new ArgumentException("K must be greater than zero");
        }

        _k = pK;
    }


    public int Size => _size;


    private void AddNodeBack(Node<K, T> pNode) // implementation method => only used in Remove() when inserting nodes back
    {
        if (Size <= 0 || _root == null)
        {
            // if the tree is empty
            _root = pNode;
            return;
        }

        Node<K, T> currentNode = _root;

        // on the left side of the tree, there are items less or equal to
        int currentDimension = 0;
        while (true)
        {
            int comp = pNode.Key.CompareTo(currentNode.Key, currentDimension);

            if (comp <= 0)
            {
                // the place for new item is on the left side
                if (currentNode.LeftChild == null)
                {
                    pNode.Dimension = (currentDimension + 1) % 2;
                    currentNode.LeftChild = pNode;
                    pNode.Father = currentNode;
                    break;
                }

                currentNode = currentNode.LeftChild;
            }
            else
            {
                // the place for new item is on the right side
                if (currentNode.RightChild == null)
                {
                    pNode.Dimension = (currentDimension + 1) % 2;
                    currentNode.RightChild = pNode;
                    pNode.Father = currentNode;
                    break;
                }

                currentNode = currentNode.RightChild;
            }

            currentDimension = (currentDimension + 1) % _k;
        }
    }

    public int Add(K pKey, T pData)
    {
        if (Size <= 0 || _root == null)
        {
            // if the tree is empty
            _root = new Node<K, T>(pKey, pData, 0);
            _size = 1;
            return _root.Data[0].Uid;
        }

        Node<K, T> currentNode = _root;

        int? uid = null;
        // on the left side of the tree, there are items less or equal to
        int currentDimension = 0;
        while (true)
        {
            int comp = pKey.CompareTo(currentNode.Key, currentDimension);

            if (comp == 0 && currentNode.Key.Equals(pKey))
            {
                // if there is already a node with the exact same Key, then we store the data part in List in Node
                uid = currentNode.AddData(pData);
                break;
            }

            if (comp <= 0)
            {
                // the place for new item is on the left side
                if (currentNode.LeftChild == null)
                {
                    currentNode.LeftChild = new Node<K, T>(pKey, pData, (currentDimension + 1) % 2);
                    currentNode.LeftChild.Father = currentNode;
                    uid = currentNode.LeftChild.Data[0].Uid;
                    break;
                }

                currentNode = currentNode.LeftChild;
            }
            else
            {
                // the place for new item is on the right side
                if (currentNode.RightChild == null)
                {
                    currentNode.RightChild = new Node<K, T>(pKey, pData, (currentDimension + 1) % 2);
                    currentNode.RightChild.Father = currentNode;
                    uid = currentNode.RightChild.Data[0].Uid;
                    break;
                }

                currentNode = currentNode.RightChild;
            }

            currentDimension = (currentDimension + 1) % _k;
        }

        _size++;
        return (int)uid!;
    }

    public List<DataPart<T>> FindDataParts(K pKey)
    {
        return FindNode(pKey)?.Data ?? new List<DataPart<T>>();
    }


    public List<T> Find(K pKey)
    {
        List<DataPart<T>> dataParts = FindDataParts(pKey);
        List<T> data = new();
        foreach (DataPart<T> dataPart in dataParts)
        {
            data.Add(dataPart.Value);
        }

        return data;
    }


    private Node<K, T>? FindNode(K pKey)
    {
        // returns the node we want to find with the pKey and also it's parent
        if (_size <= 0 || _root == null)
        {
            return null;
        }

        Node<K, T>? currentNode = _root;
        int dimension = 0;

        while (currentNode != null)
        {
            int comp = pKey.CompareTo(currentNode.Key, dimension);
            if (comp == 0 && currentNode.Key.Equals(pKey))
            {
                // the wanted node was found
                return currentNode;
            }

            currentNode = comp <= 0 ? currentNode.LeftChild : currentNode.RightChild;
            dimension = (dimension + 1) % 2;
        }

        return null; // if the node is null, then
    }

    public void Remove(K pKey, int pUid)
    {
        if (_size <= 1 && _root != null)
        {
            if (_root.Key.Equals(pKey))
            {
                // there is only 1 node in the tree (root) and it's going to be removed
                _root = null;
                _size = 0;
            }

            return;
        }

        Node<K, T>? nodeToDelete = FindNode(pKey);

        if (nodeToDelete == null)
        {
            return;
        }


        RemoveDuplicateFromNodesDuplicates(nodeToDelete, pUid);

        if (pUid > 0 && pUid < nodeToDelete.Data.Count) // (0; count) -> removing only when there is at least one left
        {
            nodeToDelete.Data.RemoveAt(pUid); // TODO
            _size--;
            return;
        }


        Stack.Stack<Node<K, T>> nodesToRemove = new();
        List<Node<K, T>> nodesToAddBack = new();

        nodesToRemove.Push(nodeToDelete);

        do
        {
            Node<K, T> node = nodesToRemove.Pop();
            if (node != nodeToDelete)
            {
                nodesToAddBack.Add(node);
            }

            while (!node.IsLeaf())
            {
                if (node.LeftChild != null)
                {
                    // finding a node with the biggest part of the key at current dimension
                    Node<K, T> nodeForSwap = FindNodeWithHighestKeyInDim(node.LeftChild, node.Dimension);
                    Swap(node, nodeForSwap);
                }
                else
                {
                    // Case when nodeToDelete is not a leaf, and it doesn't have a LEFT child =>
                    // => have to remove some from the right side
                    List<Node<K, T>> nodes = FindNodesWithLowestKeyInDim(node.RightChild!, node.Dimension);

                    if (nodes.Count <= 0)
                    {
                        throw new UnreachableException(
                            "The list of nodes with lowest key was empty => this cannot happen since node is not a leaf and LeftChild is null");
                    }

                    Swap(nodes[0], node);

                    for (int i = 1; i < nodes.Count; i++)
                    {
                        if (!nodes[i].IsInStack)
                        {
                            nodesToRemove.Push(nodes[i]);
                            nodes[i].IsInStack = true;
                        }
                    }
                }
            }

            if (node.Father == null)
            {
                throw new UnreachableException(
                    "The size of the tree >= 2 and the nodeToDelete is a leaf. There has to be a father. This should never happen.");
            }

            node.Father.ReplaceChild(node, null, _k); // this will remove the node from the tree
        } while (nodesToRemove.Size > 0);

        foreach (Node<K, T> node in nodesToAddBack)
        {
            node.IsInStack = false;
            AddNodeBack(node);
        }

        _size--;
    }

    private void RemoveDuplicateFromNodesDuplicates(Node<K, T> node, int pUid)
    {



    }

    private Node<K, T> FindNodeWithHighestKeyInDim(Node<K, T> pStartNode, int pDimension)
    {
        Node<K, T> nodeWithHighestKeyInDim = pStartNode;

        foreach (Node<K, T> node in InOrderIteratorImpl(pStartNode))
        {
            int comp = node.Key.CompareTo(nodeWithHighestKeyInDim.Key, pDimension);
            if (comp > 0 || (comp == 0 && node.IsLeaf()))
            {
                // if node has higher key in that dimension, or it is the same, but is also a leaf (leaf is better)
                nodeWithHighestKeyInDim = node;
            }
        }

        return nodeWithHighestKeyInDim;
    }


    private List<Node<K, T>> FindNodesWithLowestKeyInDim(Node<K,T> pStartNode, int pDimension)
    {
        List<Node<K, T>> nodesWithLowestKeyInDim = new();
        K lowestKey = pStartNode.Key;

        foreach (Node<K, T> node in InOrderIteratorImpl(pStartNode))
        {
            int comp = node.Key.CompareTo(lowestKey, pDimension);
            if (comp < 0)
            {
                // if node is lower than the current minimum
                lowestKey = node.Key;
                nodesWithLowestKeyInDim.Clear();
                nodesWithLowestKeyInDim.Add(node);
            }
            else if (comp == 0)
            {
                nodesWithLowestKeyInDim.Add(node);
            }
        }

        return nodesWithLowestKeyInDim;
    }

    private void Swap(Node<K, T> node1, Node<K, T> node2)
    {
        if (node1 == node2 || node1.Key.Equals(node2.Key))
        {
            // when swapping the same nodes
            return;
        }

        if (node1.Father != null && node1.Father == node2.Father)
        {
            node1.Father.SwapChilds();
        }
        else
        {
            if (node1.Father != null)
            {
                node1.Father.ReplaceChild(node1, node2, _k);
            }

            if (node2.Father != null)
            {
                node2.Father.ReplaceChild(node2, node1, _k);
            }
        }

        (node1.LeftChild, node2.LeftChild) = (node2.LeftChild, node1.LeftChild);
        (node1.RightChild, node2.RightChild) = (node2.RightChild, node1.RightChild);
        (node1.Father, node2.Father) = (node2.Father, node1.Father);
        (node1.Dimension, node2.Dimension) = (node2.Dimension, node1.Dimension);

        if (node1.LeftChild != null)
        {
            node1.LeftChild.Father = node1;
        }

        if (node1.RightChild != null)
        {
            node1.RightChild.Father = node1;
        }

        if (node2.LeftChild != null)
        {
            node2.LeftChild.Father = node2;
        }

        if (node2.RightChild != null)
        {
            node2.RightChild.Father = node2;
        }

        // Update root reference if necessary
        if (_root != null)
        {
            if (node1.Key.Equals(_root.Key))
            {
                _root = node2;
            }
            else if (node2.Key.Equals(_root.Key))
            {
                _root = node1;
            }
        }
    }

    // ---------------------------------------------------
    // -------------------- ITERATORS --------------------
    // ---------------------------------------------------

    // public default iterator
    public IEnumerator GetEnumerator()
    {
        return InOrderIterator();
    }

    // public in order iterator method for a user => it returns the !!! DATA of type T !!!
    public IEnumerator InOrderIterator()
    {
        foreach (Node<K, T> node in InOrderIteratorImpl())
        {
            foreach (DataPart<T> data in node.Data)
            {
                yield return data.Value;
            }
        }
    }

    // public in order iterator method for a user => it returns tuple of <K, T>
    public IEnumerator EntryInOrderIterator()
    {
        foreach (Node<K, T> node in InOrderIteratorImpl())
        {
            foreach (DataPart<T> data in node.Data)
            {
                yield return new Tuple<K, T>(node.Key, data.Value);
            }
        }
    }

    // public level order iterator method for a user => it returns the !!! DATA of type T !!!
    public IEnumerable<T> LevelOrder()
    {
        foreach (Node<K, T> node in LevelOrderImpl())
        {
            foreach (DataPart<T> data in node.Data)
            {
                yield return data.Value;
            }
        }
    }

    // this is implementation of InOrderIterator -> only used in this class => it returns the !!! NODE !!!
    private IEnumerable<Node<K, T>> InOrderIteratorImpl(Node<K, T>? pStartNode = null)
    {
        Node<K, T>? currentNode = pStartNode ?? _root;

        Stack.Stack<Node<K, T>> stack = new();

        while (currentNode != null || stack.Size > 0)
        {
            // we go all a way to bottom left node
            while (currentNode != null)
            {
                stack.Push(currentNode);
                currentNode = currentNode.LeftChild;
            }

            // we are at the most bottom left node => currentNode = null

            currentNode = stack.Pop(); // we go up one level to father

            yield return currentNode;

            currentNode = currentNode.RightChild; // we go to right node
        }
    }

    // this is implementation of LevelOrderIterator -> only used in this class => it returns the !!! NODE !!!
    private IEnumerable<Node<K, T>> LevelOrderImpl()
    {
        Queue.Queue<Node<K, T>> queue = new();

        if (_root != null)
        {
            queue.Add(_root);
        }

        while (queue.IsNotEmpty())
        {
            Node<K, T> currentNode = queue.Pop()!;

            if (currentNode.LeftChild != null)
            {
                queue.Add(currentNode.LeftChild);
            }

            if (currentNode.RightChild != null)
            {
                queue.Add(currentNode.RightChild);
            }

            yield return currentNode;
        }
    }

    // ---------------------------------------------------
    // ------------------ VISUALISATION ------------------
    // ---------------------------------------------------

    public void Print()
    {
        string sol = "\n";

        Queue.Queue<Node<K, T>?> queue = new();

        if (_root != null)
        {
            queue.Add(_root);
        }

        int count = 0;
        while (count < _size)
        {
            if (count != 0)
            {
                sol += ",";
            }

            Node<K, T>? currentNode = queue.Pop()!;
            if (currentNode != null)
            {
                count += currentNode.Data.Count;
                sol += currentNode.ToString();
                queue.Add(currentNode.LeftChild);
                queue.Add(currentNode?.RightChild);
            }
            else
            {
                sol += "null";
            }
        }

        Console.WriteLine(sol);
    }

    public void Swap(K pKey1, K pKey2)
    {
        Swap(FindNode(pKey1)!, FindNode(pKey2)!);
    }

    public void Update(K key, int uid, T newData)
    {
        Node<K,T>? node = FindNode(key);
        if (node == null)
        {
            // the node doesn't exists
            return;
        }

        node.SetDataValue(uid, newData);
    }
}