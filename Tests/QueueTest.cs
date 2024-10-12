using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace My.Tests;

using DataStructures.Queue;
public class QueueTest
{
    public static void TestAll()
    {
        Test1();
        Test2();
        Test3();
    }

    private static void Test1()
    {
        Queue<int> queue = new();

        queue.Add(1);
        int pop = queue.Pop();

        Assert.AreEqual(1, pop);
        Assert.AreEqual(0, queue.Size);
    }

    private static void Test2()
    {
        Queue<int> queue = new();

        queue.Add(1);
        queue.Add(2);
        int pop = queue.Pop();

        Assert.AreEqual(1, pop);
        Assert.AreEqual(1, queue.Size);
    }

    private static void Test3()
    {
        Queue<int> queue = new();

        queue.Add(1);
        queue.Add(2);
        queue.Add(3);
        queue.Add(4);
        queue.Add(5);

        queue.Pop();
        queue.Pop();
        int pop = queue.Pop();

        Assert.AreEqual(3, pop);
        Assert.AreEqual(2, queue.Size);

        pop = queue.Pop();
        Assert.AreEqual(4, pop);
        Assert.AreEqual(1, queue.Size);

        pop = queue.Pop();
        Assert.AreEqual(5, pop);
        Assert.AreEqual(0, queue.Size);

        queue.Add(6);
        Assert.AreEqual(1, queue.Size);

        pop = queue.Pop();
        Assert.AreEqual(6, pop);
        Assert.AreEqual(0, queue.Size);
    }
}