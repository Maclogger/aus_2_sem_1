using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
using AUS_Semestralna_Praca_1.FrontEnd.Assets;
using Avalonia.Controls;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class ApplicationCore
{
    public MainApplication MainApplication { get; set; }
    public KdTree<Position, Realestate> RealestatesTree { get; set; } = new(2);

    public KdTree<Position, Parcel> ParcelsTree { get; set; } = new(2);
    public KdTree<Position, Asset> AssetsTree { get; set; } = new(2);


    public int ParcelsCount { get; set; } = 0;
    public int RealestatesCount { get; set; } = 0;
    public int AssetsCount { get; set; } = 0;


    public ApplicationCore(MainApplication pMainApplication)
    {
        MainApplication = pMainApplication;
    }

    public Answer AddAsset(Position pos1, Position pos2, Asset asset)
    {
        string message = "";
        if (asset is Realestate realestate)
        {
            try
            {
                // adding new parcel to all realestates at pos1 and pos2
                List<Parcel> parcels = ParcelsTree.Find(pos1);
                foreach (Parcel parcel in parcels)
                {
                    parcel.AddRealestate(realestate);
                    realestate.AddParcel(parcel);
                }

                parcels = ParcelsTree.Find(pos2);
                foreach (Parcel parcel in parcels)
                {
                    parcel.AddRealestate(realestate);
                    realestate.AddParcel(parcel);
                }

                RealestatesTree.Add(pos1, realestate);
                RealestatesTree.Add(pos2, realestate);
            }
            catch (Exception e)
            {
                return new Answer($"Pridanie nehnuteľnosti ({pos1}-{pos2})-{realestate} sa nepodarilo. {e.Message}",
                    AnswerState.Error);
            }

            RealestatesCount++;
            message = $"Pridanie nehnuteľnosti ({pos1}-{pos2})-{realestate} bolo úspešné.";
        }
        else if (asset is Parcel parcel)
        {
            try
            {
                // adding new parcel to all realestates at pos1 and pos2
                List<Realestate> realestates = RealestatesTree.Find(pos1);
                foreach (Realestate rls in realestates)
                {
                    rls.AddParcel(parcel);
                    parcel.AddRealestate(rls);
                }

                realestates = RealestatesTree.Find(pos2);
                foreach (Realestate rls in realestates)
                {
                    rls.AddParcel(parcel);
                    parcel.AddRealestate(rls);
                }

                ParcelsTree.Add(pos1, parcel);
                ParcelsTree.Add(pos2, parcel);
            }
            catch (Exception e)
            {
                return new Answer($"Pridanie parcely ({pos1}-{pos2})-{parcel} sa nepodarilo. {e.Message}",
                    AnswerState.Error);
            }

            ParcelsCount++;
            message = $"Pridanie parcely ({pos1}-{pos2})-{parcel} bolo úspešné.";
        }
        else
        {
            throw new ArgumentException($"Asset type {asset.GetType()} is not supported.");
        }

        AssetsTree.Add(pos1, asset);
        AssetsTree.Add(pos2, asset);

        AssetsCount++;
        return new Answer(message, AnswerState.Ok);
    }


    public void RunSimTest(TextBlock block)
    {
        // TODO SimulationTester.RunSimTests();
    }

    public Tuple<Answer, List<Asset>> FindAssets(Position pos, char sign)
    {
        throw new NotImplementedException();
    }

    public (Answer, List<Realestate>) FindRealestates(Position pos)
    {
        List<Realestate> realestates = RealestatesTree.Find(pos);

        if (realestates.Count == 0)
        {
            return new(new Answer($"Neboli nájdené žiadne nehnuteľnosti na pozícii: {pos}", AnswerState.Info),
                realestates);
        }

        return (new Answer($"OK", AnswerState.Ok), realestates);
    }

    public (Answer answer, List<Parcel> parcels) FindParcels(Position pos)
    {
        List<Parcel> parcels = ParcelsTree.Find(pos);

        if (parcels.Count == 0)
        {
            return new(new Answer($"Neboli nájdené žiadne nehnuteľnosti na pozícii: {pos}", AnswerState.Info), parcels);
        }

        return (new Answer($"OK", AnswerState.Ok), parcels);
    }

    public (Answer answer, List<Asset> parcels) FindAssets(Position pos)
    {
        List<Asset> assets = AssetsTree.Find(pos);

        if (assets.Count == 0)
        {
            return new(new Answer($"Neboli nájdené žiadne nehnuteľnosti na pozícii: {pos}", AnswerState.Info), assets);
        }

        return (new Answer($"OK", AnswerState.Ok), assets);
    }

    public (Answer answer, List<Realestate> realestates) FindAllRealestates()
    {
        List<Realestate> realestates = new();
        foreach (Realestate realestate in RealestatesTree.LevelOrder())
        {
            if (!realestates.Contains(realestate))
            {
                realestates.Add(realestate);
            }
        }

        if (realestates.Count > 0)
        {
            return (new Answer("OK", AnswerState.Ok), realestates);
        }

        return (new Answer("V systéme sa nenachádzajú žiadne nehnuteľnosti.", AnswerState.Info), realestates);
    }

    public (Answer answer, List<Parcel> realestates) FindAllParcels()
    {
        List<Parcel> parcels = new();
        foreach (Parcel parcel in ParcelsTree.LevelOrder())
        {
            if (!parcels.Contains(parcel))
            {
                parcels.Add(parcel);
            }
        }

        if (parcels.Count > 0)
        {
            return (new Answer("OK", AnswerState.Ok), parcels);
        }

        return (new Answer("V systéme sa nenachádzajú žiadne nehnuteľnosti.", AnswerState.Info), parcels);
    }

    public (Answer answer, List<Asset> realestates) FindAllAssets()
    {
        List<Asset> assets = new();
        foreach (Asset asset in AssetsTree.LevelOrder())
        {
            if (!assets.Contains(asset))
            {
                assets.Add(asset);
            }
        }

        if (assets.Count > 0)
        {
            return (new Answer("OK", AnswerState.Ok), assets);
        }

        return (new Answer("V systéme sa nenachádzajú žiadne nehnuteľnosti.", AnswerState.Info), assets);
    }

    public Answer RemoveAsset(Position pos1, Position pos2, char sign)
    {
        // pos has to contain UID also
        switch (sign)
        {
            case 'R':
            {
                Realestate? realestate = RealestatesTree.FindExact(pos1);
                if (realestate == null)
                {
                    return new Answer($"Nehnuteľnosť na pozícii {pos1}-{pos2} sa už v systéme nenašla.",
                        AnswerState.Error);
                }

                foreach (Parcel prc in realestate.Parcelas)
                {
                    prc.RemoveRealestate(realestate);
                }

                RealestatesTree.Remove(pos1);
                RealestatesTree.Remove(pos2);
                RealestatesCount--;
                break;
            }
            case 'P':
            {
                Parcel? parcel = ParcelsTree.FindExact(pos1);
                if (parcel == null)
                {
                    return new Answer($"Parcela na pozícii {pos1}-{pos2} sa už v systéme nenašla.",
                        AnswerState.Error);
                }

                foreach (Realestate rls in parcel.Realestates)
                {
                    rls.RemoveParcel(parcel);
                }

                ParcelsTree.Remove(pos1);
                ParcelsTree.Remove(pos2);
                ParcelsCount--;
                break;
            }
            default:
            {
                throw new UnreachableException("Unknown asset type!");
            }
        }

        AssetsTree.Remove(pos1);
        AssetsTree.Remove(pos2);
        AssetsCount--;
        return new Answer("OK", AnswerState.Ok);
    }

    public Answer UpdateAsset(
        Position oldPos1,
        Position oldPos2,
        Position newPos1,
        Position newPos2,
        Asset newAsset
    )
    {
        if (newAsset is Realestate rls)
        {
            if (oldPos1.Equals(newPos1) && oldPos2.Equals(newPos2))
            {
                Realestate? realestate = RealestatesTree.FindExact(oldPos1);
                if (realestate == null)
                {
                    return new Answer($"Nehnuteľnosť {rls} sa v systéme nenašla. CHYBA", AnswerState.Error);
                }

                realestate.Description = rls.Description;
                realestate.RealestateNum = rls.RealestateNum;
            }
            else
            {
                RemoveAsset(oldPos1, oldPos2, 'R');
                AddAsset(newPos1, newPos2, newAsset);
            }
        }
        else if (newAsset is Parcel prc)
        {
            if (oldPos1.Equals(newPos1) && oldPos2.Equals(newPos2))
            {
                Parcel? parcel = ParcelsTree.FindExact(oldPos1);
                if (parcel == null)
                {
                    return new Answer($"Parcela {prc} sa v systéme nenašla. CHYBA", AnswerState.Error);
                }

                parcel.Description = prc.Description;
                parcel.ParcelNum = prc.ParcelNum;
            }
            else
            {
                RemoveAsset(oldPos1, oldPos2, 'P');
                AddAsset(newPos1, newPos2, newAsset);
            }
        }

        return new Answer("OK", AnswerState.Ok);
    }

    public void SaveSystem(BinaryWriter binaryWriter)
    {
        // saving positions
        Position[] positions = new Position[AssetsCount * 2];

        binaryWriter.Write(AssetsCount);
        foreach ((Position position, Asset asset) in AssetsTree.LevelOrderEntries())
        {
            positions[(int)position.Uid!] = position;
            position.Save(binaryWriter);
        }


        foreach ((Position position, Asset asset) in AssetsTree.LevelOrderEntries())
        {
            asset.Save(binaryWriter);
        }

        // writing the tree
        binaryWriter.Write(AssetsTree.Size);
        foreach ((Position? position, Asset? asset) in AssetsTree.LevelSaveOrder())
        {
            if (position == null || asset == null)
            {
                binaryWriter.Write(false);
            }
            else
            {
                binaryWriter.Write(true);
                binaryWriter.Write((int)position.Uid!);
                if (asset is Parcel parcel)
                {
                    binaryWriter.Write(true);
                    binaryWriter.Write(parcel.Index);
                }
                else if (asset is Realestate realestate)
                {

                    binaryWriter.Write(false);
                    binaryWriter.Write(realestate.Index);
                }
            }
        }

        binaryWriter.Write(ParcelsCount);
        foreach ((Position? position, Parcel? parcel) in ParcelsTree.LevelSaveOrder())
        {
            if (position == null || parcel == null)
            {
                binaryWriter.Write(false);
            }
            else
            {
                binaryWriter.Write(true);
                binaryWriter.Write((int)position.Uid!);
                binaryWriter.Write(parcel.Index);
            }
        }

        binaryWriter.Write(RealestatesCount);
        foreach ((Position? position, Realestate? realestate) in RealestatesTree.LevelSaveOrder())
        {
            if (position == null || realestate == null)
            {
                binaryWriter.Write(false);
            }
            else
            {
                binaryWriter.Write(true);
                binaryWriter.Write((int)position.Uid!);
                binaryWriter.Write(realestate.Index);
            }
        }
    }

    public void LoadSystem(BinaryReader reader)
    {
        Position[] positions = new Position[AssetsCount * 2];
        int positionsCount = reader.ReadInt32();
        for (int i = 0; i < positionsCount; i++)
        {
            Position position = Position.Load(reader);
            positions[(int)position.Uid!] = position;
        }

        int assetsCount = reader.ReadInt32();
        Asset[] assets = new Asset[assetsCount];
        for (int i = 0; i < assetsCount; i++)
        {
            Asset asset = Asset.Load(reader, positions);
            if (asset is Parcel parcel)
            {
                assets[parcel.Index] = parcel;
            }  else if (asset is Realestate realestate)
            {
                assets[realestate.Index] = realestate;
            }
        }

        assetsCount = reader.ReadInt32();
        for (int i = 0; i < assetsCount; i++)
        {


        }





    }
}