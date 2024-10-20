using System.Collections;
using System.Diagnostics;
using My.Tests;
using My.DataStructures.Stack;

namespace My.DataStructures.KdTree
{
    public interface IKey
    {
        int CompareTo(IKey pOther, int pDimension);
    }

    public class Node<K, T> where K : IKey
    {
        private Node<K, T>? _leftChild = null, _rightChild = null, _father = null;
        private K _key;
        private int _dimension;
        private List<T> _data;


        public Node(K pKey, T pData, int dimension)
        {
            _key = pKey;
            _dimension = dimension;
            _data = new List<T>();
            _data.Add(pData);
        }

        public Node(Node<K, T> pOtherNode)
        {
            _key = pOtherNode.Key;
            // _data = new List<T>(other.Data); --> TODO test if it should be new List or just a pointer copy
            _data = pOtherNode.Data;
            _father = pOtherNode.Father;
            _leftChild = pOtherNode.LeftChild;
            _rightChild = pOtherNode.RightChild;
            _dimension = pOtherNode.Dimension;
        }


        public Node<K, T>? LeftChild
        {
            get => _leftChild;
            set => _leftChild = value;
        }

        public Node<K, T>? RightChild
        {
            get => _rightChild;
            set => _rightChild = value;
        }

        public K Key
        {
            get => _key;
            set => _key = value;
        }

        public List<T> Data
        {
            get => _data;
            set => _data = value;
        }

        public Node<K, T>? Father
        {
            get => _father;
            set => _father = value;
        }

        public int Dimension
        {
            get => _dimension;
            set => _dimension = value;
        }

        public void AddData(T pItem)
        {
            _data.Add(pItem);
        }

        public void ReplaceChild(Node<K, T> pChild, Node<K, T>? pChildReplacement, int pK)
        {
            if (_leftChild != null && KdTreeUtils<K>.EqualsTwoKeys(_leftChild.Key, pChild.Key, pK))
            {
                _leftChild = pChildReplacement;
            }
            else if (_rightChild != null && KdTreeUtils<K>.EqualsTwoKeys(_rightChild.Key, pChild.Key, pK))
            {
                _rightChild = pChildReplacement;
            }
            else
            {
                throw new UnauthorizedAccessException("You are trying to remove a node from a wrong parent!");
            }
        }

        public bool IsLeaf()
        {
            return _leftChild == null && _rightChild == null;
        }

        public override string ToString()
        {
            string sol = _key.ToString() ?? "";
            sol += $"({_dimension}):";
            T last = _data[^1];
            foreach (T item in _data)
            {
                if (item == null) continue;
                sol += item.ToString();
                if (!item.Equals(last))
                {
                    sol += "->";
                }
            }

            return sol;
        }

        public void SwapChilds()
        {
            (_leftChild, _rightChild) = (_rightChild, _leftChild);
        }
    }

    public class KdTreeUtils<K> where K : IKey
    {
        public static bool EqualsTwoKeys(K pk1, K pk2, int pK)
        {
            for (int dimension = 0; dimension < pK; dimension++)
            {
                if (pk1.CompareTo(pk2, dimension) != 0)
                {
                    return false;
                }
            }

            return true;
        }
    }

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

                if (comp == 0 && KdTreeUtils<K>.EqualsTwoKeys(currentNode.Key, pKey, _k))
                {
                    // if there is already a node with the exact same Key, then we store the data part in List in Node
                    currentNode.AddData(pData);
                    break;
                }

                if (comp <= 0)
                {
                    // the place for new item is on the left side
                    if (currentNode.LeftChild == null)
                    {
                        currentNode.LeftChild = new Node<K, T>(pKey, pData, (currentDimension + 1) % 2);
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
                        currentNode.RightChild = new Node<K, T>(pKey, pData, (currentDimension + 1) % 2);
                        currentNode.RightChild.Father = currentNode;
                        break;
                    }

                    currentNode = currentNode.RightChild;
                }

