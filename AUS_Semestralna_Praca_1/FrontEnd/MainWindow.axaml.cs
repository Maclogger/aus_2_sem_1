using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Tmds.DBus.Protocol;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();// Nastavte svetlú tému pre aplikáciu
        Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
        ContentArea.Content = new Assets.StatusScreen(ContentArea);
    }

    private void OnConfigClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new Utils.ConfigSettings();
    }

    private void OnExitClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnAssetsClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new Assets.AssetsScreen(ContentArea);
    }

    private void OnStatusClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new Assets.StatusScreen(ContentArea);
    }

    private void OnParcelsClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new ParcelsScreen(ContentArea);
    }

    private void OnRealestatesClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new Realestates.RealestatesScreen(ContentArea);
    }

    private void OnTesterClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new Testing.KdTreeTesterScreen(ContentArea);
    }
}