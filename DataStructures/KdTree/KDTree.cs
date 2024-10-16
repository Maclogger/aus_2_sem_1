using System.Collections;

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
        }

        public List<T> Data
        {
            get => _data;
        }

        public Node<K, T>? Father
        {
            get => _father;
            set => _father = value;
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

        private bool EqualsTwoKeys(K pk1, K pk2)
        {
            for (int dimension = 0; dimension < _k; dimension++)
            {
                if (pk1.CompareTo(pk2, dimension) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public int Size => _size;

        public void Add(K pKey, T pData)
        {
            Node<K, T> newNode = new(pKey, pData);

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

                int comp = newNode.Key.CompareTo(currentNode.Key, currentDimension);

                if (comp == 0 && EqualsTwoKeys(currentNode.Key, pKey))
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
                        currentNode.LeftChild = newNode;
                        newNode.Father = currentNode;
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
                        newNode.Father = currentNode;
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


        private Node<K, T>? FindNode(K pKey, bool returnFather = false)
        {
            // returns the node we want to find with the pKey and also it's parent
            if (_size <= 0 || _root == null)
            {
                return null;
            }

            Node<K, T>? currentNode = _root;
            Node<K, T>? fatherNode = null;
            int dimension = 0;

            while (currentNode != null)
            {
                int comp = pKey.CompareTo(currentNode.Key, dimension);
                if (comp == 0 && EqualsTwoKeys(currentNode.Key, pKey))
                {
                    // the wanted node was found
                    return returnFather ? fatherNode : currentNode;
                }

                fatherNode = currentNode;
                currentNode = comp <= 0 ? currentNode.LeftChild : currentNode.RightChild;
                dimension++;
            }

            return null; // if the node is null, then
        }

        public void Remove(K pKey)
        {
            if (_size <= 1 && _root != null)
            {
                if (EqualsTwoKeys(_root.Key, pKey))
                {
                    // there is only 1 node in the tree (root) and it's gonna be removed
                    _root = null;
                    _size = 0;
                }
                return;
            }

            Node<K, T>? fatherNode = FindNode(pKey, returnFather: true);
            if (fatherNode == null)
            {
                // wanted node is not in the tree
                return;
            }

            Node<K, T> nodeToDelete;
            bool isLeftSon;
            if (fatherNode.LeftChild != null && EqualsTwoKeys(fatherNode.LeftChild.Key, pKey))
            {
                nodeToDelete = fatherNode.LeftChild;
                isLeftSon = true;
            }
            else
            {
                nodeToDelete = fatherNode.RightChild!;
                isLeftSon = false;
            }

            if (nodeToDelete.RightChild == null && nodeToDelete.LeftChild == null)
            {
                // base case when removing leaf
                if (isLeftSon)
                {
                    fatherNode.LeftChild = null;
                }
                else
                {
                    fatherNode.RightChild = null;
                }

                return;
            }

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

        // public in order iterator method for a user => it returns tuple of <K, T>
        public IEnumerator EntryInOrderIterator()
        {
            foreach (Node<K,T> node in InOrderIteratorImpl())
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

    }
}
