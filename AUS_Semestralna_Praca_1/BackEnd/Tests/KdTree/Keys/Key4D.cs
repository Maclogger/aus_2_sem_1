using System;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;

public class Key4D : IKey
{
    public double A { get; set; }
    public string B { get; set; }
    public int C { get; set; }
    public double D { get; set; }
    public int Uid { get; set; }

    public Key4D(double a, string b, int c, double d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public Key4D(Random random)
    {
        A = random.NextDouble(); // Generate random double between 0.0 and 1.0
        B = Guid.NewGuid().ToString(); // Generate random string using GUID
        C = random.Next(); // Generate random integer
        D = random.NextDouble(); // Generate random double between 0.0 and 1.0
        Uid = Utils.GetNextVal();
    }

    public override string ToString()
    {
        return $"A={A}, B={B}, C={C}, D={D}";
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

        return (
            Math.Abs(A - pOtherKey.A) < Config.Instance.Tolerance &&
            B == pOtherKey.B &&
            C == pOtherKey.C &&
            Math.Abs(D - pOtherKey.D) < Config.Instance.Tolerance &&
            Uid == pOtherKey.Uid
        );
    }
}