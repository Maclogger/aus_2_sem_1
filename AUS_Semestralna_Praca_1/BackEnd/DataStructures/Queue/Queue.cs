namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures.Queue;

public class Node<T>
{
    private T _data;
    private Node<T>? _next = null;

    public Node(T data)
    {
        _data = data;
    }

    public Node<T>? Next
    {
        get => _next;
        set => _next = value;
    }

    public T Data
    {
        get => _data;
    }
}

public class Queue<T>
{
    private Node<T>? _first = null;
    private Node<T>? _last = null;
    private int _size = 0;

    public int Size
    {
        get => _size;
    }

    public bool IsEmpty()
    {
        return _size == 0;
    }

    public bool IsNotEmpty()
    {
        return _size > 0;
    }

    public void Add(T pData)
    {
        if (_first == null || _last == null || Size == 0)
        {
            _first = new Node<T>(pData);
            _last = _first;
            _size = 1;
            return;
        }

        _last.Next = new Node<T>(pData);
        _last = _last.Next;
        _size++;
    }

    public T? Pop()
    {
        if (_first == null || _last == null || Size == 0)
        {
            return default;
        }

        T temp = _first.Data;
        _first = _first.Next;
        _size--;
        return temp;
    }
}