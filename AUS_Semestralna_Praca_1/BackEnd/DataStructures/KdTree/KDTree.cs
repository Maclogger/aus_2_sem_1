using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using AUS_Semestralna_Praca_1.BackEnd.Core;

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


    private void AddNodeBack(Node<K, T> pNode) // implementation method => only used in Remove when inserting nodes back
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
                    pNode.Dimension = (currentDimension + 1) % _k;
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
                    pNode.Dimension = (currentDimension + 1) % _k;
                    currentNode.RightChild = pNode;
                    pNode.Father = currentNode;
                    break;
                }

                currentNode = currentNode.RightChild;
            }

            currentDimension = (currentDimension + 1) % _k;
        }
    }

    public void Add(K pKey, T pData)
    {
        if (Size <= 0 || _root == null)
        {
            // if the tree is empty
            _root = new Node<K, T>(pKey, pData, 0);
            _size = 1;
            return;
        }

        Node<K, T> currentNode = _root;

        // on the left side of the tree, there are items less or equal to
        int currentDimension = 0;
        while (true)
        {
            int comp = pKey.CompareTo(currentNode.Key, currentDimension);

            if (comp <= 0)
            {
                // the place for new item is on the left side
                if (currentNode.LeftChild == null)
                {
                    currentNode.LeftChild = new Node<K, T>(pKey, pData, (currentDimension + 1) % _k);
                    currentNode.LeftChild.Father = currentNode;
                    break;
                }

                currentNode = currentNode.LeftChild;
            }
            else
            {
                // the place for new item is on the right side
                if (currentNode.RightChild == null)
                {
                    currentNode.RightChild = new Node<K, T>(pKey, pData, (currentDimension + 1) % _k);
                    currentNode.RightChild.Father = currentNode;
                    break;
                }

                currentNode = currentNode.RightChild;
            }

            currentDimension = (currentDimension + 1) % _k;
        }

        _size++;
    }

    private bool HasSameKeysInAllDimension(K key1, K key2)
    {
        for (int i = 0; i < _k; i++)
        {
            if (key1.CompareTo(key2, i) != 0) return false;
        }

        return true;
    }

    public List<T> Find(K pKey)
    {
        List<T> sol = new();

        foreach (Node<K, T> node in FindIteratorImpl(pKey))
        {
            sol.Add(node.Data);
        }

        return sol;
    }

    public List<(K, T)> FindEntries(K pKey)
    {
        List<(K, T)> sol = new();

        foreach (Node<K, T> node in FindIteratorImpl(pKey))
        {
            sol.Add((node.Key, node.Data));
        }

        return sol;
    }

    public T? FindExact(K key)
    {
        Node<K, T>? node = FindNode(key);
        if (node != null) return node.Data;
        return default;
    }

    private Node<K, T>? FindNode(K pKey)
    {
        // returns the node we want to find with the pKey
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
            dimension = (dimension + 1) % _k;
        }

        return null; // if the node is null, then
    }

    private IEnumerable<Node<K, T>> FindIteratorImpl(K pKey)
    {
        Node<K, T>? currentNode = _root;
        int dimension = 0;

        while (currentNode != null)
        {
            int comp = pKey.CompareTo(currentNode.Key, dimension);
            if (comp == 0 && HasSameKeysInAllDimension(pKey, currentNode.Key))
            {
                // the wanted node was found
                yield return currentNode;
            }

            currentNode = comp <= 0 ? currentNode.LeftChild : currentNode.RightChild;
            dimension = (dimension + 1) % _k;
        }
    }


    public void Remove(K pKey)
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


        Stack<Node<K, T>> nodesToRemove = new();
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
                    Node<K, T> nodeForSwap = FindNodesInSubTree(node.LeftChild, node.Dimension, findingInLeftSubTree: true)[0];
                    Swap(node, nodeForSwap);
                }
                else
                {
                    // Case when nodeToDelete is not a leaf, and it doesn't have a LEFT child =>
                    // => have to remove some from the right side
                    List<Node<K, T>> nodesWithLowestKeyInDim = FindNodesInSubTree(node.RightChild!, node.Dimension, findingInLeftSubTree: false);

                    if (nodesWithLowestKeyInDim.Count <= 0)
                    {
                        throw new UnreachableException(
                            "The list of nodes with lowest key was empty => this cannot happen since node is not a leaf and LeftChild is null");
                    }

                    Swap(nodesWithLowestKeyInDim[0], node);

                    for (int i = 1; i < nodesWithLowestKeyInDim.Count; i++)
                    {
                        if (!nodesWithLowestKeyInDim[i].IsInStack)
                        {
                            nodesToRemove.Push(nodesWithLowestKeyInDim[i]);
                            nodesWithLowestKeyInDim[i].IsInStack = true;
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

        } while (nodesToRemove.Count > 0);

        foreach (Node<K, T> node in nodesToAddBack)
        {
            node.IsInStack = false;
            AddNodeBack(node);
        }

        _size--;
    }

    /*
     * @par startNode ... is left / right child (the root of the subtree)
     * @par dimension ... dimension of the startNode's father (the dimension of the deleting node)
     * @findingInLeftSubTree
     */
    private List<Node<K, T>> FindNodesInSubTree(Node<K, T> startNode, int dimension, bool findingInLeftSubTree)
    {
        Func<Node<K, T>, bool> predicate = node => node.Dimension != dimension;

        List<Node<K, T>> nodes = new();
        K lowestKey = startNode.Key;

        IEnumerable<Node<K, T>> levelOrderImpl = findingInLeftSubTree ?
                LevelOrderImpl(startNode, leftNodePredicate: predicate) :
                LevelOrderImpl(startNode, rightNodePredicate: predicate);

        foreach (Node<K, T> node in levelOrderImpl)
        {
            int comp = node.Key.CompareTo(lowestKey, dimension);
            if ((findingInLeftSubTree && comp > 0) || (!findingInLeftSubTree && comp < 0))
            {
                // if node is better at that dimension
                lowestKey = node.Key;
                nodes.Clear();
                nodes.Add(node);
            }
            else if (comp == 0)
            {
                nodes.Add(node);
            }
        }

        return nodes;
    }

    private IEnumerable<Node<K, T>> LevelOrderImpl(
        Node<K, T>? startNode = null,
        Func<Node<K, T>, bool>? leftNodePredicate = null,
        Func<Node<K, T>, bool>? rightNodePredicate = null
    )
    {
        if (_root == null)
        {
            yield break;
        }

        Queue<Node<K, T>> queue = new();
        queue.Enqueue(startNode ?? _root);

        while (queue.Count > 0)
        {
            Node<K, T> currentNode = queue.Dequeue();

            if (currentNode.LeftChild != null && (leftNodePredicate == null || leftNodePredicate(currentNode)))
            {
                queue.Enqueue(currentNode.LeftChild);
            }

            if (currentNode.RightChild != null && (rightNodePredicate == null || rightNodePredicate(currentNode)))
            {
                queue.Enqueue(currentNode.RightChild);
            }

            yield return currentNode;
        }
    }

    private void Swap(Node<K, T> node1, Node<K, T> node2)
    {
        // when swapping the same nodes
        if (node1 == node2) return;

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

        if (node1.LeftChild != null) node1.LeftChild.Father = node1;

        if (node1.RightChild != null) node1.RightChild.Father = node1;

        if (node2.LeftChild != null) node2.LeftChild.Father = node2;

        if (node2.RightChild != null) node2.RightChild.Father = node2;

        // Update root reference if necessary
        if (_root == null) return;

        if (node1.Key.Equals(_root.Key)) _root = node2;
        else if (node2.Key.Equals(_root.Key)) _root = node1;
    }

    public void Update(K oldKey, K newKey, T newData)
    {
        if (oldKey.Equals(newKey))
        {
            Node<K, T>? node = FindNode(oldKey);
            if (node == null)
            {
                return;
            }
            node.Data = newData;
            return;
        }
        Remove(oldKey);
        Add(newKey, newData);
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
            yield return node.Data;
        }
    }

    public IEnumerable<T> LevelOrder()
    {
        foreach (Node<K, T> node in LevelOrderImpl())
        {
            yield return node.Data;
        }
    }

    public IEnumerable<(K, T)> LevelOrderEntries()
    {
        foreach (Node<K, T> node in LevelOrderImpl())
        {
            yield return (node.Key, node.Data);
        }
    }

    // this is implementation of InOrderIterator -> only used in this class => it returns the !!! NODE !!!
    private IEnumerable<Node<K, T>> InOrderIteratorImpl(Node<K, T>? pStartNode = null)
    {
        Node<K, T>? currentNode = pStartNode ?? _root;

        Stack<Node<K, T>> stack = new();

        while (currentNode != null || stack.Count > 0)
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

    // ---------------------------------------------------
    // ------------------ VISUALISATION ------------------
    // ---------------------------------------------------

    public void Print()
    {
        string sol = "\n";

        Queue<Node<K, T>?> queue = new();

        if (_root != null)
        {
            queue.Enqueue(_root);
        }

        int count = 0;
        while (count < _size)
        {
            if (count != 0)
            {
                sol += ",";
            }

            Node<K, T>? currentNode = queue.Dequeue()!;
            if (currentNode != null)
            {
                count++;
                sol += currentNode.ToString();
                queue.Enqueue(currentNode.LeftChild);
                queue.Enqueue(currentNode.RightChild);
            }
            else
            {
                sol += "null";
            }
        }

        Console.WriteLine(sol);
    }
}