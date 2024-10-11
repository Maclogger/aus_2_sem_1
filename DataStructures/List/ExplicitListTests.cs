namespace My.DataStructures.List;

public class ExplicitListTests
{
    public static void TestAll(string[] args)
    {
        TestAdd();
        TestRemoveByValue();
        TestRemoveByIndex();
        TestFind();
        TestClear();
        TestGet();
        Console.WriteLine("Všetky testy sú dokončené.");
    }

    public static void TestAdd()
    {
        var list = new ExplicitList<int>();
        list.Add(1);
        list.Add(6);
        list.Add(2);

        if (list.Get(0) == 1 && list.Get(1) == 6 && list.Get(2) == 2)
        {
            Console.WriteLine("TestAdd prešiel.");
        }
        else
        {
            Console.WriteLine("TestAdd zlyhal.");
        }

    }

    public static void TestRemoveByValue()
    {
        var list = new ExplicitList<char>();
        list.Add('a');
        list.Add('b');
        list.Add('f');
        list.Remove('a');

        if (list.Size == 2 && list.Get(0) == 'b' && list.Get(1) == 'f')
        {
            Console.WriteLine("TestRemoveByValue prešiel.");
        }
        else
        {
            Console.WriteLine("TestRemoveByValue zlyhal.");
        }
    }

    public static void TestRemoveByIndex()
    {
        var list = new ExplicitList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Remove(1);  // Odstráň z indexu 1 (hodnota 2)

        if (list.Size == 2 && list.Get(0) == 1 && list.Get(1) == 3)
        {
            Console.WriteLine("TestRemoveByIndex prešiel.");
        }
        else
        {
            Console.WriteLine("TestRemoveByIndex zlyhal.");
        }
    }

    public static void TestFind()
    {
        var list = new ExplicitList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        if (list.Find(2) == 1 && list.Find(4) == null)
        {
            Console.WriteLine("TestFind prešiel.");
        }
        else
        {
            Console.WriteLine("TestFind zlyhal.");
        }
    }

    public static void TestClear()
    {
        var list = new ExplicitList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Clear();

        if (list.Size == 0)
        {
            Console.WriteLine("TestClear prešiel.");
        }
        else
        {
            Console.WriteLine("TestClear zlyhal.");
        }
    }

    public static void TestGet()
    {
        var list = new ExplicitList<int>();
        list.Add(1);
        list.Add(2);

        if (list.Get(1) == 2 && list.Get(5) == default(int))
        {
            Console.WriteLine("TestGet prešiel.");
        }
        else
        {
            Console.WriteLine("TestGet zlyhal.");
        }
    }
}