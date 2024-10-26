using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;

namespace AUS_Semestralna_Praca_1.BackEnd.CoreGui;

public class GUI
{
    private Application _application;

    public GUI(Application application)
    {
        _application = application;
    }

    public int ChooseFromList(List<string?> options)
    {
        for (var i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"[i]: {options[i]}");
        }

        Console.WriteLine("Choose: ");
        return Int32.Parse(Console.ReadLine() ?? string.Empty);
    }

    public void Run()
    {
        string pos1Attr = "";
        string pos2Attr = "";
        string parcelAttr = "";

        ClientSys.AddToAttr(ref pos1Attr, "LAT", 0.0);
        ClientSys.AddToAttr(ref pos1Attr, "LON", 0.0);
        ClientSys.AddToAttr(ref pos1Attr, "LAT_SIGN", "E");
        ClientSys.AddToAttr(ref pos1Attr, "LON_SIGN", "N");

        ClientSys.AddToAttr(ref pos2Attr, "LAT", 10.0);
        ClientSys.AddToAttr(ref pos2Attr, "LON", 15.0);
        ClientSys.AddToAttr(ref pos2Attr, "LAT_SIGN", "E");
        ClientSys.AddToAttr(ref pos2Attr, "LON_SIGN", "N");

        ClientSys.AddToAttr(ref parcelAttr, "PARCEL_NUM", 10);
        ClientSys.AddToAttr(ref parcelAttr, "DESCRIPTION", "Prva parcela");


        Answer answer = _application.AddParcel(pos1Attr, pos2Attr, parcelAttr);
        answer.PrintOut();


        pos1Attr = "";
        pos2Attr = "";
        parcelAttr = "";

        ClientSys.AddToAttr(ref pos1Attr, "LAT", 0.0);
        ClientSys.AddToAttr(ref pos1Attr, "LON", 0.0);
        ClientSys.AddToAttr(ref pos1Attr, "LAT_SIGN", "E");
        ClientSys.AddToAttr(ref pos1Attr, "LON_SIGN", "N");

        ClientSys.AddToAttr(ref pos2Attr, "LAT", 10.0);
        ClientSys.AddToAttr(ref pos2Attr, "LON", 15.0);
        ClientSys.AddToAttr(ref pos2Attr, "LAT_SIGN", "E");
        ClientSys.AddToAttr(ref pos2Attr, "LON_SIGN", "N");

        ClientSys.AddToAttr(ref parcelAttr, "PARCEL_NUM", 11);
        ClientSys.AddToAttr(ref parcelAttr, "DESCRIPTION", "Druha parcela");

        answer = _application.AddParcel(pos1Attr, pos2Attr, parcelAttr);
        answer.PrintOut();


        pos1Attr = "";
        pos2Attr = "";
        parcelAttr = "";

        ClientSys.AddToAttr(ref pos1Attr, "LAT", 0.0);
        ClientSys.AddToAttr(ref pos1Attr, "LON", 0.0);
        ClientSys.AddToAttr(ref pos1Attr, "LAT_SIGN", "W");
        ClientSys.AddToAttr(ref pos1Attr, "LON_SIGN", "S");

        ClientSys.AddToAttr(ref pos2Attr, "LAT", 10.0);
        ClientSys.AddToAttr(ref pos2Attr, "LON", 15.0);
        ClientSys.AddToAttr(ref pos2Attr, "LAT_SIGN", "W");
        ClientSys.AddToAttr(ref pos2Attr, "LON_SIGN", "S");

        ClientSys.AddToAttr(ref parcelAttr, "PARCEL_NUM", 12);
        ClientSys.AddToAttr(ref parcelAttr, "DESCRIPTION", "Tretia parcela");

        answer = _application.AddParcel(pos1Attr, pos2Attr, parcelAttr);
        answer.PrintOut();

        Console.WriteLine("\n\n\n");
        _application.PrintParcelTree();
    }
}