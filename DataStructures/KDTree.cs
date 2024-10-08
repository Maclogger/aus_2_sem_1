using System.Collections;
using Entities;

namespace My.DataStructures
{
    public interface IKey
    {
        int CompareTo(IKey pOther, int pDimension);
        bool Equals(IKey pOther);
    }


    class Node<K, T> where K : IKey
    {
        private Node<K, T>? _leftChild = null, _rightChild = null;
        private K _key;
        private T? _data;


        public Node(K pKey, T? pData)
        {
            _key = pKey;
            _data = pData;
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

        public T? Data
        {
            get => _data;
            set => _data = value;
        }
    }

    class KdTree<K, T> : IEnumerable where K : IKey
    {
        private Node<K, T?>? _root;
        private int _k;
        private int _size = 0;
        public KdTree(int pK)
        {
            _k = pK;
        }

        public void Add(K pKey, T? pItem)
        {
            Node<K, T?> newNode = new(pKey, pItem);

            if (_size <= 0 || _root == null)
            {
                // if the tree is empty
                _root = newNode;
                _size = 1;
                return;
            }

            Node<K, T?> currentNode = _root;

            // on the left side of the tree, there are items less or equal to
            int currentDimension = 0;
            while (true)
            {
                int comp = newNode.Key.CompareTo(currentNode.Key, currentDimension);

                if (comp <= 0)
                {
                    // the place for new item is on the left side
                    if (currentNode.LeftChild == null)
                    {
                        currentNode.LeftChild = newNode;
                        break;
                    }
                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    // the place for new item is on the right side
                    if (currentNode.RightChild == null)
                    {
                        currentNode.RightChild = newNode;
                        break;
                    }
                    currentNode = currentNode.RightChild;
                }

                currentDimension = (currentDimension + 1) % _k;
            }

            _size++;
        }

        public List<T?> Find(K pKey)
        {
            List<T?> sol = new();

            Node<K, T?>? currentNode = _root;

            int currentDimension = 0;
            while (currentNode != null)
            {
                if (pKey.Equals(currentNode.Key))
                {
                    sol.Add(currentNode.Data);
                }

                int comp = pKey.CompareTo(currentNode.Key, currentDimension);

                if (comp <= 0)
                {
                    // item is on the left side
                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    // item is on the right side
                    currentNode = currentNode.RightChild;
                }

                currentDimension = (currentDimension + 1) % _k;
            }

            return sol;
        }

        public IEnumerator GetEnumerator()
        {
            Node<K, T?>? currentNode = _root;
            Stack<Node<K, T?>> stack = new();

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
                yield return currentNode.Data; // we return the value to iterator

                currentNode = currentNode.RightChild; // we go to right node
            }
        }
    }
}
