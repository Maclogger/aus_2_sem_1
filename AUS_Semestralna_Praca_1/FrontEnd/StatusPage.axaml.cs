using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public partial class StatusPage : UserControl
{
    private readonly ContentControl _contentArea;

    public StatusPage(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }
}