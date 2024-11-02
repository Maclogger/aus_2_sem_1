using System;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd.Assets;

public partial class EditSpecAssetScreen : UserControl
{
    private readonly AssetData _oldAssetData;
    private readonly ContentControl _contentArea;
    private readonly AssetData _newAssetData; // deep copy

    public EditSpecAssetScreen(AssetData oldAssetData, ContentControl contentArea)
    {
        _oldAssetData = oldAssetData;
        _contentArea = contentArea;
        _newAssetData = new(oldAssetData);
        InitializeComponent();

        Headline.Text = _oldAssetData.Type == "R" ? "Editácia nehnuteľnosti" : "Editácia parcely";

        Latitude1.Value = (decimal?)_newAssetData.Pos1Data.Latitude;
        Latitude1Sign.SelectedIndex = _newAssetData.Pos1Data.LatitudeSign == "N" ? 0 : 1;
        Longitude1.Value = (decimal?)_newAssetData.Pos1Data.Longitude;
        Longitude1Sign.SelectedIndex = _newAssetData.Pos1Data.LongitudeSign == "E" ? 0 : 1;

        Latitude2.Value = (decimal?)_newAssetData.Pos2Data.Latitude;
        Latitude2Sign.SelectedIndex = _newAssetData.Pos2Data.LatitudeSign == "N" ? 0 : 1;
        Longitude2.Value = (decimal?)_newAssetData.Pos2Data.Longitude;
        Longitude2Sign.SelectedIndex = _newAssetData.Pos2Data.LongitudeSign == "E" ? 0 : 1;

        Description.Text = _newAssetData.Description;
        Num.Value = _newAssetData.Num;
        ButtonEdit.Content = _oldAssetData.Type == "R" ? "Editovať nehnuteľnosť" : "Editovať parcelu";
    }

    private void OnEditAssetClicked(object? sender, RoutedEventArgs e)
    {
        _newAssetData.Pos1Data.Latitude = (double)Latitude1.Value!;
        _newAssetData.Pos1Data.LatitudeSign = Latitude1Sign.SelectedIndex == 0 ? "N" : "S";
        _newAssetData.Pos1Data.Longitude = (double)Longitude1.Value!;
        _newAssetData.Pos1Data.LongitudeSign = Longitude1Sign.SelectedIndex == 0 ? "E" : "W";

        _newAssetData.Pos2Data.Latitude = (double)Latitude2.Value!;
        _newAssetData.Pos2Data.LatitudeSign = Latitude2Sign.SelectedIndex == 0 ? "N" : "S";
        _newAssetData.Pos2Data.Longitude = (double)Longitude2.Value!;
        _newAssetData.Pos2Data.LongitudeSign = Longitude2Sign.SelectedIndex == 0 ? "E" : "W";

        _newAssetData.Description = Description.Text ?? string.Empty;
        _newAssetData.Num = (int)(Num.Value ?? 0);

        Answer answer = MainApplication.Instance.UpdateAsset(_oldAssetData, _newAssetData);

        new MyMessageBox(answer).Show();

        if (answer.State == AnswerState.Ok)
        {
            _contentArea.Content = new StatusScreen(_contentArea);
        }
    }
}