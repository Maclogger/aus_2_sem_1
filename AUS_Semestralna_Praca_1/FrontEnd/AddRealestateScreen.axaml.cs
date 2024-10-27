using System;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class AddRealestateScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public AddRealestateScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnAddRealestateClicked(object? sender, RoutedEventArgs e)
    {
        string pos1Attr = "";
        string pos2Attr = "";
        string parcelAttr = "";


        ClientSys.AddToAttr(ref pos1Attr, "LAT", Decimal.ToDouble((decimal)Latitude1.Value!));
        ClientSys.AddToAttr(ref pos1Attr, "LON", Decimal.ToDouble((decimal)Longitude1.Value!));
        ClientSys.AddToAttr(ref pos1Attr, "LAT_SIGN", (Longitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref pos1Attr, "LON_SIGN", (Longitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");

        ClientSys.AddToAttr(ref pos2Attr, "LAT", Decimal.ToDouble((decimal)Latitude2.Value!));
        ClientSys.AddToAttr(ref pos2Attr, "LON", Decimal.ToDouble((decimal)Longitude2.Value!));
        ClientSys.AddToAttr(ref pos2Attr, "LAT_SIGN", (Latitude2Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref pos2Attr, "LON_SIGN", (Longitude2Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");

        ClientSys.AddToAttr(ref parcelAttr, "REALESTATE_NUM", RealestateNum.Text!);
        ClientSys.AddToAttr(ref parcelAttr, "DESCRIPTION", Description.Text!);

        Answer answer = MainApplication.Instance.AddRealestate(pos1Attr, pos2Attr, parcelAttr);

        new MyMessageBox(answer).Show();
    }
}