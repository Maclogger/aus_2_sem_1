using System.Collections.Generic;
using System.Collections.ObjectModel;
using AUS_Semestralna_Praca_1.BackEnd.Core;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using Avalonia.Controls;

namespace AUS_Semestralna_Praca_1.FrontEnd.Assets;

public class AssetData
{
    public string Type { get; set; }
    public string TypeClient { get; set; }
    public int Num { get; set; }
    public string Description { get; set; }
    public PositionData Pos1Data { get; }
    public PositionData Pos2Data { get; }

    public AssetData(int num, string description, PositionData pos1Data, PositionData pos2Data, string type)
    {
        Type = type;
        Num = num;
        Description = description;
        Pos1Data = pos1Data;
        Pos2Data = pos2Data;
        TypeClient = type == "R" ? "Nehnuteľnosť" : "Parcela";
    }
}

public class PositionData
{
    public int Uid { get; }
    public double Latitude { get; }
    public string LatitudeSign { get; }
    public double Longitude { get; }
    public string LongitudeSign { get; }

    public PositionData(int uid, double latitude, string latitudeSign, double longitude, string longitudeSign)
    {
        Uid = uid;
        Latitude = latitude;
        LatitudeSign = latitudeSign;
        Longitude = longitude;
        LongitudeSign = longitudeSign;
    }

    public string Formatted => Position.ToFormattedString(Uid, Latitude, LatitudeSign, Longitude, LongitudeSign);
}


public partial class AssetsScreenList : UserControl
{
    public ObservableCollection<AssetData> ListAssets { get; set; }

    public AssetsScreenList(List<string> listOfAssets, char sign)
    {
        InitializeComponent();
        List<AssetData> list = new(listOfAssets.Count);
        foreach (string attr in listOfAssets)
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

            list.Add(new AssetData(num, description, pos1Data, pos2Data, type));
        }

        ListAssets = new ObservableCollection<AssetData>(list);
        Headline.Text = "Nájdené " + (sign == 'R' ? "nehnuteľnosti" : "parcely") + ": " + ListAssets.Count;
        DataContext = this;
    }
}