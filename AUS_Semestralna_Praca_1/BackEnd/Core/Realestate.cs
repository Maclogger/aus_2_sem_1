using System;
using System.Collections.Generic;
using System.IO;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class Realestate : Asset
{
    public int RealestateNum { get; set; } // súpisné číslo
    public string Description { get; set; } // popis
    public List<Parcel> Parcelas { get; set; } = new(); // parcely
    public Position Pos1 { get; set; }
    public Position Pos2 { get; set; }
    public int Index { get; set; } = Utils.GetNextIndex();

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
        if (!Parcelas.Contains(parcel))
        {
            Parcelas.Add(parcel);
        }
    }

    public void RemoveParcel(Parcel parcelToDelete)
    {
        Parcelas.Remove(parcelToDelete);
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

    public override void Save(BinaryWriter binaryWriter)
    {
        binaryWriter.Write('R');
        binaryWriter.Write(Index);
        binaryWriter.Write(RealestateNum);
        binaryWriter.Write(Description);
        binaryWriter.Write((int)Pos1.Uid!);
        binaryWriter.Write((int)Pos2.Uid!);
    }

    public static Realestate Load(BinaryReader reader, Position[] positions)
    {
        int index = reader.ReadInt32();
        int parcelNum = reader.ReadInt32();
        string description = reader.ReadString();
        Position pos1 = positions[reader.ReadInt32()];
        Position pos2 = positions[reader.ReadInt32()];

        Realestate realestate = new Realestate(parcelNum, description, pos1, pos2);
        realestate.Index = index;
        return realestate;
    }
}