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
        ContentArea.Content = new StatusScreen(ContentArea);
    }

    private void OnScreen2Clicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new Screen2();
    }

    private void OnConfigClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new ConfigSettings();
    }

    private void OnExitClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnAssetsClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new AssetsScreen(ContentArea);
    }

    private void OnStatusClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new StatusScreen(ContentArea);
    }

    private void OnParcelsClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new ParcelsScreen(ContentArea);
    }

    private void OnRealestatesClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new RealestatesScreen(ContentArea);
    }

    private void OnTesterClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new KdTreeTesterScreen(ContentArea);
    }
}