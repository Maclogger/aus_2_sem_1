using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AUS_Semestralna_Praca_1.FrontEnd.Assets;

public partial class AssetsScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public AssetsScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnFindAssetsClicked(object? sender, RoutedEventArgs e)
    {
        string posAttr1 = "";
        string posAttr2 = "";

        ClientSys.AddToAttr(ref posAttr1, "LAT", Decimal.ToDouble((decimal)Latitude1.Value!));
        ClientSys.AddToAttr(ref posAttr1, "LON", Decimal.ToDouble((decimal)Longitude1.Value!));
        ClientSys.AddToAttr(ref posAttr1, "LAT_SIGN", (Latitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref posAttr1, "LON_SIGN", (Longitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");


        ClientSys.AddToAttr(ref posAttr2, "LAT", Decimal.ToDouble((decimal)Latitude2.Value!));
        ClientSys.AddToAttr(ref posAttr2, "LON", Decimal.ToDouble((decimal)Longitude2.Value!));
        ClientSys.AddToAttr(ref posAttr2, "LAT_SIGN", (Latitude2Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref posAttr2, "LON_SIGN", (Longitude2Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");

        (Answer answer, List<string> assets) = MainApplication.Instance.FindAssets(posAttr1, posAttr2);

        if (answer.State is not AnswerState.Ok)
        {
            new MyMessageBox(answer).Show();
            return;
        }

        _contentArea.Content = new AssetsScreenList(assets, 'A');
    }

    private void OnFindAllAssetsClicked(object? sender, RoutedEventArgs e)
    {
        (Answer answer, List<string> assets) = MainApplication.Instance.FindAllAssets('A');

        if (answer.State is not AnswerState.Ok)
        {
            new MyMessageBox(answer).Show();
            return;
        }

        _contentArea.Content = new AssetsScreenList(assets, 'A');
    }
}