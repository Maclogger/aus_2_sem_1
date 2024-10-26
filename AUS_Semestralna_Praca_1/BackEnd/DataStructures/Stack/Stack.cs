using System;

namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures.Stack;

public class Node<T>
{
    private Node<T>? _next = null;
    private T _data;

    public Node(T pData)
    {
        _data = pData;
    }

    public T Data
    {
        get => _data;
    }

    public Node<T>? Next
    {
        get => _next;
        set => _next = value;
    }
}

public class Stack<T>
{
    private int _size = 0;
    private Node<T>? _root = null;

    public int Size
    {
        get => _size;
    }

    public void Push(T pData)
    {
        Node<T> node = new Node<T>(pData);
        node.Next = _root;
        _root = node;
        _size++;
    }

    public T Pop()
    {
        if (_size == 0 || _root is null)
        {
            throw new InvalidOperationException("The stack is empty.");
        }

        T temp = _root.Data;
        _root = _root.Next;
        _size--;

        return temp;
    }
}