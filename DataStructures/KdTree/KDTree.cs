using System.Collections;

namespace My.DataStructures.KdTree
{
    public interface IKey
    {
        int CompareTo(IKey pOther, int pDimension);
        bool Equals(IKey pOther);
    }


    public class Node<K, T> where K : IKey
    {
        private Node<K, T>? _leftChild = null, _rightChild = null;
        private K _key;
        private List<T> _data;


        public Node(K pKey, T pData)
        {
            _key = pKey;
            _data = new List<T>();
            _data.Add(pData);
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

        public void AddData(T pItem)
        {
            _data.Add(pItem);
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

        public void Add(K pKey, T pItem)
        {
            Node<K, T> newNode = new(pKey, pItem);

            if (Size <= 0 || _root == null)
            {
                // if the tree is empty
                _root = newNode;
                _size = 1;
                return;
            }
            Node<K, T> currentNode = _root;

            // on the left side of the tree, there are items less or equal to
            int currentDimension = 0;
            while (true)
            {
                // if there is already a node with the exact same Key, then we store the data part in List in Node
                if (pKey.Equals(currentNode.Key))
                {
                    currentNode.AddData(pItem);
                    break;
                }

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

        public List<T>? Find(K pKey)
        {
            Node<K, T>? currentNode = _root;

            int currentDimension = 0;
            while (currentNode != null || Size > 0)
            {
                if (pKey.Equals(currentNode!.Key))
                {
                    return currentNode.Data;
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

            return null;
        }

        public void Remove()
        {
            throw new NotImplementedException();
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
            foreach (Node<K,T> node in InOrderIteratorImpl())
            {
                foreach (T data in node.Data)
                {
                    yield return data;
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

    }
}
