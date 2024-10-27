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
}