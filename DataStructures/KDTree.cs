using System.Collections;
using Entities;

namespace My.DataStructures
{
    public interface IKey
    {
        int CompareTo(IKey other, int dimension);
    }


    class Node<T> where T : IKey
    {
        private Node<T>? _leftChild = null, _rightChild = null;
        private T _item;

        public Node(T pItem)
        {
            Item = pItem;
        }

        public Node<T>? LeftChild
        {
            get => _leftChild;
            set => _leftChild = value;
        }

        public Node<T>? RightChild
        {
            get => _rightChild;
            set => _rightChild = value;
        }

        public T Item
        {
            get => _item;
            set => _item = value;
        }

        public int CompareTo(Node<T>? pOtherNode, int dimension)
        {
            if (pOtherNode == null)
                throw new ArgumentNullException(nameof(pOtherNode));

            return Item.CompareTo(pOtherNode.Item, dimension);
        }
    }

    class KdTree<T> where T : IKey
    {
        private Node<T>? _root;
        private int _k;
        private int _size = 0;
        public KdTree(int pK)
        {
            _k = pK;
        }

        public void Add(T pItem)
        {
            Node<T> newNode = new Node<T>(pItem);

            if (_size <= 0 || _root == null)
            {
                // if the tree is empty
                _root = newNode;
                _size = 1;
                return;
            }

            Node<T>? currentNode = _root;

            // at the left side of the tree, there are items less or equal to
            int currentDimension = 0;
            while (true)
            {
                int comp = currentNode.CompareTo(newNode, currentDimension);
                if (comp > 0)
                {
                    // the place for new item is at the left side
                    if (currentNode.LeftChild == null)
                    {
                        currentNode.LeftChild = newNode;
                        break;
                    }
                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    // the place for new item is at the right side
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
    }
}
