using My.DataStructures;

namespace My.Tests;

public enum Operation
{
    Add, // 0
    Find, // 1
    //Remove // 2
}

public class OperationGenerator
{
    private Random _randomInstance = new Random();

    public Operation GetRandomOperation(Random? pRandom = null)
    {
        Operation[] ops = (Enum.GetValues(typeof(Operation)) as Operation[])!;
        int randomNumber = Utils.GetRandomIntInRange(0, ops.Length, pRandom ?? _randomInstance); // will generate <0; 2>
        return ops[randomNumber];
    }

    public void SetSeed(int seed)
    {
        _randomInstance = new Random(seed);
    }
}