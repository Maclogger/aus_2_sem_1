using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.Stack;

public class StackTests
{
    private static readonly Random RandomInstance = new Random();

    public static void RunAllTests()
    {
        Test();
        Test2();
        Test3();
        Test4();
    }

    public static void Test()
    {
        Stack<int> stack = new();

        stack.Push(1);
        int popped = stack.Pop();

        Assert.AreEqual(popped, 1);
    }

    public static void Test2()
    {
        Stack<int> stack = new();

        stack.Push(1);
        stack.Push(2);
        int popped = stack.Pop();

        Assert.AreEqual(popped, 2);
    }

    public static void Test3()
    {
        Stack<int> stack = new();

        stack.Push(2);
        stack.Push(1);
        stack.Pop();
        int popped = stack.Pop();

        Assert.AreEqual(popped, 2);
    }

    public static void Test4()
    {
        Stack<int> stack = new();

        Random randomInstance = new Random();
        List<int> list = new();
        List<int> list2 = new();

        int count = 1_000_000_000;
        for (int i = 0; i < count; i++)
        {
            int randomNumber = Utils.GetRandomIntInRange(-1000, 1000, randomInstance);
            list.Add(randomNumber);
            stack.Push(randomNumber);
        }

        for (int i = 0; i < count; i++)
        {
            int var = stack.Pop();
            list2.Add(var);
        }

        list2.Reverse();

        CollectionAssert.AreEqual(list2, list);
    }
}