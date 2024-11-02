using System;
using System.Diagnostics;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd.Assets;

public partial class AssetsGeneratorScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public AssetsGeneratorScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }

    private void OnFillUpSystemClicked(object? sender, RoutedEventArgs e)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Answer answer = MainApplication.Instance.FillUpSystem((double)ProbabilityOfDuplicates.Value!, (int)ElementCount.Value!, RealestateParcelSlider.Value);
        sw.Stop();
        Console.WriteLine($"FillUpSystem finished in {sw.ElapsedMilliseconds} ms.");
        new MyMessageBox(answer).Show();
    }
}