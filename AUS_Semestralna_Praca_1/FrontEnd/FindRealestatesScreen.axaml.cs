using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class FindRealestatesScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public FindRealestatesScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnFindRealestatesClicked(object? sender, RoutedEventArgs e)
    {
        string posAttr = "";

        ClientSys.AddToAttr(ref posAttr, "LAT", Decimal.ToDouble((decimal)Latitude1.Value!));
        ClientSys.AddToAttr(ref posAttr, "LON", Decimal.ToDouble((decimal)Longitude1.Value!));
        ClientSys.AddToAttr(ref posAttr, "LAT_SIGN", (Longitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "N");
        ClientSys.AddToAttr(ref posAttr, "LON_SIGN", (Longitude1Sign.SelectedValue as ComboBoxItem)!.Content!.ToString() ?? "W");

        Tuple<Answer,List<string>> foundTuple = MainApplication.Instance.FindRealestates(posAttr);
        if (foundTuple.Item1.State is not AnswerState.Ok)
        {
            new MyMessageBox(foundTuple.Item1).Show();
            return;
        }








    }
}