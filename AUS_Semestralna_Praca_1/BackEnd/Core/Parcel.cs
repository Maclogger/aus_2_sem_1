using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class Parcel : Asset
{
    private int _parcelNum; // číslo parcely
    private string _description; // popis
    private List<Realestate> _realestates = new(); // nehnuteľnosti
    private Position _pos1, _pos2;
    private int? _uid1, _uid2;

    public Parcel(int pParcelNum, string pDescription, Position pPos1, Position pPos2)
    {
        _parcelNum = pParcelNum;
        _description = pDescription;
        _pos1 = pPos1;
        _pos2 = pPos2;
    }


    public int ParcelNum
    {
        get => _parcelNum;
        set => _parcelNum = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public List<Realestate> Realestates
    {
        get => _realestates;
        set => _realestates = value;
    }

    public Position Pos1
    {
        get => _pos1;
        set => _pos1 = value;
    }

    public Position Pos2
    {
        get => _pos2;
        set => _pos2 = value;
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

    public void AddRealestate(Realestate realestate)
    {
        if (!Realestates.Contains(realestate))
        {
            _realestates.Add(realestate);
        }
    }

    public override string ToString()
    {
        if (Config.Instance.FormattedOutput)
        {
            string sol = "";

            sol += $"\nParcela ({_pos1}-{_pos2})\n";
            sol += $"Číslo parcely: {_parcelNum}\n";
            sol += $"Popis: '{_description}'\n";

            return sol;
        }

        return $"{_parcelNum}: {_description}";
    }

    public override void ToAttr(ref string attr)
    {
        ClientSys.AddToAttr(ref attr, "POS_1", _pos1.ToString());
        ClientSys.AddToAttr(ref attr, "POS_2", _pos2.ToString());
        ClientSys.AddToAttr(ref attr, "PARCEL_NUM", _parcelNum);
        ClientSys.AddToAttr(ref attr, "DESCRIPTION", _description);
        ClientSys.AddToAttr(ref attr, "UID_1", (int)_uid1!);
        ClientSys.AddToAttr(ref attr, "UID_2", (int)_uid2!);
    }
}