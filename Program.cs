using My.DataStructures.KdTree;
using My.Tests;

namespace My;

class Program
{
    public static void Main(string[] args)
    {
        SimulationTester simulationTester = new SimulationTester(
            pProbAdd: 0.2, pProbFind: 0, pProbRemove: 0.8, pProbUpdate: 0, pProbOfRemovingExistingElement: 1,
            pCheckAfterOperationCount: 1000, pShouldPrint: false
        );


        bool tryCath = true;
        int count = 1000;
        int seedCount = 100000;


        for (int seed = 1; seed <= seedCount; seed++)
        {
            if (tryCath)
            {
                try
                {
                    simulationTester.RunCordInt(seed, count);
                    if (seed % (seedCount / 100) == 0)
                    {
                        Console.WriteLine($"Seed: {seed} / {seedCount} OK");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"PADLO TO PRI SEEDE: {seed}");
                }
            }
            else
            {
                simulationTester.RunCordInt(seed, count);
            }
        }
    }


}