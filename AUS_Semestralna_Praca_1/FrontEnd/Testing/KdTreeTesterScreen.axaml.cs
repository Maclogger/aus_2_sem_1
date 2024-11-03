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
        TreeSizeBlockCount.Text = MainApplication.Instance.TestTree!.Size.ToString();
    }

    private void OnRunTestClicked(object? sender, RoutedEventArgs e)
    {
        MainApplication.Instance.RunSimTest(MyTextBlock);
        TreeSizeBlockCount.Text = MainApplication.Instance.TestTree!.Size.ToString();
    }

    private void OnPrintTreeClicked(object? sender, RoutedEventArgs e)
    {
        MainApplication.Instance.PrintTestTree(MyTextBlock);
    }

    private void OnFillUpTestTreeClicked(object? sender, RoutedEventArgs e)
    {
        MainApplication.Instance.PrepareTestTree();
        TreeSizeBlockCount.Text = MainApplication.Instance.TestTree!.Size.ToString();
    }
}