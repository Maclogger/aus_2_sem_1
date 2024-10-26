using System;

namespace AUS_Semestralna_Praca_1.BackEnd.Tests;

public enum Operation
{
    Add, // 0
    Find, // 1
    Remove, // 2
    Update // 3
}

public class OperationGenerator
{
    private readonly int? _seed;
    private readonly double _probAdd;
    private readonly double _probFind;
    private readonly double _probRemove;
    private readonly double _probUpdate;
    private Random _randomInstance;

    public OperationGenerator(int? seed = null, double probAdd = 0.5, double probFind = 0.5, double probRemove = 0,
        double probUpdate = 0)
    {
        _seed = seed;
        _probAdd = probAdd;
        _probFind = probFind;
        _probRemove = probRemove;
        _probUpdate = probUpdate;
        _randomInstance = seed == null ? new Random() : new Random((int)seed);
    }

    public Operation GetRandomOperation(Random? pRandom = null)
    {
        Random random = pRandom ?? _randomInstance;
        double randomNumber = random.NextDouble();

        if (randomNumber <= _probAdd)
        {
            return Operation.Add;
        }

        if (randomNumber <= _probAdd + _probFind)
        {
            return Operation.Find;
        }

        if (randomNumber <= _probAdd + _probFind + _probRemove)
        {
            return Operation.Remove;
        }

        return Operation.Update;
    }

    public void SetSeed(int pSeed)
    {
        _randomInstance = new Random(pSeed);
    }
}