using System;
using System.Collections.Generic;
using System.Diagnostics;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests;

public class MyIntKey : IKey
{
    private int _value;

    public MyIntKey(int pValue)
    {
        Value = pValue;
    }

    public int Value
    {
        get => _value;
        set => _value = value;
    }

    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not MyIntKey myIntKey)
        {
            throw new ArgumentException("Object is not an IntItem");
        }

        if (Value == myIntKey.Value)
        {
            return 0;
        }

        return Value < myIntKey.Value ? -1 : 1;
    }

    public bool Equals(IKey pOther)
    {
        if (pOther is not MyIntKey myIntKey) return false;
        return Value == myIntKey.Value;
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

    public Cord(Random random)
    {
        _x = random.Next(int.MinValue, int.MaxValue);
        _y = random.Next(int.MinValue, int.MaxValue);
    }

    public int X
    {
        get => _x;
    }

    public int Y
    {
        get => _y;
    }

    public override string ToString()
    {
        return $"[{_x};{_y}]";
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
        }
        else if (pDimension == 1)
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
        if (pOther is not Cord cord) return false;
        return _x == cord.X && _y == cord.Y;
    }
}

public class KeyInt
{
    private IKey _key;
    private int _data;

    public KeyInt(IKey key, int data)
    {
        _key = key;
        _data = data;
    }

    public IKey Key
    {
        get => _key;
        set => _key = value;
    }

    public int Data
    {
        get => _data;
        set => _data = value;
    }
}

public class SimulationTester
{
    private double _probAdd;
    private double _probFind;
    private double _probUpdate;
    private double _probRemove;
    private double _probOfRemovingExistingElement;
    private int _checkAfterOperationCount;
    private readonly bool _shouldPrint;

    public SimulationTester(double pProbAdd = 0.25, double pProbFind = 0.25,
        double pProbUpdate = 0.25, double pProbRemove = 0.25, double pProbOfRemovingExistingElement = 0.7,
        int pCheckAfterOperationCount = 100, bool pShouldPrint = true)
    {
        _probAdd = pProbAdd;
        _probFind = pProbFind;
        _probUpdate = pProbUpdate;
        _probRemove = pProbRemove;
        _probOfRemovingExistingElement = pProbOfRemovingExistingElement;
        _checkAfterOperationCount = pCheckAfterOperationCount;
        _shouldPrint = pShouldPrint;
    }

