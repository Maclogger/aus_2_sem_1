using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AUS_Semestralna_Praca_1.BackEnd.Core;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AUS_Semestralna_Praca_1.FrontEnd.Assets;

public class AssetData
{
    public string Type { get; set; }
    public string TypeClient { get; set; }
    public int Num { get; set; }
    public string Description { get; set; }
    public PositionData Pos1Data { get; set; }
    public PositionData Pos2Data { get; set; }

    public string OverlayButtonText { get; set; }

    public AssetData(int num, string description, PositionData pos1Data, PositionData pos2Data, string type)
    {
        Type = type;
        Num = num;
        Description = description;
        Pos1Data = pos1Data;
        Pos2Data = pos2Data;
        TypeClient = type == "R" ? "Nehnuteľnosť" : "Parcela";
        OverlayButtonText = type == "R" ? "Parcely" : "Nehnuteľnosti";
    }

    public AssetData(AssetData other)
    {
        Type = other.Type;
        Num = other.Num;
        Description = other.Description;
        Pos1Data = new PositionData(other.Pos1Data);
        Pos2Data = new PositionData(other.Pos2Data);
        TypeClient = Type == "R" ? "Nehnuteľnosť" : "Parcela";
    }
}

public class PositionData
{
    public int Uid { get; set; }
    public double Latitude { get; set; }
    public string LatitudeSign { get; set; }
    public double Longitude { get; set; }
    public string LongitudeSign { get; set; }

    public PositionData(int uid, double latitude, string latitudeSign, double longitude, string longitudeSign)
    {
        Uid = uid;
        Latitude = latitude;
        LatitudeSign = latitudeSign;
        Longitude = longitude;
        LongitudeSign = longitudeSign;
    }

    public PositionData(PositionData other)
    {
        Uid = other.Uid;
        Latitude = other.Latitude;
        LatitudeSign = other.LatitudeSign;
        Longitude = other.Longitude;
        LongitudeSign = other.LongitudeSign;
    }

    public string Formatted => Position.ToFormattedString(Uid, Latitude, LatitudeSign, Longitude, LongitudeSign);
}

public partial class AssetsScreenList : UserControl
{
    private readonly ContentControl _contentArea;
    public ObservableCollection<AssetData> ListAssets { get; set; }

    public AssetsScreenList(List<string> listOfAssets, char sign, ContentControl contentArea)
    {
        _contentArea = contentArea;
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

        if (sign == 'A')
        {
            Headline.Text = $"Nájdené nehnuteľnosti a parcely: {ListAssets.Count}";
        }
        else
        {
            Headline.Text = "Nájdené " + (sign == 'R' ? "nehnuteľnosti" : "parcely") + ": " + ListAssets.Count;
        }

        DataContext = this;
    }


    private void OnRemoveRecordClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is AssetData asset)
        {
            ListAssets.Remove(asset);
            MainApplication.Instance.RemoveAsset(asset);
        }
    }

    private void OnEditRecordClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is AssetData asset)
        {
            _contentArea.Content = new EditSpecAssetScreen(asset, _contentArea);
        }
    }

    private void OnShowOverlayingAssetsClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is AssetData asset)
        {
            (Answer answer, List<string> assets) = MainApplication.Instance.GetOverlayingAssets(asset);

            if (answer.State is not AnswerState.Ok)
            {
                new MyMessageBox(answer).Show();
                return;
            }

            _contentArea.Content = new AssetsScreenList(assets, 'A', _contentArea);
        }
    }
}