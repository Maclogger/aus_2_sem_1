using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures;

public class SelectSort<T> where T : IComparable
{
    public static T FindMedian(T[] array, int? pSeed = null)
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
        Random random;

        if (pSeed == null)
        {
            random = new Random();
        }
        else
        {
            random = new Random((int)pSeed);
        }
        T[] newArray = new T[array.Length];

        while (true)
        {
            if (endIndex - startIndex == 1)
            {
                // comparing last 2 items
                int comp = array[startIndex].CompareTo(array[endIndex]);
                if (comp > 0)
                {
                    // needs to swap last to elements
                    (array[startIndex], array[endIndex]) = (array[endIndex], array[startIndex]);
                }

                return array[medianIndex];
            }

            int pivotIndex = random.Next(startIndex, endIndex + 1);
            T pivot = array[pivotIndex];
            int left = startIndex;
            int right = endIndex;

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i == pivotIndex) {continue;}

                int comp = array[i].CompareTo(pivot);
                if (comp <= 0) // if array[i] <= pivot
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

            newArray[left] = array[pivotIndex];

            if (left == medianIndex)
            {
                return newArray[medianIndex];
            }

            if (medianIndex < left)
            {
                // going left from pivot
                endIndex = left - 1;
            }
            else
            {
                // going right from pivot
                startIndex = left + 1;
            }

            // copying all relevant items from newArray to array
            for (int i = startIndex; i <= endIndex; i++)
            {
                array[i] = newArray[i];
            }
        }
    }
}

public class SelectSortTest
{
    public static void RunAllTests()
    {
        List<Action> tests =
        [
            Test0, Test1, Test2, Test3, Test4, Test5, Test6, Test7, Test8,
            Test9, Test10, Test11, Test12, Test13, Test14, Test15, Test16, Test17, Test18
        ];

        int seedCount = 10_000;
        for (int seed = 1; seed < seedCount; seed++)
        {
            int oktests = 0;
            int testNumber = 1;
            foreach (Action test in tests)
            {
                try
                {
                    _seed = seed;
                    test();
                    oktests++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Test {testNumber} Failed: {ex.Message} pri seede: {seed}");
                }
                testNumber++;
            }

            if (oktests == tests.Count && seed % 10 == 0)
            {
                Console.WriteLine($"Seed {seed} / {seedCount} -> OK");
            }
        }

        long sum = 0;
        foreach (long ellapedTime in _ellapsedTimes)
        {
            sum += ellapedTime;
        }
        long averageTime = sum / _ellapsedTimes.Count;

        Console.WriteLine($"Average: {averageTime} milliseconds");
    }


    private static int _seed = 2;
    private static List<long> _ellapsedTimes = new();

    public static void Test0() => Assert.AreEqual(3, SelectSort<int>.FindMedian([3, 2, 7, 9, 1], _seed));
    public static void Test1() => Assert.AreEqual(5, SelectSort<int>.FindMedian([1, 2, 3, 4, 5, 6, 7, 8, 9], _seed));
    public static void Test2() => Assert.AreEqual(2, SelectSort<int>.FindMedian([1, 2, 3], _seed));
    public static void Test3() => Assert.AreEqual(8, SelectSort<int>.FindMedian([4, 8, 22], _seed));
    public static void Test4() => Assert.AreEqual(8, SelectSort<int>.FindMedian([22, 8, 4], _seed));
    public static void Test5() => Assert.AreEqual(8, SelectSort<int>.FindMedian([22, 4, 8], _seed));
    public static void Test6() => Assert.AreEqual(1, SelectSort<int>.FindMedian([1, 1, 1], _seed));
    public static void Test7() => Assert.AreEqual(1, SelectSort<int>.FindMedian([1], _seed));
    public static void Test8() => Assert.AreEqual(5, SelectSort<int>.FindMedian([9, 8, 7, 6, 5, 4, 3, 2, 1], _seed));
    public static void Test9() => Assert.AreEqual(1, SelectSort<int>.FindMedian([1, 1, 2, 2, 1, 2], _seed));
    public static void Test10() => Assert.AreEqual(1, SelectSort<int>.FindMedian([1, 2], _seed));
    public static void Test11() => Assert.AreEqual(2, SelectSort<int>.FindMedian([4, 2, 1, 4], _seed));
    public static void Test12() => Assert.AreEqual(0, SelectSort<int>.FindMedian([0, 0, 0, 0, 0, 0, 0, 0], _seed));
    public static void Test13() => Assert.AreEqual(7, SelectSort<int>.FindMedian([9, 8, 7, 6], _seed));
    public static void Test14() => Assert.AreEqual(10, SelectSort<int>.FindMedian([10, 5, 20, 15], _seed));
    public static void Test15() => RunLargeTest(500_000, 1_000, -1_000_000, 0, 1_000_000, 2_000_000, true, _seed);
    public static void Test16() => RunLargeTest(500_000, 1_000, -1_000_000, 300_000, 700_000, 1_000_000, false, _seed);
    public static void Test17() => RunLargeTest(150, 1_000, 0, 100, 200, 300, false, _seed);
    public static void Test18() => RunLargeTest(1, 1_000, 0, 0, 1, 1, false, _seed);

    private static void RunLargeTest(int pMedianExpected, int pCount, int pMin1, int pMax1, int pMin2, int pMax2, bool pEven, int pSeed)
    {
        Random random = new Random(pSeed);
        List<int> list = [pMedianExpected];

        int leftSideCount = pEven ? pCount - 1 : pCount;

        for (int i = 0; i < leftSideCount; i++)
        {
            list.Add(random.Next(pMin1, pMax1));
        }
        for (int i = 0; i < pCount; i++)
        {
            list.Add(random.Next(pMin2, pMax2));
        }

        int[] ints = Shuffle(list, random);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        int median = SelectSort<int>.FindMedian(ints);
        sw.Stop();

        _ellapsedTimes.Add(sw.ElapsedMilliseconds);

        Assert.AreEqual(pMedianExpected, median);
    }

    private static int[] Shuffle(List<int> list, Random random)
    {
        return list.OrderBy(x => random.Next()).ToArray();
    }
}