    public void Run2DTest(int pSeed = 1, int pCount = 100, TextBlock? block = null)
    {
        if (block == null) return;
        int seed = pSeed;
        int count = pCount;

        OperationGenerator opGen = new(probAdd: _probAdd, probFind: _probFind, probUpdate: _probUpdate,
            probRemove: _probRemove);
        Random gen = new(seed); // for generating random numbers
        Random genForRemovingExistingElement = new(seed * 2); // for generating random numbers
        Random genForRandomOperation = new(seed * 2); // for generating random numbers

        List<KeyInt> expectedInTree = new();

        KdTree<Cord, int> tree = new(2);

        Cord notExisting = new Cord(gen); // randomly generated 1 node which will never exist in tree

        int countBeforeTest = Config.Instance.ElementCountBeforeTest;
        for (int i = 0; i < countBeforeTest; i++)
        {
            Cord randomKey;
            while (true)
            {
                randomKey =
                    new Cord(gen); // random generated new Key4D (could be existing although the probability is low)
                if (!notExisting.Equals(randomKey))
                {
                    break; // the probability is low but not zero => now is zero
                }
            }

            int randomValue = genForRandomOperation.Next(-100000, 100000);

            tree.Add(randomKey, randomValue); // adding randomly generated Key4D into tree, data is just an i
            expectedInTree.Add(new KeyInt(randomKey, randomValue));
        }

        for (int i = 0; i < count; i++)
        {
            Operation op = opGen.GetRandomOperation(genForRandomOperation);
            if (op == Operation.Add)
            {
                Cord randomKey;
                while (true)
                {
                    randomKey =
                        new Cord(gen); // random generated new Cord (could be existing although the probability is low)
                    if (randomKey.X != notExisting.X || randomKey.Y != notExisting.Y)
                    {
                        break; // the probability is low but not zero => now is zero (ignoring
                    }
                }

                tree.Add(randomKey, i); // adding randomly generated Cord into tree, data is just an i

                expectedInTree.Add(new KeyInt(randomKey, i));
                if (_shouldPrint)
                {
                    Console.WriteLine($"Adding element {randomKey} {i}");
                    block.Text += $"Adding element {randomKey} {i}\n";
                }
            }
            else if (op == Operation.Remove)
            {
                if (expectedInTree.Count > 0 &&
                    genForRemovingExistingElement.NextDouble() < _probOfRemovingExistingElement)
                {
                    int indexOfElementToRemove = gen.Next(0, expectedInTree.Count);
                    KeyInt randomExistingElement = expectedInTree[indexOfElementToRemove];

                    List<DataPart<int>> itemsWithMatchingKey = tree.FindDataParts((Cord)randomExistingElement.Key);

                    if (itemsWithMatchingKey == null)
                    {
                        throw new KeyNotFoundException(
                            "The node was in expected list, but was not found by the KdTree. :(");
                    }

                    int j = 0;
                    for (; j < itemsWithMatchingKey.Count; j++)
                    {
                        if (itemsWithMatchingKey[j].Value == randomExistingElement.Data)
                        {
                            break;
                        }
                    }

                    tree.Remove((Cord)randomExistingElement.Key,
                        itemsWithMatchingKey[j].Uid); // removing item from the tree
                    expectedInTree.RemoveAt(indexOfElementToRemove);
                    if (_shouldPrint)
                    {
                        Console.WriteLine(
                            $"Removing element {(Cord)randomExistingElement.Key} {randomExistingElement.Data} at index {indexOfElementToRemove}");
                        block.Text += $"Removing element {randomExistingElement.Key} {randomExistingElement.Data}\n";
                    }
                }
                else
                {
                    // removing not existing element
                    tree.Remove(notExisting, 0); // removing node which doesn't exist (shouldn't change anything)
                    if (_shouldPrint)
                    {
                        Console.WriteLine("Removing a not existing element");
                        block.Text += $"Removing a not existing element\n";
                    }
                }
            }
            else if (op == Operation.Update)
            {
                if (expectedInTree.Count > 0)
                {
                    int indexOfElementToUpdate = gen.Next(0, expectedInTree.Count);
                    KeyInt randomExistingElement = expectedInTree[indexOfElementToUpdate];

                    List<DataPart<int>> itemsWithMatchingKey = tree.FindDataParts((Cord)randomExistingElement.Key);

                    if (itemsWithMatchingKey == null)
                    {
                        throw new KeyNotFoundException(
                            "The node was in expected list, but was not found by the KdTree. :(");
                    }

                    int j = 0;
                    for (; j < itemsWithMatchingKey.Count; j++)
                    {
                        if (itemsWithMatchingKey[j].Value == randomExistingElement.Data)
                        {
                            break;
                        }
                    }

                    int newData = genForRandomOperation.Next();
                    tree.Update((Cord)randomExistingElement.Key, itemsWithMatchingKey[j].Uid,
                        newData); // updating item from the tree
                    expectedInTree[indexOfElementToUpdate].Data = newData;
                    if (_shouldPrint)
                    {
                        Console.WriteLine(
                            $"Updating element {(Cord)randomExistingElement.Key} {randomExistingElement.Data} at index {indexOfElementToUpdate}");
                        block.Text +=
                            $"Updating element {(Cord)randomExistingElement.Key} {randomExistingElement.Data}\n";
                    }
                }
            }
            else if (op == Operation.Find)
            {
                if (expectedInTree.Count > 0)
                {
                    int indexOfElementToUpdate = gen.Next(0, expectedInTree.Count);
                    KeyInt randomExistingElement = expectedInTree[indexOfElementToUpdate];

                    List<int> found = tree.Find((Cord)randomExistingElement.Key);

                    if (!found.Contains(randomExistingElement.Data))
                    {
                        throw new Exception("The node was not in expected list");
                    }
                }
            }

            if (i % _checkAfterOperationCount == 0)
            {
                CheckCorrectness(expectedInTree, tree);
                if (_shouldPrint) Console.WriteLine($"{pSeed}: {i + _checkAfterOperationCount} / {count}");
            }

            //if (_shouldPrint) tree.Print();
        }

        CheckCorrectness(expectedInTree, tree);
        if (_shouldPrint) Console.WriteLine(tree.Size);
    }


