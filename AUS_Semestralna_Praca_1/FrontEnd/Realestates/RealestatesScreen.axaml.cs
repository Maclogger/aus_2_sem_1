using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd;

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
        _contentArea.Content = new AddRealestateScreen(_contentArea);
    }

    private void OnFindRealestate(object? sender, RoutedEventArgs e)
    {
        _contentArea.Content = new FindRealestatesScreen(_contentArea);
    }
}