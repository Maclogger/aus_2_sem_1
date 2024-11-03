using System;
using System.Collections.Generic;
using System.IO;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.Files;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class Realestate : Asset
{
    public int RealestateNum { get; set; } // súpisné číslo
    public string Description { get; set; } // popis
    public List<Parcel> Parcels { get; set; } = new(); // parcely
    public Position Pos1 { get; set; }
    public Position Pos2 { get; set; }
    public int Index { get; set; } = Utils.GetNextIndex();

    public int[] Neighbours { get; set; }

    public Realestate(int pRealestateNum, string pDescription, Position pPos1, Position pPos2)
    {
        RealestateNum = pRealestateNum;
        Description = pDescription;
        Pos1 = pPos1;
        Pos2 = pPos2;
    }

    public Realestate(Random random, Position pPos1, Position pPos2)
    {
        RealestateNum = Utils.GetRandomIntInRange(0, 10_000, random);
        Description = Utils.GetNextStringValOfLentgth(10);
        Pos1 = pPos1;
        Pos2 = pPos2;
    }

    public void AddParcel(Parcel parcel)
    {
        if (!Parcels.Contains(parcel))
        {
            Parcels.Add(parcel);
        }
    }

    public void RemoveParcel(Parcel parcelToDelete)
    {
        Parcels.Remove(parcelToDelete);
    }

    public override void ToAttr(ref string attr)
    {
        Pos1.AddToAttr(ref attr, 1);
        Pos2.AddToAttr(ref attr, 2);
        ClientSys.AddToAttr(ref attr, "NUM", RealestateNum);
        ClientSys.AddToAttr(ref attr, "DESCRIPTION", Description);
        ClientSys.AddToAttr(ref attr, "TYPE", "R");
    }

    public override string ToString()
    {
        return $"[{Pos1}, {Pos2}]";
    }

    public static Realestate GetDeepCopy(Realestate realestate)
    {
        return new Realestate(
            realestate.RealestateNum,
            realestate.Description,
            Position.GetDeepCopy(realestate.Pos1),
            Position.GetDeepCopy(realestate.Pos2));
    }

    public override void Save(CsvWriter writer)
    {
        writer.Write("type", 'R');
        writer.Write("realestate_index", Index);
        writer.Write("realestate_num", RealestateNum);
        writer.Write("description", Description);
        writer.Write("pos_1_uid", (int)Pos1.Uid!);
        writer.Write("pos_2_uid", (int)Pos2.Uid!);

        writer.Write("count_of_neighbours_parcels", Parcels.Count);
        for (var i = 0; i < Parcels.Count; i++)
        {
            var parcel = Parcels[i];
            writer.Write($"parcel_{i}", parcel.Index);
        }
    }

    public static Realestate Load(CsvReader reader, Position[] positions)
    {
        int index = reader.ReadInt();
        int parcelNum = reader.ReadInt();
        string description = reader.ReadString();
        Position pos1 = positions[reader.ReadInt()];
        Position pos2 = positions[reader.ReadInt()];

        Realestate realestate = new Realestate(parcelNum, description, pos1, pos2);
        realestate.Index = index;

        int parcelCount = reader.ReadInt();
        int[] neighbours = new int[parcelCount];
        for (int i = 0; i < parcelCount; i++)
        {
            neighbours[i] = reader.ReadInt();
        }

        realestate.Neighbours = neighbours;

        return realestate;
    }
}