    public void CheckCorrectness<K>(List<KeyInt> pExpectedInTree, KdTree<K, int> pTree) where K : IKey
    {
        Assert.AreEqual(pExpectedInTree.Count, pTree.Size);

        List<int> actual = new();
        List<int> expected = new();

        foreach (int data in pTree)
        {
            actual.Add(data);
        }

        foreach (KeyInt c in pExpectedInTree)
        {
            expected.Add(c.Data);
        }

        actual.Sort();
        expected.Sort();

        CollectionAssert.AreEqual(actual, expected);
    }

    public void Run4DTest(int pSeed = 1, int pCount = 100, TextBlock? block = null)
    {
        if (block == null) return;
        int seed = pSeed;
        int count = pCount;

        OperationGenerator opGen = new(probAdd: _probAdd, probFind: _probFind, probUpdate: _probUpdate,
            probRemove: _probRemove);
        Random gen = new(seed); // for generating random numbers
        Random genForRemovingExistingElement = new(seed * 2); // for generating random numbers
        Random genForRandomOperation = new(seed * 2); // for generating random numbers

        List<KeyInt> expectedInTree = new();

        KdTree<Key4D, int> tree = new(4);

        Key4D notExisting = new Key4D(gen); // randomly generated 1 node which will never exist in tree

        int countBeforeTest = Config.Instance.ElementCountBeforeTest;
        for (int i = 0; i < countBeforeTest; i++)
        {
            Key4D randomKey;
            while (true)
            {
                randomKey =
                    new Key4D(gen); // random generated new Key4D (could be existing although the probability is low)
                if (!notExisting.Equals(randomKey))
                {
                    break; // the probability is low but not zero => now is zero
                }
            }

            int randomValue = genForRandomOperation.Next(10, 10);

            tree.Add(randomKey, randomValue); // adding randomly generated Key4D into tree, data is just an i
            expectedInTree.Add(new KeyInt(randomKey, randomValue));
        }


        for (int i = 0; i < count; i++)
        {
            Operation op = opGen.GetRandomOperation(genForRandomOperation);
            if (op == Operation.Add)
            {
                Key4D randomKey;
                while (true)
                {
                    randomKey =
                        new Key4D(gen); // random generated new Key4D (could be existing although the probability is low)
                    if (!notExisting.Equals(randomKey))
                    {
                        break; // the probability is low but not zero => now is zero
                    }
                }

                int randomData = genForRandomOperation.Next(-100000, 100000);
                tree.Add(randomKey, randomData);

                expectedInTree.Add(new KeyInt(randomKey, randomData));
                if (_shouldPrint)
                {
                    Console.WriteLine($"Adding element {randomKey} {i}");
                    block.Text += $"Adding element {randomKey} {i}\n";
                }
            }
            else if (op == Operation.Remove)
            {
                if (expectedInTree.Count > 0 &&
                    genForRemovingExistingElement.NextDouble() < _probOfRemovingExistingElement)
                {
                    int indexOfElementToRemove = gen.Next(0, expectedInTree.Count);
                    KeyInt randomExistingElement = expectedInTree[indexOfElementToRemove];

                    List<DataPart<int>> itemsWithMatchingKey = tree.FindDataParts((Key4D)randomExistingElement.Key);

                    if (itemsWithMatchingKey == null)
                    {
                        throw new KeyNotFoundException(
                            "The node was in expected list, but was not found by the KdTree. :(");
                    }

                    int j = 0;
                    for (; j < itemsWithMatchingKey.Count; j++)
                    {
                        if (itemsWithMatchingKey[j].Value == randomExistingElement.Data)
                        {
                            break;
                        }
                    }

                    tree.Remove((Key4D)randomExistingElement.Key,
                        itemsWithMatchingKey[j].Uid); // removing item from the tree
                    expectedInTree.RemoveAt(indexOfElementToRemove);
                    if (_shouldPrint)
                    {
                        Console.WriteLine(
                            $"Removing element {(Key4D)randomExistingElement.Key} {randomExistingElement.Data} at index {indexOfElementToRemove}");
                        block.Text += $"Removing element {randomExistingElement.Key} {randomExistingElement.Data}\n";
                    }
                }
                else
                {
                    // removing not existing element
                    tree.Remove(notExisting, 0); // removing node which doesn't exist (shouldn't change anything)
                    if (_shouldPrint)
                    {
                        Console.WriteLine("Removing a not existing element");
                        block.Text += $"Removing a not existing element\n";
                    }
                }
            }
            else if (op == Operation.Update)
            {
                if (expectedInTree.Count > 0)
                {
                    int indexOfElementToUpdate = gen.Next(0, expectedInTree.Count);
                    KeyInt randomExistingElement = expectedInTree[indexOfElementToUpdate];

                    List<DataPart<int>> itemsWithMatchingKey = tree.FindDataParts((Key4D)randomExistingElement.Key);

                    if (itemsWithMatchingKey == null)
                    {
                        throw new KeyNotFoundException(
                            "The node was in expected list, but was not found by the KdTree. :(");
                    }

                    int j = 0;
                    for (; j < itemsWithMatchingKey.Count; j++)
                    {
                        if (itemsWithMatchingKey[j].Value == randomExistingElement.Data)
                        {
                            break;
                        }
                    }

                    int newData = genForRandomOperation.Next();
                    tree.Update((Key4D)randomExistingElement.Key, itemsWithMatchingKey[j].Uid,
                        newData); // updating item from the tree
                    expectedInTree[indexOfElementToUpdate].Data = newData;
                    if (_shouldPrint)
                    {
                        Console.WriteLine(
                            $"Updating element {(Key4D)randomExistingElement.Key} {randomExistingElement.Data} at index {indexOfElementToUpdate}");
                        block.Text +=
                            $"Updating element {(Key4D)randomExistingElement.Key} {randomExistingElement.Data}\n";
                    }
                }
            }
            else if (op == Operation.Find)
            {
                if (expectedInTree.Count > 0)
                {
                    int indexOfElementToUpdate = gen.Next(0, expectedInTree.Count);
                    KeyInt randomExistingElement = expectedInTree[indexOfElementToUpdate];

                    List<int> found = tree.Find((Key4D)randomExistingElement.Key);

                    if (!found.Contains(randomExistingElement.Data))
                    {
                        throw new Exception("The node was not in expected list");
                    }
                }
            }

            if (i % _checkAfterOperationCount == 0)
            {
                CheckCorrectness(expectedInTree, tree);
                if (_shouldPrint) Console.WriteLine($"{pSeed}: {i + _checkAfterOperationCount} / {count}");
            }

            //if (_shouldPrint) tree.Print();
        }

        CheckCorrectness(expectedInTree, tree);
        if (_shouldPrint) Console.WriteLine(tree.Size);
    }
}

