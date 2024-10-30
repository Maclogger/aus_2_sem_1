using System;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
public class Cord : IKey
{
    private int _x;
    private int _y;

    public Cord(int pX, int pY)
    {
        _x = pX;
        _y = pY;
    }

    public Cord(Random random)
    {
        _x = random.Next(int.MinValue, int.MaxValue);
        _y = random.Next(int.MinValue, int.MaxValue);
    }

    public int X
    {
        get => _x;
    }

    public int Y
    {
        get => _y;
    }

    public override string ToString()
    {
        return $"[{_x};{_y}]";
    }

    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not Cord cord)
        {
            throw new ArgumentException("Object is not an IntItem");
        }

        if (pDimension == 0)
        {
            if (_x < cord.X)
            {
                return -1;
            }

            if (_x > cord.X)
            {
                return 1;
            }
        }
        else if (pDimension == 1)
        {
            if (_y < cord.Y)
            {
                return -1;
            }

            if (_y > cord.Y)
            {
                return 1;
            }
        }

        return 0;
    }

    public bool Equals(IKey pOther)
    {
        if (pOther is not Cord cord) return false;
        return _x == cord.X && _y == cord.Y;
    }
}
