using System;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree;

public class Cord : IComparable<Cord>
{
    public int X { get; }
    public int Y { get; }

    public Cord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Cord(Random random)
    {
        X = random.Next(1, 51);
        Y = random.Next(1, 51);
    }

    public Cord(Cord otherCord)
    {
        X = otherCord.X;
        Y = otherCord.Y;
    }


    // just because of sorting and testing purposes
    public int CompareTo(Cord? other)
    {
        if (other == null) return -1;
        if (X < other.X) return -1;
        if (X > other.X) return 1;

        if (Y < other.Y) return -1;
        if (Y > other.Y) return 1;
        return 0;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Cord cord) return false;
        return X == cord.X && Y == cord.Y;
    }

    public override string ToString()
    {
        return $"[{X}, {Y}]";
    }
}