public class KdTreeTest
{
    public static void RunAllTests()
    {
        RemoveRightTest();
        return;
        SwapTest();
        TestPrint();
        // Add
        TestAdd();
        TestAddRandom();
        TestAddRandom2D();
        TestAddRandomSize();
        TestAddDuplicates();
        // Iterators
        TestInOrder();
        TestLevelOrder();
        // Other
        TestFind();
        TestFindWithDuplicates();
        // Operation Generator
        //TestWithOperationGenerator();
        SwapTest();
        RandomizedSwapTest();
        // Remove
        // Leaf
        RemoveTest();
        // Has left son
        RemoveTest0();
        RemoveTest1();
        RemoveTest2();
        RemoveTest3();
        RemoveTheWholeLeftSubTreeTest();
        // Doesn't have left son
    }

    public static void RemoveRightTest()
    {
        KdTree<Cord, string> tree = new(2);

        tree.Add(new Cord(22, 39), "Senica");

        tree.Add(new Cord(24, 36), "Tlmače - úrad");

        tree.Add(new Cord(24, 34), "Tlmače");
        tree.Add(new Cord(24, 40), "Tlmače - parkovisko");
        tree.Add(new Cord(24, 35), "Tlmače - nem.");

        tree.Add(new Cord(30, 33), "Levice");
        tree.Add(new Cord(29, 46), "Bojnice");

        tree.Add(new Cord(27, 43), "Nováky");

        tree.Remove(new Cord(24, 40), 0);
        tree.Remove(new Cord(27, 43), 0);
        tree.Remove(new Cord(22, 39), 0);
        tree.Remove(new Cord(30, 33), 0);
        tree.Remove(new Cord(24, 36), 0);
        tree.Remove(new Cord(24, 34), 0);
        tree.Remove(new Cord(24, 35), 0);
        tree.Remove(new Cord(29, 46), 0);

        Assert.AreEqual(0, tree.Size);
    }

