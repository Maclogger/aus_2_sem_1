using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.Core;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1;



public class MainApplication
{
    // SINGLETON
    private static readonly MainApplication _instance = new();
    public static MainApplication Instance => _instance;

    private ApplicationCore _core;
    private ConsoleGui _consoleGui;

    private MainApplication()
    {
        _core = new ApplicationCore(this);
        _consoleGui = new ConsoleGui(this);
    }

    public ApplicationCore? Core
    {
        get => _core;
        set => _core = value;
    }

    // GUI => Core
    public Answer AddParcel(string pos1Attr, string pos2Attr, string parcelAttr)
    {
        int? parcelNum = ClientSys.GetIntFromAttr(parcelAttr, "PARCEL_NUM");
        string description = ClientSys.GetStringFromAttr(parcelAttr, "DESCRIPTION")!;

        Position? pos1;
        Position? pos2;
        try
        {
            pos1 = new(pos1Attr); // it can throw inside
            pos2 = new(pos2Attr); // it can throw inside
            int dummy = (int)parcelNum!; // if parcelNum is not a number => crash
        }
        catch
        {
            return new Answer("Some of the attributes are missing or invalid.", AnswerState.Error);
        }

        Parcel parcel = new((int)parcelNum, description, pos1, pos2);

        return _core.AddParcel(pos1, pos2, parcel);
    }

    public Answer RemoveParcel(string attr)
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        _consoleGui.Run();
    }

    public int GetUidFromUserByChoosingFromList<T>(List<DataPart<T>> list)
    {
        List<string?> optionsForUser = new();
        foreach (DataPart<T> dataPart in list)
        {
            string option = dataPart.Value?.ToString() ?? "NULL";
            optionsForUser.Add(option);
        }
        
        int index = _consoleGui.ChooseFromList(optionsForUser); // TODO implement GUI

        return list[index].Uid;
    }

    public void PrintParcelTree()
    {
        _core.PrintParcelTree();
    }
}