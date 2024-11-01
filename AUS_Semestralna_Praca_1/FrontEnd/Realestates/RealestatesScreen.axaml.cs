using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AUS_Semestralna_Praca_1.FrontEnd.Realestates;

public partial class RealestatesScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public RealestatesScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnAddRealestateClicked(object? sender, RoutedEventArgs e)
    {
        _contentArea.Content = new Realestates.AddRealestateScreen(_contentArea);
    }

    private void OnFindRealestate(object? sender, RoutedEventArgs e)
    {
        _contentArea.Content = new Realestates.FindRealestatesScreen(_contentArea);
    }

    private void OnFindAllRealestates(object? sender, RoutedEventArgs e)
    {

        (Answer answer, List<string> realestates) = MainApplication.Instance.FindAllAssets('R');

        if (answer.State is not AnswerState.Ok)
        {
            new MyMessageBox(answer).Show();
            return;
        }

        _contentArea.Content = new RealestatesListScreen(realestates);
    }
}