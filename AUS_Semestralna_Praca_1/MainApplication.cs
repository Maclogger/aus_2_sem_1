using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using AUS_Semestralna_Praca_1.BackEnd.Core;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Files;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
using AUS_Semestralna_Praca_1.FrontEnd.Assets;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace AUS_Semestralna_Praca_1;

public class MainApplication
{
    // SINGLETON
    private static readonly MainApplication _instance = new();
    public static MainApplication Instance => _instance;

    public ApplicationCore Core { get; set; }


    public KdTree<TestKey, Cord>? TestTree { get; set; } = new(2);
    public List<TestKey>? ExpectedInTree { get; set; }


    private MainApplication()
    {
        Core = new ApplicationCore(this);

        /*
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
    */
    }


    public int RealestateCount => Core.RealestatesCount;
    public int ParcelCount => Core.ParcelsCount;
    public int AssetsCount => Core.AssetsCount;

    private List<Asset> _overlayingAssets = new();

    public Answer
        AddAsset(string pos1Attr, string pos2Attr, string assetAttr, char sign) // sign: 'R': realestate, 'P': parcel
    {
        int? assetNum = ClientSys.GetIntFromAttr(assetAttr, "NUM");
        string description = ClientSys.GetStringFromAttr(assetAttr, "DESCRIPTION")!;

        Position? pos1;
        Position? pos2;
        try
        {
            pos1 = new(pos1Attr); // it can throw inside
            pos2 = new(pos2Attr); // it can throw inside
            int dummy = (int)assetNum!; // if parcelNum/realestateNum is not a number => crash
        }
        catch
        {
            return new Answer("Some of the attributes are missing or invalid.", AnswerState.Error);
        }

        Asset asset;
        if (sign == 'R')
        {
            asset = new Realestate((int)assetNum, description, pos1, pos2);
        }
        else
        {
            asset = new Parcel((int)assetNum, description, pos1, pos2);
        }

        return Core.AddAsset(pos1, pos2, asset);
    }

    public (Answer, List<string>) FindAssets(string posAttr, char sign) // 'R': realestate, 'P': parcel, 'A': asset
    {
        Position pos;
        try
        {
            pos = new(posAttr);
            pos.SetUidNull(); // easier to test, uid is null
        }
        catch (Exception e)
        {
            return (new Answer($"Some of the attributes are missing or invalid. {e.Message}", AnswerState.Error),
                new List<string>());
        }

        List<string> sol = new();
        Answer answer;

        switch (sign)
        {
            case 'R':
            {
                (answer, List<Realestate> realestates) = Core.FindRealestates(pos);
                foreach (Realestate realestate in realestates)
                {
                    string realestateStr = "";
                    realestate.ToAttr(ref realestateStr);
                    sol.Add(realestateStr);
                }

                break;
            }
            case 'P':
            {
                (answer, List<Parcel> parcels) = Core.FindParcels(pos);
                foreach (Parcel parcel in parcels)
                {
                    string parcelStr = "";
                    parcel.ToAttr(ref parcelStr);
                    sol.Add(parcelStr);
                }

                break;
            }
            default:
            {
                (answer, List<Asset> assets) = Core.FindAssets(pos);
                foreach (Asset asset in assets)
                {
                    string assetStr = "";
                    asset.ToAttr(ref assetStr);
                    sol.Add(assetStr);
                }

                break;
            }
        }

        if (answer.State is AnswerState.Error or AnswerState.Info)
        {
            return (answer, new List<string>());
        }

        return (answer, sol);
    }

    public (Answer answer, List<string> assets) FindAssets(string posAttr1, string posAttr2)
    {
        (Answer answer, List<string> assets) = FindAssets(posAttr1, 'A');
        (Answer answer2, List<string> assetsOther) = FindAssets(posAttr2, 'A');

        if (answer.State is AnswerState.Error)
        {
            return (answer, new List<string>());
        }

        if (answer2.State is AnswerState.Error)
        {
            return (answer2, new List<string>());
        }


        foreach (string assetStr in assetsOther)
        {
            if (!assets.Contains(assetStr))
            {
                assets.Add(assetStr);
            }
        }

        Answer final = answer.State is AnswerState.Ok ? answer : answer2;
        return (final, assets);
    }