                currentDimension = (currentDimension + 1) % _k;
            }

            _size++;
        }

        public List<T>? Find(K pKey)
        {
            return FindNode(pKey)?.Data;
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
                if (comp == 0 && KdTreeUtils<K>.EqualsTwoKeys(currentNode.Key, pKey, _k))
                {
                    // the wanted node was found
                    return currentNode;
                }

                currentNode = comp <= 0 ? currentNode.LeftChild : currentNode.RightChild;
                dimension = (dimension + 1) % 2;
            }

            return null; // if the node is null, then
        }

        public void Remove(K pKey)
        {
            if (_size <= 1 && _root != null)
            {
                if (KdTreeUtils<K>.EqualsTwoKeys(_root.Key, pKey, _k))
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


            Stack.Stack<Node<K, T>> stackNodesToDelete = new();
            stackNodesToDelete.Push(nodeToDelete);

            while (stackNodesToDelete.Size > 0)
            {
                nodeToDelete = stackNodesToDelete.Pop();

                while (!nodeToDelete.IsLeaf())
                {
                    if (nodeToDelete.LeftChild != null)
                    {
                        // finding a node with the biggest part of the key at current dimension
                        Node<K, T> nodeForSwap = FindNodeWithHighestKeyInDim(nodeToDelete.LeftChild, nodeToDelete.Dimension);
                        Swap(nodeToDelete, nodeForSwap);
                    }
                    else
                    {
                        // Case when nodeToDelete is not a leaf, and it doesn't have a LEFT child =>
                        // => have to remove some from the right side
                        // TODO List<Node<K, T>> nodesToSwap

                        throw new UnreachableException("Okej toto sa nemalo stať.");
                    }
                }
                if (nodeToDelete.Father == null)
                {
                    throw new UnreachableException(
                        "The size of the tree >= 2 and the nodeToDelete is a leaf. There has to be a father. This should never happen.");
                }

                nodeToDelete.Father.ReplaceChild(nodeToDelete, null, _k); // this will remove the node from the tree
            }

            _size--;
        }

        private void Swap(Node<K, T> node1, Node<K, T> node2)
        {
            if (node1 == node2 || KdTreeUtils<K>.EqualsTwoKeys(node1.Key, node2.Key, _k))
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
                if (KdTreeUtils<K>.EqualsTwoKeys(node1.Key, _root.Key, _k))
                {
                    _root = node2;
                }
                else if (KdTreeUtils<K>.EqualsTwoKeys(node2.Key, _root.Key, _k))
                {
                    _root = node1;
                }
            }
        }


        private Node<K, T> FindNodeWithHighestKeyInDim(Node<K, T> pStartNode, int dimension)
        {
            Node<K, T> nodeWithHighestKeyInDim = pStartNode;

            foreach (Node<K, T> node in InOrderIteratorImpl(pStartNode))
            {
                int comp = node.Key.CompareTo(nodeWithHighestKeyInDim.Key, dimension);
                if (comp > 0 || (comp == 0 && node.IsLeaf()))
                {
                    // if node has higher key in that dimension, or it is the same, but is also a leaf (leaf is better)
                    nodeWithHighestKeyInDim = node;
                }
            }

            return nodeWithHighestKeyInDim;
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
                foreach (T data in node.Data)
                {
                    yield return data;
                }
            }
        }

        // public in order iterator method for a user => it returns tuple of <K, T>
        public IEnumerator EntryInOrderIterator()
        {
            foreach (Node<K, T> node in InOrderIteratorImpl())
            {
                foreach (T data in node.Data)
                {
                    yield return new Tuple<K, T>(node.Key, data);
                }
            }
        }

        // public level order iterator method for a user => it returns the !!! DATA of type T !!!
        public IEnumerable<T> LevelOrder()
        {
            foreach (Node<K, T> node in LevelOrderImpl())
            {
                foreach (T data in node.Data)
                {
                    yield return data;
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
    }
}