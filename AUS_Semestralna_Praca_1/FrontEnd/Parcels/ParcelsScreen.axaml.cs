using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.FrontEnd.Assets;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class ParcelsScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public ParcelsScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnAddParcelClicked(object? sender, RoutedEventArgs e)
    {
        _contentArea.Content = new AddParcelScreen(_contentArea);
    }

    private void OnFindParcelsClicked(object? sender, RoutedEventArgs e)
    {
        _contentArea.Content = new FindSpecAssetsScreen(_contentArea, 'P');
    }

    private void OnFindAllParcelsClicked(object? sender, RoutedEventArgs e)
    {
        (Answer answer, List<string> realestates) = MainApplication.Instance.FindAllAssets('P');

        if (answer.State is not AnswerState.Ok)
        {
            new MyMessageBox(answer).Show();
            return;
        }

        _contentArea.Content = new AssetsScreenList(realestates, 'P');
    }
}