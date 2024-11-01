using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class Realestate : Asset
{
    public int RealestateNum { get; set; } // súpisné číslo
    public string Description { get; set; } // popis
    public List<Parcel> Parcelas { get; set; } = new(); // parcely
    public Position Pos1 { get; set; }
    public Position Pos2 { get; set; }

    public Realestate(int pRealestateNum, string pDescription, Position pPos1, Position pPos2)
    {
        RealestateNum = pRealestateNum;
        Description = pDescription;
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
}