using System;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;

public class MyIntKey : IKey
{
    private int _value;

    public MyIntKey(int pValue)
    {
        Value = pValue;
    }

    public int Value
    {
        get => _value;
        set => _value = value;
    }

    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not MyIntKey myIntKey)
        {
            throw new ArgumentException("Object is not an IntItem");
        }

        if (Value == myIntKey.Value)
        {
            return 0;
        }

        return Value < myIntKey.Value ? -1 : 1;
    }

    public bool Equals(IKey pOther)
    {
        if (pOther is not MyIntKey myIntKey) return false;
        return Value == myIntKey.Value;
    }
}