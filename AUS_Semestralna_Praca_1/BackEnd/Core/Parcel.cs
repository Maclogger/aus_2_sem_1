using System;
using System.Collections.Generic;
using System.IO;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.Files;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class Parcel : Asset
{
    public int ParcelNum { get; set; } // číslo parcely
    public string Description { get; set; } // popis
    public List<Realestate> Realestates { get; set; } = new(); // nehnuteľnosti
    public Position Pos1 { get; set; }
    public Position Pos2 { get; set; }

    public int NumberOfSaves { get; set; } = 0;

    public Parcel(int pParcelNum, string pDescription, Position pPos1, Position pPos2)
    {
        ParcelNum = pParcelNum;
        Description = pDescription;
        Pos1 = pPos1;
        Pos2 = pPos2;
    }

    public Parcel(Random random, Position pPos1, Position pPos2)
    {
        ParcelNum = Utils.GetRandomIntInRange(0, 10_000, random);
        Description = Utils.GetNextStringValOfLentgth(10);
        Pos1 = pPos1;
        Pos2 = pPos2;
    }

    public void AddRealestate(Realestate realestate)
    {
        if (!Realestates.Contains(realestate))
        {
            Realestates.Add(realestate);
        }
    }

    public override string ToString()
    {
        if (Config.Instance.FormattedOutput)
        {
            string sol = "";

            sol += $"\nParcela ({Pos1}-{Pos2})\n";
            sol += $"Číslo parcely: {ParcelNum}\n";
            sol += $"Popis: '{Description}'\n";

            return sol;
        }

        return $"{ParcelNum}: {Description}";
    }

    public override void ToAttr(ref string attr)
    {
        Pos1.AddToAttr(ref attr, 1);
        Pos2.AddToAttr(ref attr, 2);
        ClientSys.AddToAttr(ref attr, "NUM", ParcelNum);
        ClientSys.AddToAttr(ref attr, "DESCRIPTION", Description);
        ClientSys.AddToAttr(ref attr, "TYPE", "P");
    }

    public void RemoveRealestate(Realestate realestate)
    {
        Realestates.Remove(realestate);
    }

    public static Parcel GetDeepCopy(Parcel parcel)
    {
        return new Parcel(
            parcel.ParcelNum,
            parcel.Description,
            Position.GetDeepCopy(parcel.Pos1),
            Position.GetDeepCopy(parcel.Pos2));
    }


    public void Save(CsvWriter writer)
    {
        writer.Write("parcel_num", ParcelNum);
        writer.Write("description", Description);
        Pos1.Save(writer);
        Pos2.Save(writer);
    }

    public static Parcel Load(CsvReader reader)
    {
        return new Parcel(reader.ReadInt(), reader.ReadString(), Position.Load(reader), Position.Load(reader));
    }
}