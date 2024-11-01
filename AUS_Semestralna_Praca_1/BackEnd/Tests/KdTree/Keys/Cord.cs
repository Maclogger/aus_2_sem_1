using System;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
public class Cord : IKey
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Uid { get; set; }


    public Cord(int pX, int pY)
    {
        X = pX;
        Y = pY;
        Uid = Utils.GetNextVal();
    }

    public Cord(Random random)
    {
        X = random.Next(int.MinValue, int.MaxValue);
        Y = random.Next(int.MinValue, int.MaxValue);
        Uid = Utils.GetNextVal();
    }

    public override string ToString()
    {
        return Config.Instance.UidPrint ? $"[{X};{Y};{Uid}]" : $"[{X};{Y}]";
    }

    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not Cord cord)
        {
            throw new ArgumentException("Object is not an IntItem");
        }

        if (pDimension == 0)
        {
            if (X < cord.X)
            {
                return -1;
            }

            if (X > cord.X)
            {
                return 1;
            }
        }
        else if (pDimension == 1)
        {
            if (Y < cord.Y)
            {
                return -1;
            }

            if (Y > cord.Y)
            {
                return 1;
            }
        }

        return 0;
    }

    public bool Equals(IKey pOther)
    {
        if (pOther is not Cord cord) return false;
        return X == cord.X && Y == cord.Y && Uid == cord.Uid;
    }
}
