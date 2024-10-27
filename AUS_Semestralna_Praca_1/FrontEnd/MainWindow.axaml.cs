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
        ContentArea.Content = new Parcels();
    }

    private void OnScreen1Clicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new Parcels();
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

    private void OnParcelAndRealestates(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OnStatusClicked(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new StatusPage(ContentArea);
    }
}