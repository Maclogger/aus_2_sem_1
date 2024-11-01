/*using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
using Avalonia.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree;

public static class SimulationTester
{
    private static Key4D _notExistingKey = new Key4D(0.0, "not_existing", 0, 0.0);

    public static void CheckCorrectness<K>(List<KeyInt> pExpectedInTree, KdTree<K, int> pTree) where K : IKey
    {
        Assert.AreEqual(pExpectedInTree.Count, pTree.Size);

        List<int> actual = new(pExpectedInTree.Count);
        List<int> expected = new(pExpectedInTree.Count);

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

    private static Tuple<KdTree<Key4D, int>, List<KeyInt>> Create4DTree()
    {
        int countBeforeTest = Config.Instance.ElementCountBeforeTest;

        Random gen = new();
        KdTree<Key4D, int> tree = new(2);
        List<KeyInt> expectedInTree = new(countBeforeTest);

        for (int i = 0; i < countBeforeTest; i++)
        {
            Key4D
                randomKey = new Key4D(
                    gen); // random generated new Key4D (could be existing although the probability is low)
            int randomValue = gen.Next(10, 10);

            tree.Add(randomKey, randomValue); // adding randomly generated Key4D into tree, data is just an i
            expectedInTree.Add(new KeyInt(randomKey, randomValue));
        }

        return new Tuple<KdTree<Key4D, int>, List<KeyInt>>(tree, expectedInTree);
    }

    public static void RunSimTests(TextBlock block)
    {
        var tuple = Create4DTree();
        KdTree<Key4D, int> key4DTree = tuple.Item1;
        List<KeyInt> expectedInKey4DTree = tuple.Item2;

        bool tryCath = true;
        int startSeed = Config.Instance.Seed;
        int seedCount = Config.Instance.SeedCount;
        int count = Config.Instance.OperationCount;

        for (int seed = startSeed; seed <= startSeed + seedCount; seed++)
        {
            if (tryCath)
            {
                try
                {
                    RunOneSimTest(key4DTree, expectedInKey4DTree, seed, count, block);

                    if (seedCount < 100 || seed % (seedCount / 100) == 0)
                    {
                        block.Text += $"Seed: {seed} / {startSeed + seedCount} OK\n";
                        Console.WriteLine($"Seed: {seed} / {startSeed + seedCount} OK");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"PADLO TO PRI SEEDE: {seed} {e.Message}");
                    return;
                }
            }
            else
            {
                RunOneSimTest(key4DTree, expectedInKey4DTree, seed, count, block);
            }
        }
    }

    public static void RunOneSimTest(
        KdTree<Key4D, int> tree,
        List<KeyInt> expectedInTree,
        int pSeed = 1,
        int pCount = 100, TextBlock? block = null)
    {
        if (block == null) return;
        int seed = pSeed;
        int count = pCount;

        OperationGenerator opGen = new(
            probAdd: Config.Instance.ProbOfAdd,
            probFind: Config.Instance.ProbOfFind,
            probUpdate: Config.Instance.ProbOfUpdate,
            probRemove: Config.Instance.ProbOfRemove
        );

        Random gen = new(seed); // for generating random numbers

        for (int i = 0; i < count; i++)
        {
            Operation op = opGen.GetRandomOperation(gen);
            switch (op)
            {
                case Operation.Add:
                    DoOperationAdd(tree, expectedInTree, gen, block, i);
                    break;
                case Operation.Remove:
                    DoOperationRemove(tree, expectedInTree, gen, block);
                    break;
                case Operation.Find:
                    DoOperationFind(tree, expectedInTree, gen, block);
                    break;
                case Operation.Update:
                    DoOperationUpdate(tree, expectedInTree, gen, block);
                    break;
            }

            if (i % Config.Instance.CheckAfterOperationCount == 0)
            {
                CheckCorrectness(expectedInTree, tree);
                if (Config.Instance.ShoudPrint)
                    Console.WriteLine($"{pSeed}: {i + Config.Instance.CheckAfterOperationCount} / {count}");
            }

            //if (_shouldPrint) tree.Print();
        }

        CheckCorrectness(expectedInTree, tree);
        if (Config.Instance.ShoudPrint) Console.WriteLine(tree.Size);
    }

    private static void DoOperationAdd(
        KdTree<Key4D, int> tree,
        List<KeyInt> expectedInTree,
        Random gen,
        TextBlock block,
        int i
    )
    {
        Key4D randomKey;
        while (true)
        {
            if (gen.NextDouble() < Config.Instance.ProbOfDuplicate && expectedInTree.Count > 0)
            {
                int randomIndex = gen.Next(expectedInTree.Count);
                randomKey = (Key4D)expectedInTree[randomIndex].Key;
            }
            else
            {
                randomKey = new Key4D(
                    gen); // rand generated new Key4D (could be existing although the probability is low)
            }

            if (!_notExistingKey.Equals(randomKey))
            {
                break; // the probability is low but not zero => now is zero
            }
        }

        int randomData = gen.Next(-100000, 100000);
        tree.Add(randomKey, randomData);
        expectedInTree.Add(new KeyInt(randomKey, randomData));

        if (Config.Instance.ShoudPrint)
        {
            Console.WriteLine($"Adding element {randomKey} {i}");
            block.Text += $"Adding element {randomKey} {i}\n";
        }
    }

    private static void DoOperationFind(
        KdTree<Key4D, int> tree,
        List<KeyInt> expectedInTree,
        Random gen,
        TextBlock block
    )
    {
        if (expectedInTree.Count <= 0) return;

        int indexOfElementToUpdate = gen.Next(0, expectedInTree.Count);
        KeyInt randomExistingElement = expectedInTree[indexOfElementToUpdate];

        List<int> found = tree.Find((Key4D)randomExistingElement.Key);

        if (!found.Contains(randomExistingElement.Data))
        {
            throw new Exception("The node was not in expected list");
        }
    }

    private static void DoOperationRemove(
        KdTree<Key4D, int> tree,
        List<KeyInt> expectedInTree,
        Random gen,
        TextBlock block
    )
    {
        if (expectedInTree.Count > 0 && gen.NextDouble() < Config.Instance.ProbOfDuplicate)
        {
            int indexOfElementToRemove = gen.Next(0, expectedInTree.Count);
            KeyInt randomExistingElement = expectedInTree[indexOfElementToRemove];

            List<int> itemsWithMatchingKey = tree.Find((Key4D)randomExistingElement.Key);

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

            tree.Remove((Key4D)randomExistingElement.Key, itemsWithMatchingKey[j].Uid); // removing item from the tree
            expectedInTree.RemoveAt(indexOfElementToRemove);
            if (Config.Instance.ShoudPrint)
            {
                Console.WriteLine(
                    $"Removing element {(Key4D)randomExistingElement.Key} {randomExistingElement.Data} at index {indexOfElementToRemove}");
                block.Text += $"Removing element {randomExistingElement.Key} {randomExistingElement.Data}\n";
            }
        }
        else
        {
            // removing not existing element
            tree.Remove(_notExistingKey, 0); // removing node which doesn't exist (shouldn't change anything)
            if (Config.Instance.ShoudPrint)
            {
                Console.WriteLine("Removing a not existing element");
                block.Text += "Removing a not existing element\n";
            }
        }
    }

    private static void DoOperationUpdate(
        KdTree<Key4D, int> tree,
        List<KeyInt> expectedInTree,
        Random gen,
        TextBlock block
    )
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

            int newData = gen.Next();
            tree.Update((Key4D)randomExistingElement.Key, itemsWithMatchingKey[j].Uid,
                newData); // updating item from the tree
            expectedInTree[indexOfElementToUpdate].Data = newData;
            if (Config.Instance.ShoudPrint)
            {
                Console.WriteLine(
                    $"Updating element {(Key4D)randomExistingElement.Key} {randomExistingElement.Data} at index {indexOfElementToUpdate}");
                block.Text +=
                    $"Updating element {(Key4D)randomExistingElement.Key} {randomExistingElement.Data}\n";
            }
        }
    }
}*/