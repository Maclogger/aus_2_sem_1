using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.Core;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests;
using Avalonia.Controls;

namespace AUS_Semestralna_Praca_1;

public class MainApplication
{
    // SINGLETON
    private static readonly MainApplication _instance = new();
    public static MainApplication Instance => _instance;

    private ApplicationCore _core;
    private ConsoleGui _consoleGui;

    private MainApplication()
    {
        _core = new ApplicationCore(this);
        _consoleGui = new ConsoleGui(this);

        Position pos1 = new Position(0, 'N', 0.0, 'E');
        Position pos2 = new Position(10, 'N', 15.0, 'W');
        _core.AddRealestate(pos1, pos2, new Realestate(1, "Popis prvej nehnuteľnosti", pos1, pos2));

        pos1 = new Position(1.0, 'N', 0.0, 'W');
        pos2 = new Position(8.0, 'N', 19.0, 'W');
        _core.AddRealestate(pos1, pos2, new Realestate(2, "SDfjsdlfkj", pos1, pos2));

        pos1 = new Position(1.0, 'N', 10.0, 'W');
        pos2 = new Position(8.0, 'N', 19.0, 'W');
        _core.AddRealestate(pos1, pos2, new Realestate(13, "TOTO TU BYT NEMA!!!", pos1, pos2));

        pos1 = new Position(0.0, 'N', 0.0, 'W');
        pos2 = new Position(12.0, 'N', 16.0, 'W');
        _core.AddRealestate(pos1, pos2, new Realestate(3, "fsdlkfjľ", pos1, pos2));

        pos1 = new Position(1.0, 'N', 2.0, 'W');
        pos2 = new Position(0.0, 'N', 0.0, 'W');
        _core.AddRealestate(pos1, pos2, new Realestate(16, "SLDF", pos1, pos2));

        pos1 = new Position(0.0, 'N', 0.0, 'W');
        pos2 = new Position(11.0, 'N', 19.0, 'W');
        _core.AddRealestate(pos1, pos2, new Realestate(4, "Popis ntej", pos1, pos2));

        pos1 = new Position(0.0, 'N', 0.0, 'W');
        pos2 = new Position(5.0, 'N', 47.0, 'W');
        _core.AddRealestate(pos1, pos2, new Realestate(24, "AHOJ", pos1, pos2));
    }

    public int ParcelCount
    {
        get => _core.GetParcelCount();
    }

    public int RealEstateCount
    {
        get => _core.GetRealEstateCount();
    }

    public int AssetCount
    {
        get => _core.GetAssetCount();
    }

    // GUI => Core
    public Answer AddParcel(string pos1Attr, string pos2Attr, string parcelAttr)
    {
        int? parcelNum = ClientSys.GetIntFromAttr(parcelAttr, "PARCEL_NUM");
        string description = ClientSys.GetStringFromAttr(parcelAttr, "DESCRIPTION")!;

        Position? pos1;
        Position? pos2;
        try
        {
            pos1 = new(pos1Attr); // it can throw inside
            pos2 = new(pos2Attr); // it can throw inside
            int dummy = (int)parcelNum!; // if parcelNum is not a number => crash
        }
        catch
        {
            return new Answer("Some of the attributes are missing or invalid.", AnswerState.Error);
        }

        Parcel parcel = new((int)parcelNum, description, pos1, pos2);

        return _core.AddParcel(pos1, pos2, parcel);
    }

