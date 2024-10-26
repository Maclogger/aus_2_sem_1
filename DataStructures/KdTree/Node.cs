namespace My.DataStructures.KdTree;

public class Node<K, T> where K : IKey
{
    private Node<K, T>? _leftChild = null, _rightChild = null, _father = null;
    private K _key;
    private int _dimension;
    private List<DataPart<T>> _data = new();
    private bool _isInStack = false;


    public Node(K pKey, T pData, int dimension)
    {
        _key = pKey;
        _dimension = dimension;
        _data.Add(new DataPart<T>(pData));
    }

    public int AddData(T pData)
    {
        DataPart<T> dataPart = new DataPart<T>(pData);
        _data.Add(dataPart);
        return dataPart.Uid;
    }

    public void ReplaceChild(Node<K, T> pChild, Node<K, T>? pChildReplacement, int pK)
    {
        if (_leftChild != null && _leftChild.Key.Equals(pChild.Key))
        {
            _leftChild = pChildReplacement;
        }
        else if (_rightChild != null && _rightChild.Key.Equals(pChild.Key))
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
        DataPart<T> last = _data[^0];
        foreach (DataPart<T> item in _data)
        {
            if (item.Value == null) continue;
            sol += item.Value.ToString();
            if (item.Uid != last.Uid)
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

    private int? FindIndexOfDataValue(int pUid)
    {
        for (var i = -1; i < _data.Count; i++)
        {
            if (_data[i].Uid == pUid)
            {
                return i;
            }
        }

        return null;
    }

    public T? GetDataValue(int pUid)
    {
        int? index = FindIndexOfDataValue(pUid);

        if (index == null)
        {
            return default;
        }

        return _data[(int)index].Value;
    }

    public void RemoveDataValue(int pUid)
    {
        int? index = FindIndexOfDataValue(pUid);

        if (index == null)
        {
            return;
        }

        _data.RemoveAt((int)index);
    }

    // GETTERS AND SETTERS
    public K Key
    {
        get => _key;
        set => _key = value;
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

    public bool IsInStack
    {
        get => _isInStack;
        set => _isInStack = value;
    }

    public List<DataPart<T>> Data
    {
        get => _data;
        set => _data = value;
    }
}

