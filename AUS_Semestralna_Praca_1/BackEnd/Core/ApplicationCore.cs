using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
using Avalonia.Controls;
using Avalonia.Remote.Protocol.Input;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class ApplicationCore
{
    private readonly MainApplication _mainApplication;
    private KdTree<Position, Realestate> _realestatesTree = new(2);
    private KdTree<Position, Parcel> _parcelasTree = new(2);
    private KdTree<Position, Asset> _assetsTree = new(2);


    private KdTree<Cord, int> _cordTree;
    private KdTree<Key4D, int> _key4DTree;
    private List<KeyInt> _expectedInCordTree;
    private List<KeyInt> _expectedInKey4DTree;

    private int _parcelsCount = 0;
    private int _realestatesCount = 0;
    private int _assetsCount = 0;


    public ApplicationCore(MainApplication pMainApplication)
    {
        _mainApplication = pMainApplication;
        Tuple<KdTree<Cord, int>, List<KeyInt>> tuple = Create2DTree();
        CordTree = tuple.Item1;
        ExpectedInCordTree = tuple.Item2;
        var tuple2 = Create4DTree();
        Key4DTree = tuple2.Item1;
        ExpectedInKey4DTree = tuple2.Item2;
    }

    public KdTree<Cord, int> CordTree
    {
        get => _cordTree;
        set => _cordTree = value;
    }

    public KdTree<Key4D, int> Key4DTree
    {
        get => _key4DTree;
        set => _key4DTree = value;
    }

    public List<KeyInt> ExpectedInCordTree
    {
        get => _expectedInCordTree;
        set => _expectedInCordTree = value;
    }

    public List<KeyInt> ExpectedInKey4DTree
    {
        get => _expectedInKey4DTree;
        set => _expectedInKey4DTree = value;
    }

    public Answer AddParcel(Position pos1, Position pos2, Parcel parcel)
    {
        try
        {
            // adding new parcel to all realestates at pos1 and pos2
            foreach (Realestate realestate in _realestatesTree.Find(pos1))
            {
                realestate.AddParcel(parcel);
                parcel.AddRealestate(realestate);
            }

            foreach (Realestate realestate in _realestatesTree.Find(pos2))
            {
                realestate.AddParcel(parcel);
                parcel.AddRealestate(realestate);
            }

            int uid1 = _parcelasTree.Add(pos1, parcel);
            int uid2 = _parcelasTree.Add(pos2, parcel);

            _assetsTree.Add(pos1, parcel);
            _assetsTree.Add(pos2, parcel);

            parcel.Uid1 = uid1;
            parcel.Uid2 = uid2;
        }
        catch (Exception e)
        {
            return new Answer($"Pridanie prvku ({pos1}-{pos2})-{parcel} sa nepodarilo. {e.Message}", AnswerState.Error);
        }

        _parcelsCount++;
        _assetsCount++;
        return new Answer($"Pridanie parcely ({pos1}-{pos2})-{parcel} bolo úspešné.", AnswerState.Ok);
    }

    public Answer RemoveParcel(Position pos1, int pUid1, Position pos2)
    {
        /*try
        {
            List<DataPart<Parcel>> dupParcelas1 = _parcelasTree.FindDataParts(pos1);
            List<DataPart<Parcel>> dupParcelas2 = _parcelasTree.FindDataParts(pos2);

            if (dupParcelas1.Count == 0 || dupParcelas2.Count == 0)
            {
                return new Answer($"!!! ERROR: Parcela na súradniciach ({pos1}-{pos2}) sa už nenachádzala v systéme!!! ERROR", AnswerState.Error);
            }

            Parcel parcelToDelete = DataPart<Parcel>.GetValue(dupParcelas1, pUid1)!;

            List<Realestate> realestatesAtPos1 = _realestatesTree.Find(pos1);
            List<Realestate> realestatesAtPos2 = _realestatesTree.Find(pos2);
            foreach (Realestate realestate in realestatesAtPos1)
            {
                realestate.RemoveParcel(parcelToDelete);
            }
            foreach (Realestate realestate in realestatesAtPos2)
            {
                realestate.RemoveParcel(parcelToDelete);
            }

            int uid2 = (int)DataPart<Parcel>.GetUid(dupParcelas2, parcelToDelete)!;
            _parcelasTree.Remove(pos1, pUid1);
            _parcelasTree.Remove(pos1, uid2);

            return new Answer($"Parcela na súradniciach ({pos1}-{pos2}) bola úspešne odstránená.", AnswerState.Ok);
        }
        catch (Exception e)
        {
            return new Answer($"Odstránenie parcely na súradniciach ({pos1}-{pos2}) sa nepodarilo. {e.Message}", AnswerState.Error);
        }*/
        throw new NotImplementedException();
    }

    public Answer UpdateParcel(Position oldPos1, Position oldPos2, Position newPos1, Position newPos2, Parcel newParcel,
        int pUid)
    {
        /*try
        {
            List<DataPart<Parcel>> foundDataParts = _parcelasTree.FindDataParts(oldPos1);
            Parcel? parcel = DataPart<Parcel>.GetValue(foundDataParts, pUid);

            if (parcel == null)
            {
                return new Answer($"Parcela na súradniciach {oldPos1}-{oldPos2} sa nenašla aj keď sa mala. CHYBA", AnswerState.Error);
            }

            if ((oldPos1.Equals(newPos1) && oldPos2.Equals(newPos2)) || (oldPos1.Equals(newPos2) && oldPos2.Equals(newPos1)))
            {
                // if only data is changed
                parcel.Description = newParcel.Description;
                parcel.ParcelNum = newParcel.ParcelNum;
                return new Answer($"Parcela na súradniciach {oldPos1}-{oldPos2} bola úspešne aktualizovaná.", AnswerState.Ok);
            }


            RemoveParcel();



            RemoveParcel(oldPosition, index);


            AddParcel();
        }
        catch (Exception e)
        {
            return new Answer($"Odstránenie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error);
        }



        if (parcels.Count <= 0)
        {
            return new Answer($"Neexistuje žiadna parcela na súradnici {oldPosition}", AnswerState.Info);
        }
        */


        throw new NotImplementedException();
    }

    public Tuple<Answer, List<DataPart<Parcel>>> FindParcelas(Position position)
    {
        try
        {
            List<DataPart<Parcel>> dpParcelas = _parcelasTree.FindDataParts(position);

            if (dpParcelas.Count <= 0)
            {
                return new Tuple<Answer, List<DataPart<Parcel>>>(
                    new Answer($"Neexistuje žiadna parcela na súradnici {position}", AnswerState.Info), dpParcelas);
            }

            return new Tuple<Answer, List<DataPart<Parcel>>>(
                new Answer($"Našlo sa {dpParcelas.Count} parciel.", AnswerState.Ok), dpParcelas);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<DataPart<Parcel>>>(
                new Answer($"Vyhľadanie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error),
                new List<DataPart<Parcel>>());
        }
    }

    public void PrintParcelTree()
    {
        _parcelasTree.Print();
    }

    public int GetParcelCount()
    {
        return _parcelsCount;
    }

    public int GetRealEstateCount()
    {
        return _realestatesCount;
    }

    public int GetAssetCount()
    {
        return _assetsCount;
    }

    public Answer AddRealestate(Position pos1, Position pos2, Realestate realestate)
    {
        try
        {
            // adding new parcel to all realestates at pos1 and pos2
            foreach (Parcel parcel in _parcelasTree.Find(pos1))
            {
                parcel.AddRealestate(realestate);
                realestate.AddParcel(parcel);
            }

            foreach (Parcel parcel in _parcelasTree.Find(pos2))
            {
                parcel.AddRealestate(realestate);
                realestate.AddParcel(parcel);
            }

            int uid1 = _realestatesTree.Add(pos1, realestate);
            int uid2 = _realestatesTree.Add(pos2, realestate);

            _assetsTree.Add(pos1, realestate);
            _assetsTree.Add(pos2, realestate);

            realestate.Uid1 = uid1;
            realestate.Uid2 = uid2;
        }
        catch (Exception e)
        {
            return new Answer($"Pridanie nehnuteľnosti ({pos1}-{pos2})-{realestate} sa nepodarilo. {e.Message}",
                AnswerState.Error);
        }

        _realestatesCount++;
        _assetsCount++;
        return new Answer($"Pridanie parcely ({pos1}-{pos2})-{realestate} bolo úspešné.", AnswerState.Ok);
    }

    public Tuple<Answer, List<DataPart<Realestate>>> FindRealestates(Position position)
    {
        try
        {
            List<DataPart<Realestate>> dpRealestates = _realestatesTree.FindDataParts(position);

            if (dpRealestates.Count <= 0)
            {
                return new Tuple<Answer, List<DataPart<Realestate>>>(
                    new Answer($"Neexistuje žiadna parcela na súradnici {position}", AnswerState.Info), dpRealestates);
            }


            return new Tuple<Answer, List<DataPart<Realestate>>>(
                new Answer($"Našlo sa {dpRealestates.Count} parciel.", AnswerState.Ok), dpRealestates);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<DataPart<Realestate>>>(
                new Answer($"Vyhľadanie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error),
                new List<DataPart<Realestate>>());
        }
    }

    public Tuple<Answer, List<DataPart<Asset>>> FindAssets(Position pos1, Position pos2)
    {
        try
        {
            List<DataPart<Asset>> dpAssets = _assetsTree.FindDataParts(pos1);
            List<DataPart<Asset>> tempDpAssets = _assetsTree.FindDataParts(pos2);

            foreach (DataPart<Asset> dpAsset in tempDpAssets)
            {
                if (!dpAssets.Contains(dpAsset))
                {
                    dpAssets.Add(dpAsset);
                }
            }

            if (dpAssets.Count <= 0)
            {
                return new Tuple<Answer, List<DataPart<Asset>>>(
                    new Answer($"Neexistuje žiadna parcela/nehnuteľnosť na súradniciach {pos1},{pos2}",
                        AnswerState.Info), dpAssets);
            }


            return new Tuple<Answer, List<DataPart<Asset>>>(
                new Answer($"Našlo sa {dpAssets.Count} parciel/nehnuteľností.", AnswerState.Ok), dpAssets);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<DataPart<Asset>>>(
                new Answer($"Vyhľadanie parcely na súradnici {pos1},{pos2} sa nepodarilo. {e.Message}",
                    AnswerState.Error),
                new List<DataPart<Asset>>());
        }
    }

    public Tuple<KdTree<Cord, int>, List<KeyInt>> Create2DTree()
    {
        int countBeforeTest = Config.Instance.ElementCountBeforeTest;

        Random gen = new();
        KdTree<Cord, int> tree = new(2);
        List<KeyInt> expectedInTree = new(countBeforeTest);

        for (int i = 0; i < countBeforeTest; i++)
        {
            Cord randomKey = new Cord(gen); // random generated new Key4D (could be existing although the probability is low)
            int randomValue = gen.Next(10, 10);

            tree.Add(randomKey, randomValue); // adding randomly generated Key4D into tree, data is just an i
            expectedInTree.Add(new KeyInt(randomKey, randomValue));
        }

        return new Tuple<KdTree<Cord, int>, List<KeyInt>>(tree, expectedInTree);
    }

    public Tuple<KdTree<Key4D, int>, List<KeyInt>> Create4DTree()
    {
        int countBeforeTest = Config.Instance.ElementCountBeforeTest;

        Random gen = new();
        KdTree<Key4D, int> tree = new(2);
        List<KeyInt> expectedInTree = new(countBeforeTest);

        for (int i = 0; i < countBeforeTest; i++)
        {
            Key4D randomKey = new Key4D(gen); // random generated new Key4D (could be existing although the probability is low)
            int randomValue = gen.Next(10, 10);

            tree.Add(randomKey, randomValue); // adding randomly generated Key4D into tree, data is just an i
            expectedInTree.Add(new KeyInt(randomKey, randomValue));
        }

        return new Tuple<KdTree<Key4D, int>, List<KeyInt>>(tree, expectedInTree);
    }

    public void InitializeTestTrees()
    {
        var tuple = Create2DTree();
        var tuple2 = Create4DTree();
        _cordTree = tuple.Item1;
        _expectedInCordTree = tuple.Item2;
        _key4DTree = tuple2.Item1;
        _expectedInKey4DTree = tuple2.Item2;
    }

    public void RunSimTest(TextBlock block)
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
                    SimulationTester.RunSimTest(Key4DTree, ExpectedInKey4DTree, seed, count, block);

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
                SimulationTester.RunSimTest(Key4DTree, ExpectedInKey4DTree, seed, count, block);
            }
        }
    }
}