using System;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;

public class TestKey : IKey
{
    public Cord Cord { get; }

    private string UniversalKey { get; } = Utils.GetNextStringValOfLentgth(10);

    public TestKey(Cord cord)
    {
        Cord = cord;
    }

    public TestKey(Random random)
    {
        Cord = new Cord(random);
    }

    public TestKey(TestKey other)
    {
        Cord = new Cord(other.Cord);
    }

    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not TestKey testKey) throw new InvalidCastException("Compared IKey is not a TestKey");

        double left = pDimension == 0 ? Cord.X : Cord.Y;
        double right = pDimension == 0 ? testKey.Cord.X : testKey.Cord.Y;

        if (Math.Abs(left - right) < Config.Instance.Tolerance)
        {
            return 0;
        }

        return left < right ? -1 : 1;
    }

    public bool Equals(IKey pOther)
    {
        if (pOther is not TestKey testKey) throw new InvalidCastException("Compared IKey is not a TestKey");
        return UniversalKey == testKey.UniversalKey;
    }

    public override string ToString()
    {
        return $"{UniversalKey} {Cord}";
    }
}