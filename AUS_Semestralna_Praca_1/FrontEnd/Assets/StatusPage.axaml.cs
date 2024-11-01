using Avalonia.Controls;

namespace AUS_Semestralna_Praca_1.FrontEnd.Assets;

public partial class StatusScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public StatusScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }
}