    public static void RemoveTheWholeLeftSubTreeTest()
    {
        KdTree<Cord, string> tree = SetUpRemoveTreeTest1();

        tree.Remove(new Cord(12, 41), 0);
        tree.Remove(new Cord(22, 32), 0);
        tree.Remove(new Cord(22, 31), 0);
        tree.Remove(new Cord(22, 42), 0);
        tree.Remove(new Cord(22, 39), 0);

        List<string> expected = new();
        expected.Add("Nitra");
        expected.Add("Tlmače - nem.");
        expected.Add("Tlmače");
        expected.Add("Levice");
        expected.Add("Tlmače - úrad");
        expected.Add("Tlmače - parkovisko");
        expected.Add("Nováky");
        expected.Add("Bojnice");

        List<string> actual = new();
        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void RemoveTest()
    {
        KdTree<Cord, string> tree = SetUpRemoveTreeTest1();

        tree.Remove(new Cord(27, 43), 0);

        List<string> expected = new();
        expected.Add("Senica - úrad");
        expected.Add("Senica - škola");
        expected.Add("Senica");
        expected.Add("Hodonín");
        expected.Add("Senica - stanica");
        expected.Add("Nitra");
        expected.Add("Tlmače - nem.");
        expected.Add("Tlmače");
        expected.Add("Levice");
        expected.Add("Tlmače - úrad");
        expected.Add("Tlmače - parkovisko");
        expected.Add("Bojnice");

        List<string> actual = new();
        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void RemoveTest0()
    {
        KdTree<Cord, string> tree = SetUpRemoveTreeTest1();

        tree.Remove(new Cord(29, 46), 0);

        List<string> expected = new();
        expected.Add("Senica - úrad");
        expected.Add("Senica - škola");
        expected.Add("Senica");
        expected.Add("Hodonín");
        expected.Add("Senica - stanica");
        expected.Add("Nitra");
        expected.Add("Tlmače - nem.");
        expected.Add("Tlmače");
        expected.Add("Levice");
        expected.Add("Tlmače - úrad");
        expected.Add("Tlmače - parkovisko");
        expected.Add("Nováky");

        List<string> actual = new();
        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void RemoveTest1()
    {
        KdTree<Cord, string> tree = SetUpRemoveTreeTest1();

        tree.Remove(new Cord(23, 35), 0);

        List<string> expected = new();
        expected.Add("Senica - škola");
        expected.Add("Senica");
        expected.Add("Hodonín");
        expected.Add("Senica - stanica");
        expected.Add("Senica - úrad");
        expected.Add("Tlmače - nem.");
        expected.Add("Tlmače");
        expected.Add("Levice");
        expected.Add("Tlmače - úrad");
        expected.Add("Tlmače - parkovisko");
        expected.Add("Nováky");
        expected.Add("Bojnice");

        List<string> actual = new();
        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void RemoveTest2()
    {
        KdTree<Cord, string> tree = SetUpRemoveTreeTest1();

        tree.Remove(new Cord(22, 42), 0);

        List<string> expected = new();
        expected.Add("Senica - úrad");
        expected.Add("Senica - škola");
        expected.Add("Senica");
        expected.Add("Hodonín");
        expected.Add("Nitra");
        expected.Add("Tlmače - nem.");
        expected.Add("Tlmače");
        expected.Add("Levice");
        expected.Add("Tlmače - úrad");
        expected.Add("Tlmače - parkovisko");
        expected.Add("Nováky");
        expected.Add("Bojnice");

        List<string> actual = new();
        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    public static void RemoveTest3()
    {
        KdTree<Cord, string> tree = SetUpRemoveTreeTest1();

        tree.Remove(new Cord(24, 36), 0);

        List<string> expected = new();
        expected.Add("Senica - úrad");
        expected.Add("Senica - škola");
        expected.Add("Senica");
        expected.Add("Hodonín");
        expected.Add("Senica - stanica");
        expected.Add("Nitra");
        expected.Add("Tlmače");
        expected.Add("Levice");
        expected.Add("Tlmače - nem.");
        expected.Add("Tlmače - parkovisko");
        expected.Add("Nováky");
        expected.Add("Bojnice");

        List<string> actual = new();
        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
    }

    private static KdTree<Cord, string> SetUpRemoveTreeTest1()
    {
        KdTree<Cord, string> tree = new(2);

        tree.Add(new Cord(23, 35), "Nitra");

        tree.Add(new Cord(22, 39), "Senica");
        tree.Add(new Cord(24, 36), "Tlmače - úrad");

        tree.Add(new Cord(22, 31), "Senica - škola");
        tree.Add(new Cord(22, 42), "Senica - stanica");
        tree.Add(new Cord(24, 34), "Tlmače");
        tree.Add(new Cord(24, 40), "Tlmače - parkovisko");

        tree.Add(new Cord(22, 32), "Senica - úrad");
        tree.Add(new Cord(12, 41), "Hodonín");
        tree.Add(new Cord(24, 35), "Tlmače - nem.");
        tree.Add(new Cord(30, 33), "Levice");
        tree.Add(new Cord(29, 46), "Bojnice");

        tree.Add(new Cord(27, 43), "Nováky");

        return tree;
    }

    private static void TestPrint()
    {
        KdTree<Cord, string> tree = Create2dTreeEx1();
        tree.Add(new Cord(20, 33), "Sereďovina");
        tree.Print();
    }

    private static void SwapInList<T>(List<T> pList, int pIndex1, int pIndex2)
    {
        (pList[pIndex1], pList[pIndex2]) = (pList[pIndex2], pList[pIndex1]);
    }

    public static void RandomizedSwapTest()
    {
        int count = 1000;
        int seedCount = 100;
        for (int seed = 1; seed < seedCount; seed++)
        {
            Random random = new(seed);
            for (int i = 0; i < count; i++)
            {
                List<Tuple<Cord, string>> listOfTuples = new();
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(13, 32), "Bratislava"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(16, 31), "Galanta"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(20, 33), "Sereď"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(12, 41), "Hodonín"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(14, 39), "Senica"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(17, 42), "Trnava"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(23, 35), "Nitra"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(26, 35), "Moravce"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(28, 34), "Tlmače"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(30, 33), "Levice"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(25, 36), "Topoľčianky"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(24, 40), "Bošany"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(27, 43), "Nováky"));
                listOfTuples.Add(new Tuple<Cord, string>(new Cord(29, 46), "Bojnice"));

                int randomIndex1 = random.Next(0, listOfTuples.Count);
                int randomIndex2 = random.Next(0, listOfTuples.Count);

                KdTree<Cord, string> tree = Create2dTreeEx1();

                tree.Swap(listOfTuples[randomIndex1].Item1, listOfTuples[randomIndex2].Item1);
                SwapInList(listOfTuples, randomIndex1, randomIndex2);

                List<string> expected = new();
                List<string> actual = new();

                foreach (Tuple<Cord, string> tuple in listOfTuples)
                {
                    expected.Add(tuple.Item2);
                }

                foreach (string s in tree)
                {
                    actual.Add(s);
                }

                CollectionAssert.AreEqual(expected, actual);
            }
            //Console.WriteLine($"{seed} / {seedCount} OK");
        }

        Console.WriteLine("ok");
    }

    public static void SwapTest()
    {
        TestSwapUni([
                "Bratislava",
                "Galanta",
                "Nitra",
                "Hodonín",
                "Senica",
                "Trnava",
                "Sereď",
                "Moravce",
                "Tlmače",
                "Levice",
                "Topoľčianky",
                "Bošany",
                "Nováky",
                "Bojnice"
            ], new Cord(20, 33), new Cord(23, 35)
        );
        TestSwapUni([
                "Bratislava",
                "Galanta",
                "Sereď",
                "Hodonín",
                "Senica",
                "Trnava",
                "Nitra",
                "Tlmače",
                "Moravce",
                "Levice",
                "Topoľčianky",
                "Bošany",
                "Nováky",
                "Bojnice"
            ], new Cord(26, 35), new Cord(28, 34)
        );
        TestSwapUni([
                "Bratislava",
                "Galanta",
                "Topoľčianky",
                "Hodonín",
                "Senica",
                "Trnava",
                "Nitra",
                "Moravce",
                "Tlmače",
                "Levice",
                "Sereď",
                "Bošany",
                "Nováky",
                "Bojnice"
            ], new Cord(20, 33), new Cord(25, 36)
        );
    }

    private static void TestSwapUni(List<string> expected, Cord cord1, Cord cord2)
    {
        KdTree<Cord, string> tree = Create2dTreeEx1();
        tree.Swap(cord1, cord2);

        List<string> actual = new(14);

        foreach (string s in tree)
        {
            actual.Add(s);
        }

        CollectionAssert.AreEqual(expected, actual);
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
        // 14

        return tree;
    }

    public static void TestAdd()
    {
        KdTree<Cord, string> tree = Create2dTreeEx1();

        int size = tree.Size;
        Assert.AreEqual(size, 14);
    }

    public static void TestAddRandom()
    {
        int count = 100_000;
        Random random = new();
        KdTree<MyIntKey, int> tree = SetUpRandomIntTree(count, -10_000, 10_000);

        Assert.AreEqual(tree.Size, count);
    }

    public static void TestAddRandomSize()
    {
        int count = Utils.GetRandomIntInRange(1_000, 1_000_000);
        Random random = new();
        KdTree<MyIntKey, int> tree = SetUpRandomIntTree(count, -10_000, 10_000);

        Assert.AreEqual(tree.Size, count);
    }

    public static void TestInOrder()
    {
        KdTree<Cord, string> tree = Create2dTreeEx1();
        List<string> expected =
        [
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
        KdTree<Cord, string> tree = Create2dTreeEx1();
        List<string> expected =
        [
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

    public static void TestAddDuplicates()
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
        expected.Add("Tlmače");
        expected.Add("Bošany");

        actual.AddRange(tree.Find(new Cord(28, 34)));
        actual.AddRange(tree.Find(new Cord(24, 40)));

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

    public static void TestWithOperationGenerator()
    {
        int operationCount = 100_000;
        OperationGenerator opGen = new(probAdd: 1.0, probFind: 0.0, probUpdate: 0.0, probRemove: 0.0);
        Random gen = new();
        List<Tuple<Cord, int>> inserted = new();
        List<int> found = new();
        double probOfFindingExistingElement = 0.9;

        KdTree<Cord, int> tree = new(2);

        for (int i = 0; i < operationCount; i++)
        {
            Operation op = opGen.GetRandomOperation();
            if (op == Operation.Add)
            {
                // Add a random Cord and index to the tree and list
                Cord randomCord = new Cord(gen);
                tree.Add(randomCord, i);
                inserted.Add(new Tuple<Cord, int>(randomCord, i));
            }
            else if (op == Operation.Find)
            {
                double randomNumber = gen.NextDouble();
                if (inserted.Count > 0 && randomNumber < probOfFindingExistingElement)
                {
                    // Pick a random item from the list of inserted elements
                    int randomIndex = gen.Next(0, inserted.Count);
                    Tuple<Cord, int> tuple = inserted[randomIndex];
                    List<int>? listOfResults = tree.Find(tuple.Item1);

                    // Check that the found list is not null or empty and contains the expected item
                    Assert.IsNotNull(listOfResults, "The result list should not be null.");
                    Assert.IsTrue(listOfResults.Count > 0, "The result list should not be empty.");
                    Assert.IsTrue(listOfResults.Contains(tuple.Item2),
                        $"The value {tuple.Item2} should be in the result list.");

                    found.Add(tuple.Item2);
                }
                else
                {
                    // Generate a random Cord that was not inserted before
                    Cord randomCord = new Cord(gen);
                    List<int>? listOfResults = tree.Find(randomCord);

                    // Check that the list should be empty for a new item
                    Assert.IsTrue(listOfResults == null || listOfResults.Count == 0,
                        "A new item should not be found.");
                }
            }
        }
    }

    public static void TestAddRandom2D()
    {
        Random random = new();
        int count = 1_000_000;
        KdTree<Cord, string> tree = new(2);
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < count; i++)
        {
            Cord cord = new Cord(random);
            tree.Add(cord, $"Data {i}");
        }

        sw.Stop();
        Assert.AreEqual(tree.Size, count);
        Console.WriteLine(sw.ElapsedMilliseconds);
        /*
        foreach (string s in tree)
        {
            Console.WriteLine(s);
        }
    */
    }
}