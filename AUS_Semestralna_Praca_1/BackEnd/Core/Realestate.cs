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
    public int NumberOfSaves { get; set; } = 0;

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
        if (Config.Instance.FormattedOutput)
        {
            string sol = "";

            sol += $"\nNehnuteľnosť ({Pos1}-{Pos2})\n";
            sol += $"Číslo parcely: {RealestateNum}\n";
            sol += $"Popis: '{Description}'\n";

            return sol;
        }

        return $"{RealestateNum}: {Description}";
    }

    public static Realestate GetDeepCopy(Realestate realestate)
    {
        return new Realestate(
            realestate.RealestateNum,
            realestate.Description,
            Position.GetDeepCopy(realestate.Pos1),
            Position.GetDeepCopy(realestate.Pos2));
    }

    public void Save(CsvWriter writer)
    {
        writer.Write("realestate_num", RealestateNum);
        writer.Write("description", Description);
        Pos1.Save(writer);
        Pos2.Save(writer);
    }

    public static Realestate Load(CsvReader reader)
    {
        return new Realestate(reader.ReadInt(), reader.ReadString(), Position.Load(reader), Position.Load(reader));
    }
}