using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AUS_Semestralna_Praca_1.FrontEnd.Testing;

public partial class KdTreeTesterScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public KdTreeTesterScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnRun4DTestClicked(object? sender, RoutedEventArgs e)
    {
        MainApplication.Instance.RunTest(MyTextBlock);
    }

    private void OnPrintTreeClicked(object? sender, RoutedEventArgs e)
    {
        // TODO MainApplication.Instance.PrintOut4DTree(MyTextBlock);
    }
}