using Avalonia;
using System;
using AUS_Semestralna_Praca_1.FrontEnd;

namespace AUS_Semestralna_Praca_1;

partial class Program
{
    /*
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
        */
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        //(MainApplication.Instance.FillUpSystem(0.1, 20_000, 0.5);

    }


    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
