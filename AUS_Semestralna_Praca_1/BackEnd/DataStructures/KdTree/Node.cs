using System;
using System.Collections.Generic;

namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

public class Node<K, T> where K : IKey
{
    public Node<K, T>? LeftChild { get; set; } = null;
    public Node<K, T>? RightChild { get; set; } = null;
    public Node<K, T>? Father { get; set; } = null;
    public K Key { get; set; }
    public int Dimension { get; set; }
    public T Data { get; set; }
    public bool IsInStack { get; set; } = false;


    public Node(K pKey, T pData, int dimension)
    {
        Key = pKey;
        Dimension = dimension;
        Data = pData;
    }

    public void ReplaceChild(Node<K, T> pChild, Node<K, T>? pChildReplacement, int pK)
    {
        if (LeftChild != null && LeftChild.Key.Equals(pChild.Key))
        {
            LeftChild = pChildReplacement;
        }
        else if (RightChild != null && RightChild.Key.Equals(pChild.Key))
        {
            RightChild = pChildReplacement;
        }
        else
        {
            throw new UnauthorizedAccessException("You are trying to remove a node from a wrong parent!");
        }
    }

    public bool IsLeaf()
    {
        return LeftChild == null && RightChild == null;
    }

    public override string ToString()
    {
        string sol = Key.ToString() ?? "";
        sol += $"({Dimension}):";

        if (Data == null) return sol;

        sol += Data.ToString();
        return sol;
    }

    public void SwapChilds()
    {
        (LeftChild, RightChild) = (RightChild, LeftChild);
    }
}
