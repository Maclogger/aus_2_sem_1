using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AUS_Semestralna_Praca_1.FrontEnd.Assets;

public partial class FindSpecAssetsScreen : UserControl
{
    public char Sign { get; }
    private ContentControl _contentArea;
    //public string Headline => Sign == 'R' ? "Vyhľadávanie nehnuteľností" : "Vyhľadávanie parciel";
    public FindSpecAssetsScreen(ContentControl contentArea, char sign)
    {
        Sign = sign;
        _contentArea = contentArea;
        InitializeComponent();
        Headline.Text = Sign == 'R' ? "Vyhľadávanie nehnuteľností" : "Vyhľadávanie parciel";
        FindButton.Content = Sign == 'R' ? "Vyhľadať nehnuteľnsti" : "Vyhľadať parcely";
    }

    private void OnFindAssetClicked(object? sender, RoutedEventArgs e)
    {
        string posAttr = "";

        ClientSys.AddToAttr(ref posAttr, "LAT", Decimal.ToDouble((decimal)Latitude1.Value!));
        ClientSys.AddToAttr(ref posAttr, "LON", Decimal.ToDouble((decimal)Longitude1.Value!));
        ClientSys.AddToAttr(ref posAttr, "LAT_SIGN", (Latitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref posAttr, "LON_SIGN", (Longitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");


        (Answer answer, List<string> specificAssets) = MainApplication.Instance.FindAssets(posAttr, Sign);

        if (answer.State is not AnswerState.Ok)
        {
            new MyMessageBox(answer).Show();
            return;
        }

        _contentArea.Content = new AssetsScreenList(specificAssets, 'R', _contentArea);
    }
}