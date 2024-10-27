using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class Realestate : Asset
{
    private int _realestateNum; // súpisné číslo
    private string _description; // popis
    private List<Parcel> _parcelas = new(); // parcely
    private Position _pos1, _pos2;
    private int? _uid1, _uid2;

    public Realestate(int pRealestateNum, string pDescription, Position pPos1, Position pPos2)
    {
        _realestateNum = pRealestateNum;
        _description = pDescription;
        _pos1 = pPos1;
        _pos2 = pPos2;
    }


    public void AddParcel(Parcel parcel)
    {
        if (!_parcelas.Contains(parcel))
        {
            _parcelas.Add(parcel);
        }
    }

    public void RemoveParcel(Parcel parcelToDelete)
    {
        _parcelas.Remove(parcelToDelete);
    }
    
    public int? Uid1
    {
        get => _uid1;
        set => _uid1 = value;
    }

    public int? Uid2
    {
        get => _uid2;
        set => _uid2 = value;
    }

    public override void ToAttr(ref string attr)
    {
        ClientSys.AddToAttr(ref attr, "POS_1", _pos1.ToString());
        ClientSys.AddToAttr(ref attr, "POS_2", _pos2.ToString());
        ClientSys.AddToAttr(ref attr, "REALESTATE_NUM", _realestateNum);
        ClientSys.AddToAttr(ref attr, "DESCRIPTION", _description);
        ClientSys.AddToAttr(ref attr, "UID_1", (int)_uid1!);
        ClientSys.AddToAttr(ref attr, "UID_2", (int)_uid2!);
    }
}