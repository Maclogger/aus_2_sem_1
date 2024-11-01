using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class StatusScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public StatusScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }
}