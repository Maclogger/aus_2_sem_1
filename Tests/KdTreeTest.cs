using Microsoft.VisualStudio.TestTools.UnitTesting;
using My.DataStructures;
using My.DataStructures.KdTree;

namespace My.Tests;

public class MyIntKey : IKey
{
    private int _value;

    public MyIntKey(int value)
    {
        _value = value;
    }

    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not MyIntKey myIntKey)
        {
            throw new ArgumentException("Object is not an IntItem");
        }

        if (_value == myIntKey._value)
        {
            return 0;
        }

        return _value < myIntKey._value ? -1 : 1;
    }

    public bool Equals(IKey pOther)
    {
        if (pOther is not MyIntKey myIntKey)
        {
            throw new ArgumentException("Object is not an IntItem");
        }

        return _value == myIntKey._value;
    }
}

public class Cord : IKey
{
    private int _x;
    private int _y;

    public Cord(int pX, int pY)
    {
        _x = pX;
        _y = pY;
    }

    public int X
    {
        get => _x;
        set => _x = value;
    }

    public int Y
    {
        get => _y;
        set => _y = value;
    }

    public override string ToString()
    {
        return $"[{_x}, {_y}]";
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
        } else if (pDimension == 1)
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
        if (pOther is not Cord cord)
        {
            throw new ArgumentException("Object is not an IntItem");
        }

        return _x == cord.X && _y == cord.Y;
    }
}

public class KdTreeTest
{
    public static void RunAllTests()
    {
        TestAdd();
        RandomAddTest();
    }

    private static KdTree<MyIntKey, int> SetUpRandomIntTree(int pCount, int pMin, int pMax)
    {
        KdTree<MyIntKey, int> tree = new(1);
        Random random = new();

        int count = pCount;
        for (int i = 0; i < count; i++)
        {
            int randomNumber = Utils.GetRandomIntInRange(pMin, pMax, random);
            tree.Add(new MyIntKey(randomNumber), randomNumber);
        }

        return tree;
    }

    private static KdTree<Cord, string> Create2dTreeEx1()
    {
        KdTree<Cord, string> tree = new(2);
        tree.Add(new Cord(23, 35), "Nitra");
        tree.Add(new Cord(20, 33), "Sereď");
        tree.Add(new Cord(25, 36), "Topoľčianky");
        tree.Add(new Cord(16, 31), "Galanta");
        tree.Add(new Cord(14, 39), "Senica");
        tree.Add(new Cord(28, 34), "Tlmače");
        tree.Add(new Cord(24, 40), "Bošany");
        tree.Add(new Cord(13, 32), "Bratislava");
        tree.Add(new Cord(12, 41), "Hodonín");
        tree.Add(new Cord(17, 42), "Trnava");
        tree.Add(new Cord(26, 35), "Moravce");
        tree.Add(new Cord(30, 33), "Levice");
        tree.Add(new Cord(29, 46), "Bojnice");
        tree.Add(new Cord(27, 43), "Nováky");

        return tree;
    }

    public static void TestAdd()
    {
        KdTree<Cord,string> tree = Create2dTreeEx1();

        int size = tree.Size;
        Assert.AreEqual(size, 14);
    }

    public static void RandomAddTest()
    {
        int count = 10_000;
        Random random = new();
        KdTree<MyIntKey, int> tree = SetUpRandomIntTree(count, -10_000, 10_000);

        Assert.AreEqual(tree.Size, count);
    }

    public static void TestInOrder()
    {
        KdTree<Cord,string> tree = Create2dTreeEx1();
        List<string> expected = [
            "Bratislava",
            "Galanta",
            "Sereď",
            "Hodonín",
            "Senica",
            "Trnava",
            "Nitra",
            "Moravce",
            "Tlmače",
            "Levice",
            "Topoľčianky",
            "Bošany",
            "Nováky",
            "Bojnice"
        ];
        List<string> actual = new();
        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void TestLevelOrder()
    {
        KdTree<Cord,string> tree = Create2dTreeEx1();
        List<string> expected = [
            "Nitra",
            "Sereď",
            "Topoľčianky",
            "Galanta",
            "Senica",
            "Tlmače",
            "Bošany",
            "Bratislava",
            "Hodonín",
            "Trnava",
            "Moravce",
            "Levice",
            "Bojnice",
            "Nováky"
        ];
        List<string> actual = new();
        foreach (string s in tree.LevelOrder())
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void TestDuplicates()
    {
        KdTree<Cord, int> tree = new(2);

        tree.Add(new Cord(1, 1), 1);
        tree.Add(new Cord(1, 1), 2);
        tree.Add(new Cord(1, 1), 3);
        tree.Add(new Cord(1, 1), 4);

        Assert.AreEqual(tree.Size, 4);

        List<int> expected = new();
        expected.Add(1);
        expected.Add(2);
        expected.Add(3);
        expected.Add(4);

        List<int> actual = new();
        foreach (int i in tree)
        {
            actual.Add(i);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void TestFind()
    {
        KdTree<Cord, string> tree = Create2dTreeEx1();
        List<string>? actual = tree.Find(new Cord(14, 39));

        List<string> expected = new();
        expected.Add("Senica");

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void TestFindWithDuplicates()
    {
        KdTree<Cord, string> tree = Create2dTreeEx1();
        tree.Add(new Cord(14, 39), "Žilina");

        List<string>? actual = tree.Find(new Cord(14, 39));
        actual!.Sort();

        List<string> expected = new();
        expected.Add("Senica");
        expected.Add("Žilina");
        expected.Sort();

        CollectionAssert.AreEqual(expected, actual);
    }
}