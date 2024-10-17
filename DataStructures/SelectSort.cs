using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace My.DataStructures;

public class SelectSort<T> where T : IComparable
{
    public static T FindMedian(T[] array)
    {
        if (array.Length == 0)
        {
            throw new ArgumentException("Array is empty", nameof(array));
        }

        if (array.Length == 1)
        {
            return array[0];
        }

        // if the count is even => returning the left index
        int medianIndex = array.Length / 2 - (array.Length % 2 == 0 ? 1 : 0);
        int startIndex = 0;
        int endIndex = array.Length - 1;

        while (true)
        {
            T pivot = array[endIndex]; // TODO should be random
            int left = startIndex;
            int right = endIndex;
            T[] newArray = new T[array.Length];

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (array[i].CompareTo(pivot) <= 0) // if array[i] < pivot
                {
                    newArray[left] = array[i];
                    left++;
                }
                else
                {
                    newArray[right] = array[i];
                    right--;
                }
            }

            left--; // because pivot was also inserted (<= 0)

            if (left == medianIndex)
            {
                return newArray[medianIndex];
            }

            if (left > medianIndex)
            {
                endIndex = left - 1; // going left
            }
            else
            {
                startIndex = left + 1; // goint right
            }

            array = newArray;
        }
    }
}


public class SelectSortTest
{
    public static void RunAllTests()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Test7();
        Test8();
        Test9();
        Test10();
        Test11();
        Test12();
        Test13();
        Test14();
        Test15();
        Test16();
    }

    private static void Test16()
    {
        // even number test
        Random random = new Random(13);
        List<int> list = new List<int>();
        list.Add(500_000);

        int count = 5_000;

        for (int i = 0; i < count - 1; i++)
        {
            list.Add(random.Next(-1_000_000, 300_000));
        }
        for (int i = 0; i < count; i++)
        {
            list.Add(random.Next(700_000, 1_000_000));
        }

        int[] ints = Shuffle(list, random);

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(500_000, median);
    }

    private static void Test10()
    {
        int[] ints = {
            1, 2
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(1, median);
    }

    private static void Test11()
    {
        int[] ints = {
            4, 2, 1, 4
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(2, median);
    }

    private static void Test12()
    {
        int[] ints = {
            0, 0, 0, 0, 0, 0, 0, 0
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(0, median);
    }

    private static void Test13()
    {
        int[] ints = {
            9, 8, 7, 6
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(7, median);
    }

    public static void Test14()
    {
        int[] ints = {
            10, 5, 20, 15
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(10, median);
    }

    private static void Test1()
    {
        int[] ints = {
            1, 2, 3, 4, 5, 6, 7, 8, 9
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(5, median);
    }

    private static void Test2()
    {
        int[] ints = {
            1, 2, 3
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(2, median);
    }

    private static void Test3()
    {
        int[] ints = {
            4, 8, 22
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(8, median);
    }

    private static void Test4()
    {
        int[] ints = {
            22, 8, 4
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(8, median);
    }

    private static void Test5()
    {
        int[] ints = {
            22, 4, 8
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(8, median);
    }

    private static void Test6()
    {
        int[] ints = {
            1, 1, 1
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(1, median);
    }

    private static void Test7()
    {
        int[] ints = {
            1
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(1, median);
    }

    private static void Test8()
    {
        int[] ints = {
            9, 8, 7, 6, 5, 4, 3, 2, 1
        };

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(5, median);
    }

    public static void Test9()
    {
        // odd number test
        int count = 100_000;

        Random random = new Random(13);
        List<int> list = new List<int>(count);
        list.Add(150);


        for (int i = 0; i < count; i++)
        {
            list.Add(random.Next(0, 100));
        }
        for (int i = 0; i < count; i++)
        {
            list.Add(random.Next(200, 300));
        }

        int[] ints = Shuffle(list, random);

        Stopwatch sw = new Stopwatch();
        Console.WriteLine("stopwatch start");
        sw.Start();
        int median = SelectSort<int>.FindMedian(ints);
        sw.Stop();

        Assert.AreEqual(150, median);
        Console.WriteLine(sw.Elapsed);
    }

    private static void Test15()
    {
        // even number test
        Random random = new Random(13);
        List<int> list = new List<int>();
        list.Add(150);

        int count = 100000;

        for (int i = 0; i < count - 1; i++)
        {
            list.Add(random.Next(0, 100));
        }
        for (int i = 0; i < count; i++)
        {
            list.Add(random.Next(200, 300));
        }

        int[] ints = Shuffle(list, random);

        int median = SelectSort<int>.FindMedian(ints);

        Assert.AreEqual(150, median);
    }

    private static int[] Shuffle(List<int> list, Random random)
    {
       return list.OrderBy(x => random.Next()).ToArray();
    }
}