    public Answer RemoveParcel(string attr)
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        _consoleGui.Run();
    }

    public int GetUidFromUserByChoosingFromList<T>(List<DataPart<T>> list)
    {
        List<string?> optionsForUser = new();
        foreach (DataPart<T> dataPart in list)
        {
            string option = dataPart.Value?.ToString() ?? "NULL";
            optionsForUser.Add(option);
        }

        int index = _consoleGui.ChooseFromList(optionsForUser); // TODO implement GUI

        return list[index].Uid;
    }

    public void PrintParcelTree()
    {
        _core.PrintParcelTree();
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////// REALESTATE //////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////
    public Answer AddRealestate(string pos1Attr, string pos2Attr, string realestateAttr)
    {
        int? realestateNum = ClientSys.GetIntFromAttr(realestateAttr, "REALESTATE_NUM");
        string description = ClientSys.GetStringFromAttr(realestateAttr, "DESCRIPTION")!;

        Position? pos1;
        Position? pos2;
        try
        {
            pos1 = new(pos1Attr); // it can throw inside
            pos2 = new(pos2Attr); // it can throw inside
            int dummy = (int)realestateNum!; // if parcelNum is not a number => crash
        }
        catch
        {
            return new Answer("Some of the attributes are missing or invalid.", AnswerState.Error);
        }

        Realestate realestate = new((int)realestateNum, description, pos1, pos2);

        return _core.AddRealestate(pos1, pos2, realestate);
    }

    public Tuple<Answer, List<string>> FindRealestates(string posAttr)
    {
        Position pos;
        try
        {
            pos = new(posAttr);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<string>>(
                new Answer("Some of the attributes are missing or invalid.", AnswerState.Error), new List<string>());
        }

        Tuple<Answer, List<DataPart<Realestate>>> tuple = _core.FindRealestates(pos);

        if (tuple.Item1.State is AnswerState.Error or AnswerState.Info)
        {
            return new Tuple<Answer, List<string>>(tuple.Item1, new List<string>());
        }

        // Order has to be correct
        List<string> solList = new(tuple.Item2.Count);
        foreach (DataPart<Realestate> realDp in tuple.Item2)
        {
            string sol = "";
            realDp.Value.ToAttr(ref sol);
            solList.Add(sol);
        }

        return new Tuple<Answer, List<string>>(tuple.Item1, solList);
    }

    public Tuple<Answer, List<string>> FindAssets(string posAttr1, string posAttr2)
    {
        Position pos1;
        Position pos2;
        try
        {
            pos1 = new(posAttr1);
            pos2 = new(posAttr2);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<string>>(
                new Answer("Some of the attributes are missing or invalid.", AnswerState.Error), new List<string>());
        }

        Tuple<Answer, List<DataPart<Asset>>> tuple = _core.FindAssets(pos1, pos2);

        if (tuple.Item1.State is AnswerState.Error or AnswerState.Info)
        {
            return new Tuple<Answer, List<string>>(tuple.Item1, new List<string>());
        }

        // Order has to be correct
        List<string> solList = new(tuple.Item2.Count);
        foreach (DataPart<Asset> realDp in tuple.Item2)
        {
            string sol = "";
            realDp.Value.ToAttr(ref sol);
            ClientSys.AddToAttr(ref sol, "TYPE", realDp.Value is Realestate ? "RS" : "PC");
            solList.Add(sol);
        }

        return new Tuple<Answer, List<string>>(tuple.Item1, solList);
    }


    public void RunTest(TextBlock block, bool shouldRun2DTest = true)
    {
        SimulationTester simulationTester = new SimulationTester(
            pProbAdd: Config.Instance.ProbOfAdd,
            pProbFind: Config.Instance.ProbOfFind, pProbRemove: Config.Instance.ProbOfRemove,
            pProbUpdate: Config.Instance.ProbOfUpdate,
            pProbOfRemovingExistingElement: Config.Instance.ProbOfAddingExistingElement,
            pCheckAfterOperationCount: 1000, pShouldPrint: Config.Instance.ShoudPrint
        );


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
                    if (shouldRun2DTest)
                    {
                        simulationTester.Run2DTest(seed, count, block);
                    }
                    else
                    {
                        simulationTester.Run4DTest(seed, count, block);
                    }

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
                if (shouldRun2DTest)
                {
                    simulationTester.Run2DTest(seed, count, block);
                }
                else
                {
                    simulationTester.Run4DTest(seed, count, block);
                }
            }
        }
    }
}