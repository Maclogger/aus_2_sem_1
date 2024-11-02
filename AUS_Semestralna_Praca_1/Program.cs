using Avalonia;
using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.FrontEnd;
using AUS_Semestralna_Praca_1.FrontEnd.Assets;

namespace AUS_Semestralna_Praca_1;

partial class Program
{
    /*
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
        */

    private static List<AssetData> Transform(List<string> strings)
    {
        List<AssetData> data = new(strings.Count);
        foreach (string attr in strings)
        {
            string type = ClientSys.GetStringFromAttr(attr, "TYPE")!;

            int num = (int)ClientSys.GetIntFromAttr(attr, "NUM")!;
            string description = ClientSys.GetStringFromAttr(attr, "DESCRIPTION")!;

            int uid1 = (int)ClientSys.GetIntFromAttr(attr, "UID_1")!;
            double latitude1 = (double)ClientSys.GetDoubleFromAttr(attr, "LAT_1")!;
            string latitudeSign1 = ClientSys.GetStringFromAttr(attr, "LAT_SIGN_1")!;
            double longitude1 = (double)ClientSys.GetDoubleFromAttr(attr, "LON_1")!;
            string longitudeSign1 = ClientSys.GetStringFromAttr(attr, "LON_SIGN_1")!;

            PositionData pos1Data = new(uid1, latitude1, latitudeSign1, longitude1, longitudeSign1);

            int uid2 = (int)ClientSys.GetIntFromAttr(attr, "UID_2")!;
            double latitude2 = (double)ClientSys.GetDoubleFromAttr(attr, "LAT_2")!;
            string latitudeSign2 = ClientSys.GetStringFromAttr(attr, "LAT_SIGN_2")!;
            double longitude2 = (double)ClientSys.GetDoubleFromAttr(attr, "LON_2")!;
            string longitudeSign2 = ClientSys.GetStringFromAttr(attr, "LON_SIGN_2")!;

            PositionData pos2Data = new(uid2, latitude2, latitudeSign2, longitude2, longitudeSign2);

            data.Add(new AssetData(num, description, pos1Data, pos2Data, type));
        }

        return data;
    }

    public static int counter = 0;
    public static int counter2 = 0;
    [STAThread]
    public static void Main(string[] args)
    {
        MainApplication.Instance.FillUpSystem(1, 100, 0.5);

        (_, List<string> parcelsStrings) = MainApplication.Instance.FindAllAssets('P');
        (_, List<string> realestatesString) = MainApplication.Instance.FindAllAssets('R');

        List<AssetData> parcelData = Transform(parcelsStrings);
        List<AssetData> realestateData = Transform(realestatesString);

        foreach (AssetData data in parcelData)
        {
            MainApplication.Instance.RemoveAsset(data);
        }

        foreach (AssetData data in realestateData)
        {
            MainApplication.Instance.RemoveAsset(data);
        }

        Console.WriteLine("TU SME!!!!");

        Console.WriteLine(MainApplication.Instance.AssetsCount);
        Console.WriteLine(MainApplication.Instance.ParcelCount);
        Console.WriteLine(MainApplication.Instance.RealestateCount);

        MainApplication.Instance.FindAllAssets('P');
        MainApplication.Instance.FindAllAssets('R');
        MainApplication.Instance.FindAllAssets('A');


        //BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }


    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