    public (Answer answer, List<string> assets) FindAllAssets(char sign)
    {
        Answer answer;
        List<string> sol = new();
        switch (sign)
        {
            case 'R':
            {
                (answer, List<Realestate> realestates) = Core.FindAllRealestates();
                foreach (Realestate realestate in realestates)
                {
                    string realestateStr = "";
                    realestate.ToAttr(ref realestateStr);
                    sol.Add(realestateStr);
                }

                break;
            }
            case 'P':
            {
                (answer, List<Parcel> parcels) = Core.FindAllParcels();
                foreach (Parcel parcel in parcels)
                {
                    string parcelStr = "";
                    parcel.ToAttr(ref parcelStr);
                    sol.Add(parcelStr);
                }

                break;
            }
            default:
            {
                (answer, List<Asset> assets) = Core.FindAllAssets();
                foreach (Asset asset in assets)
                {
                    string assetStr = "";
                    asset.ToAttr(ref assetStr);
                    sol.Add(assetStr);
                }

                break;
            }
        }

        if (answer.State is AnswerState.Error or AnswerState.Info)
        {
            return (answer, new List<string>());
        }

        return (answer, sol);
    }

    public void RemoveAsset(AssetData asset)
    {
        Position pos1 = new(asset.Pos1Data.Latitude, asset.Pos1Data.LatitudeSign[0], asset.Pos1Data.Longitude,
            asset.Pos1Data.LongitudeSign[0]);
        pos1.Uid = asset.Pos1Data.Uid;
        Position pos2 = new(asset.Pos2Data.Latitude, asset.Pos2Data.LatitudeSign[0], asset.Pos2Data.Longitude,
            asset.Pos2Data.LongitudeSign[0]);
        pos2.Uid = asset.Pos2Data.Uid;

        Core.RemoveAsset(pos1, pos2, asset.Type[0]);
    }


    private (Position, bool, Position, bool) GetRandomPositionsWithOverlay(
        List<Position> positions,
        Random random,
        double probabilityOfOverlay
    )
    {
        Position pos1;
        Position pos2;
        bool wasWithOverlay1 = false;
        bool wasWithOverlay2 = false;
        if (random.NextDouble() < probabilityOfOverlay && positions.Count >= 2)
        {
            pos1 = new(positions[random.Next(0, positions.Count)]);
            wasWithOverlay1 = true;
        }
        else
        {
            pos1 = new(random);
        }

        if (random.NextDouble() < probabilityOfOverlay && positions.Count >= 2)
        {
            pos2 = new(positions[random.Next(0, positions.Count)]);
            wasWithOverlay2 = true;
        }
        else
        {
            pos2 = new(random);
        }

        if (pos1.Equals(pos2))
        {
            pos2 = new(random);
        }

        return (pos1, wasWithOverlay1, pos2, wasWithOverlay2);
    }


    public Answer FillUpSystem(double probabilityOfOverlay, int elementCount, double realestateParcelRatio)
    {
        Random random = new(1);
        int realestatePositionsCountApprox = (int)(realestateParcelRatio * elementCount * 2);
        int parcelPositionsCountApprox = 2 * elementCount - realestatePositionsCountApprox;

        List<Position> positionsOfRealestate = new(realestatePositionsCountApprox);
        List<Position> positionsOfParcels = new(parcelPositionsCountApprox);

        for (int i = 0; i < elementCount; i++)
        {
            if (!Config.Instance.ShoudPrint) Console.WriteLine($"{i + 1} / {elementCount}");
            if (random.NextDouble() < realestateParcelRatio)
            {
                // generating new realestate
                (Position pos1, bool isWithOverlay1, Position pos2, bool isWithOverlay2) =
                    GetRandomPositionsWithOverlay(positionsOfParcels, random, probabilityOfOverlay);
                Realestate realestate = new(random, pos1, pos2);
                positionsOfRealestate.Add(pos1);
                positionsOfRealestate.Add(pos2);
                if (isWithOverlay1 || isWithOverlay2)
                {
                    _overlayingAssets.Add(realestate);
                }

                Core.AddAsset(pos1, pos2, realestate);
            }
            else
            {
                // generating new parcel
                (Position pos1, bool isWithOverlay1, Position pos2, bool isWithOverlay2) =
                    GetRandomPositionsWithOverlay(positionsOfRealestate, random, probabilityOfOverlay);
                Parcel parcel = new(random, pos1, pos2);
                positionsOfParcels.Add(pos1);
                positionsOfParcels.Add(pos2);
                if (isWithOverlay1 || isWithOverlay2)
                {
                    _overlayingAssets.Add(parcel);
                }

                Core.AddAsset(pos1, pos2, parcel);
            }
        }

        return new Answer("OK", AnswerState.Ok);
    }

