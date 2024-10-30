using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class KdTreeTesterScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public KdTreeTesterScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
        MainApplication.Instance.InitializeTestTrees();
    }

    private void OnRun4DTestClicked(object? sender, RoutedEventArgs e)
    {
        MainApplication.Instance.RunTest(MyTextBlock);
    }

    private void OnPrintTreeClicked(object? sender, RoutedEventArgs e)
    {
        MainApplication.Instance.PrintOut4DTree(MyTextBlock);
    }
}