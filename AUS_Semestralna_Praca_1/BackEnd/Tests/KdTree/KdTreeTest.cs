using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
using Avalonia.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree;

public static class SimulationTester
{
    private static void CheckCorrectness(List<TestKey> pExpectedInTree, KdTree<TestKey, Cord> pTree)
    {
        Assert.AreEqual(pExpectedInTree.Count, pTree.Size);

        List<Cord> actual = new(pExpectedInTree.Count);
        List<Cord> expected = new(pExpectedInTree.Count);

        foreach (Cord data in pTree)
        {
            actual.Add(data);
        }

        foreach (TestKey testKey in pExpectedInTree)
        {
            expected.Add(testKey.Cord);
        }

        actual.Sort();
        expected.Sort();

        CollectionAssert.AreEqual(actual, expected);
    }

    public static (KdTree<TestKey, Cord>, List<TestKey>) CreateTestTree()
    {
        int countBeforeTest = Config.Instance.ElementCountBeforeTest;

        Random gen = new();
        KdTree<TestKey, Cord> tree = new(2);
        List<TestKey> expectedInTree = new(countBeforeTest);

        for (int i = 0; i < countBeforeTest; i++)
        {
            TestKey randomKey = new TestKey(gen);

            tree.Add(randomKey, randomKey.Cord); // adding randomly generated Key4D into tree, data is just an i
            expectedInTree.Add(randomKey);
        }

        return (tree, expectedInTree);
    }

    public static void RunSimTests(KdTree<TestKey, Cord> tree, List<TestKey> expectedInTree, TextBlock block)
    {
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
                    RunOneSimTest(tree, expectedInTree, seed, count, block);

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
                RunOneSimTest(tree, expectedInTree, seed, count, block);
            }
        }
    }

    private static void RunOneSimTest(
        KdTree<TestKey, Cord> tree,
        List<TestKey> expectedInTree,
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
        KdTree<TestKey, Cord> tree,
        List<TestKey> expectedInTree,
        Random gen,
        TextBlock block,
        int i
    )
    {
        TestKey randomNewKey;
        if (gen.NextDouble() < Config.Instance.ProbOfDuplicate && expectedInTree.Count > 0)
        {
            int randomIndex = gen.Next(expectedInTree.Count);
            randomNewKey = new TestKey(expectedInTree[randomIndex]);
        }
        else
        {
            randomNewKey = new TestKey(gen); // rand generated new Key4D (could be existing although the probability is low)
        }

        tree.Add(randomNewKey, randomNewKey.Cord);
        expectedInTree.Add(randomNewKey);

        if (Config.Instance.ShoudPrint)
        {
            Console.WriteLine($"Adding element {randomNewKey} {i}");
            block.Text += $"Adding element {randomNewKey} {i}\n";
        }
    }

    private static void DoOperationFind(
        KdTree<TestKey, Cord> tree,
        List<TestKey> expectedInTree,
        Random gen,
        TextBlock block
    )
    {
        if (expectedInTree.Count <= 0) return;

        int indexOfElementToUpdate = gen.Next(0, expectedInTree.Count);
        TestKey randomExistingElement = expectedInTree[indexOfElementToUpdate];

        List<Cord> found = tree.Find(randomExistingElement);

        if (!found.Contains(randomExistingElement.Cord))
        {
            block.Text += $"The node {randomExistingElement.Cord} was not in expected list!!! TEST FAILED\n";
            throw new Exception($"The node {randomExistingElement.Cord} was not in expected list!!! TEST FAILED");
        }
    }

    private static void DoOperationRemove(
        KdTree<TestKey, Cord> tree,
        List<TestKey> expectedInTree,
        Random gen,
        TextBlock block
    )
    {
        if (expectedInTree.Count > 0 && gen.NextDouble() < Config.Instance.ProbOfDuplicate)
        {
            int indexOfElementToRemove = gen.Next(0, expectedInTree.Count);
            TestKey randomExistingElement = expectedInTree[indexOfElementToRemove];

            tree.Remove(randomExistingElement);
            expectedInTree.RemoveAt(indexOfElementToRemove);
            if (Config.Instance.ShoudPrint)
            {
                Console.WriteLine(
                    $"Removing element {randomExistingElement} {randomExistingElement} at index {indexOfElementToRemove}");
                block.Text += $"Removing element {randomExistingElement} {randomExistingElement} at index {indexOfElementToRemove}\n";
            }
        }
        else
        {
            // removing new random element (probably not existing one)
            tree.Remove(new TestKey(gen));
            if (Config.Instance.ShoudPrint)
            {
                Console.WriteLine("Removing a random existing element");
                block.Text += "Removing a random existing element\n";
            }
        }
    }

    private static void DoOperationUpdate(
        KdTree<TestKey, Cord> tree,
        List<TestKey> expectedInTree,
        Random gen,
        TextBlock block
    )
    {
        if (expectedInTree.Count > 0)
        {
            int indexOfElementToUpdate = gen.Next(0, expectedInTree.Count);
            TestKey randomExistingElement = expectedInTree[indexOfElementToUpdate];

            List<Cord> itemsWithMatchingKey = tree.Find(randomExistingElement);

            if (itemsWithMatchingKey == null || itemsWithMatchingKey.Count <= 0)
            {
                block.Text +=
                    $"The node {itemsWithMatchingKey} was in expected list, but was not found by the KdTree. :( TEST FAILED! \n";
                throw new KeyNotFoundException(
                    $"The node {itemsWithMatchingKey} was in expected list, but was not found by the KdTree. :( TEST FAILED! ");
            }

            TestKey newKeyAndData = new TestKey(gen);
            tree.Update(randomExistingElement, newKeyAndData, newKeyAndData.Cord); // updating item from the tree
            expectedInTree[indexOfElementToUpdate] = newKeyAndData;

            if (Config.Instance.ShoudPrint)
            {
                Console.WriteLine(
                    $"Updating element {randomExistingElement} at index {indexOfElementToUpdate}");
                block.Text +=
                    $"Updating element {randomExistingElement} at index {indexOfElementToUpdate}\n";
            }
        }
    }
}