using System;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class AddParcelScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public AddParcelScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnAddParcelClicked(object? sender, RoutedEventArgs e)
    {
        string pos1Attr = "";
        string pos2Attr = "";
        string parcelAttr = "";

        ClientSys.AddToAttr(ref pos1Attr, "LAT", Decimal.ToDouble((decimal)Latitude1.Value!));
        ClientSys.AddToAttr(ref pos1Attr, "LON", Decimal.ToDouble((decimal)Longitude1.Value!));
        ClientSys.AddToAttr(ref pos1Attr, "LAT_SIGN",
            (Latitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref pos1Attr, "LON_SIGN",
            (Longitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");

        ClientSys.AddToAttr(ref pos2Attr, "LAT", Decimal.ToDouble((decimal)Latitude2.Value!));
        ClientSys.AddToAttr(ref pos2Attr, "LON", Decimal.ToDouble((decimal)Longitude2.Value!));
        ClientSys.AddToAttr(ref pos2Attr, "LAT_SIGN",
            (Latitude2Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref pos2Attr, "LON_SIGN",
            (Longitude2Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");

        ClientSys.AddToAttr(ref parcelAttr, (string)"NUM", (string)ParcelNum.Text!);
        ClientSys.AddToAttr(ref parcelAttr, (string)"DESCRIPTION", (string)Description.Text!);

        Answer answer = MainApplication.Instance.AddAsset(pos1Attr, pos2Attr, parcelAttr, 'P');

        new MyMessageBox(answer).Show();
    }

    private void OnRandomParcelClicked(object? sender, RoutedEventArgs e)
    {
        Latitude1.Value =
            AUS_Semestralna_Praca_1.BackEnd.DataStructures.Utils.GetRandomIntInRange(
                (int)Math.Ceiling(Config.Instance.MinLatitudeAbs), (int)Config.Instance.MaxLatitudeAbs);
        Latitude2.Value =
            AUS_Semestralna_Praca_1.BackEnd.DataStructures.Utils.GetRandomIntInRange(
                (int)Math.Ceiling(Config.Instance.MinLatitudeAbs), (int)Config.Instance.MaxLatitudeAbs);
        Longitude1.Value =
            AUS_Semestralna_Praca_1.BackEnd.DataStructures.Utils.GetRandomIntInRange(
                (int)Math.Ceiling(Config.Instance.MinLongitudeAbs), (int)Config.Instance.MaxLongitudeAbs);
        Longitude2.Value =
            AUS_Semestralna_Praca_1.BackEnd.DataStructures.Utils.GetRandomIntInRange(
                (int)Math.Ceiling(Config.Instance.MinLongitudeAbs), (int)Config.Instance.MaxLongitudeAbs);
        Description.Text = AUS_Semestralna_Praca_1.BackEnd.DataStructures.Utils.GetNextStringValOfLentgth(10);
        ParcelNum.Value = AUS_Semestralna_Praca_1.BackEnd.DataStructures.Utils.GetRandomIntInRange(0, 100);
        InvalidateVisual();
    }
}