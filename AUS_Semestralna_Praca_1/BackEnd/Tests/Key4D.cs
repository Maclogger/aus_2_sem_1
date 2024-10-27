using System;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests;

public class Key4D : IKey
{
    private double _a;
    private string _b;
    private int _c;
    private double _d;

    public Key4D(double a, string b, int c, double d)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
    }

    public Key4D(Random random)
    {
        _a = random.NextDouble(); // Generate random double between 0.0 and 1.0
        _b = Guid.NewGuid().ToString(); // Generate random string using GUID
        _c = random.Next(); // Generate random integer
        _d = random.NextDouble(); // Generate random double between 0.0 and 1.0
    }

    public double A
    {
        get => _a;
        set => _a = value;
    }

    public string B
    {
        get => _b;
        set => _b = value;
    }

    public int C
    {
        get => _c;
        set => _c = value;
    }

    public double D
    {
        get => _d;
        set => _d = value;
    }


    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not Key4D pOtherKey) return 0;
        switch (pDimension)
        {
            case 0:
            {
                if (A < pOtherKey.A) return -1;
                if (A > pOtherKey.A) return 1;
                return String.Compare(B, pOtherKey.B, StringComparison.Ordinal);
            }
            case 1:
            {
                return C < pOtherKey.C ? -1 : C > pOtherKey.C ? 1 : 0;
            }
            case 2:
            {
                if (D < pOtherKey.D) return -1;
                if (D > pOtherKey.D) return 1;
                return 0;
            }
            default:
            {
                int comp = String.Compare(B, pOtherKey.B, StringComparison.Ordinal);
                if (comp == 0)
                {
                    return C < pOtherKey.C ? -1 : C > pOtherKey.C ? 1 : 0;
                }
                return comp;
            }
        }
    }

    public bool Equals(IKey pOther)
    {
        if (pOther is not Key4D pOtherKey) return false;
        return (Math.Abs(A - pOtherKey.A) < Config.Instance.Tolerance &&
                B == pOtherKey.B &&
                C == pOtherKey.C &&
                Math.Abs(D - pOtherKey.D) < Config.Instance.Tolerance);
    }
}