    public Answer UpdateAsset(AssetData oldAssetData, AssetData newAssetData)
    {
        Position oldPos1 = new Position(oldAssetData.Pos1Data.Latitude, oldAssetData.Pos1Data.LatitudeSign[0],
            oldAssetData.Pos1Data.Longitude, oldAssetData.Pos1Data.LongitudeSign[0]);
        oldPos1.Uid = oldAssetData.Pos1Data.Uid;
        Position oldPos2 = new Position(oldAssetData.Pos2Data.Latitude, oldAssetData.Pos2Data.LatitudeSign[0],
            oldAssetData.Pos2Data.Longitude, oldAssetData.Pos2Data.LongitudeSign[0]);
        oldPos2.Uid = oldAssetData.Pos2Data.Uid;

        Position newPos1 = new Position(newAssetData.Pos1Data.Latitude, newAssetData.Pos1Data.LatitudeSign[0],
            newAssetData.Pos1Data.Longitude, newAssetData.Pos1Data.LongitudeSign[0]);
        oldPos1.Uid = newAssetData.Pos1Data.Uid;
        Position newPos2 = new Position(newAssetData.Pos2Data.Latitude, newAssetData.Pos2Data.LatitudeSign[0],
            newAssetData.Pos2Data.Longitude, newAssetData.Pos2Data.LongitudeSign[0]);
        oldPos2.Uid = newAssetData.Pos2Data.Uid;

        if (newAssetData.Type == "R")
        {
            Realestate realestate = new Realestate(newAssetData.Num, newAssetData.Description, newPos1, newPos2);
            return Core.UpdateAsset(oldPos1, oldPos2, newPos1, newPos2, realestate);
        }
        else
        {
            Parcel parcel = new Parcel(newAssetData.Num, newAssetData.Description, newPos1, newPos2);
            return Core.UpdateAsset(oldPos1, oldPos2, newPos1, newPos2, parcel);
        }
    }

    public void SaveSystem(CsvWriter writer)
    {
        writer.Write("version", Config.Instance.Version);
        Core.SaveSystem(writer);
    }

    public void LoadSystem(CsvReader reader)
    {
        string version = reader.ReadString();
        if (version != Config.Instance.Version)
        {
            throw new VersionNotFoundException("Neplatná verzia súboru!");
        }

        Core.LoadSystem(reader);
    }

    public void RunSimTest(TextBlock block)
    {
        var sw = Stopwatch.StartNew();
        SimulationTester.RunSimTests(TestTree!, ExpectedInTree!, block);
        sw.Stop();
        Console.WriteLine("Simulation time: " + sw.Elapsed);
    }

    public void PrepareTestTree()
    {
        (TestTree, ExpectedInTree) = SimulationTester.CreateTestTree();
    }

    public void PrintTestTree(TextBlock myTextBlock)
    {
        myTextBlock.Text = "";
        foreach ((TestKey testKey, Cord cord) in TestTree!.LevelOrderEntries())
        {
            myTextBlock.Text += $"{testKey}: {cord}\n";
        }
    }

    public (Answer answer, List<string> assets) GetOverlayingAssets(AssetData? ofAssetData = null)
    {
        List<string> sol = [];

        List<Asset> assets = [];
        if (ofAssetData == null)
        {
            if (Config.Instance.ShouldUpdateOverlays)
            {
                _overlayingAssets = Core.FindAllOverlayingAssets();
            }
            assets = _overlayingAssets;
        }
        else
        {
            var (realestates, parcels) = Core.GetOverlayingAssets(ofAssetData);

            foreach (Realestate realestate in realestates)
            {
                assets.Add(realestate);
            }

            foreach (Parcel parcel in parcels)
            {
                assets.Add(parcel);
            }
        }

        if (assets.Count <= 0)
            return (new Answer("Neexistuje žiadna parcela ani nehnuteľnosť, ktorá sa prekrýva.", AnswerState.Info), sol);
        
        foreach (Asset asset in assets)
        {
            string realestateStr = "";
            asset.ToAttr(ref realestateStr);
            sol.Add(realestateStr);
        }

        return (new Answer("OK", AnswerState.Ok), sol);
    }
}