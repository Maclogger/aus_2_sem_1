using System;
using System.Collections.Generic;
using System.IO;
using AUS_Semestralna_Praca_1.BackEnd.Files;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace AUS_Semestralna_Praca_1.FrontEnd.Files;

public partial class FileSystemScreen : UserControl
{
    private readonly ContentControl _contentArea;

    public FileSystemScreen(ContentControl contentArea)
    {
        _contentArea = contentArea;
        InitializeComponent();
    }


    private async void OnSaveSystemClicked(object? sender, RoutedEventArgs e)
    {
        // Získajte najvyššiu úroveň pre aktuálny ovládací prvok.
        TopLevel? topLevel = TopLevel.GetTopLevel(this);

        // Spustí asynchrónnu operáciu na otvorenie dialógu.
        IStorageFile? file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Uložiť CSV súbor",
            FileTypeChoices = new[] { new FilePickerFileType("CSV Files") { Patterns = new[] { "*.csv" } } },
            SuggestedFileName = "DataFile"
        });

        if (file is not null)
        {
            await using var stream = await file.OpenWriteAsync();
            using var writer = new StreamWriter(stream);

            CsvWriter csvWriter = new CsvWriter(writer);
            // Volajte metódu na uloženie údajov v CSV formáte
            MainApplication.Instance.SaveSystem(csvWriter);

            await writer.FlushAsync();
        }
    }

    private async void OnLoadSystemClicked(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        IReadOnlyList<IStorageFile?> files = await topLevel!.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Vyberte CSV súbor",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType(".csv") }
            });

        IStorageFile? file = files[0];
        if (file != null)
        {
            await using var stream = await file.OpenReadAsync();
            using var streamReader = new StreamReader(stream);

            CsvReader reader = new(streamReader);

            // Volajte metódu na načítanie údajov z CSV formátu
            MainApplication.Instance.LoadSystem(reader);
        }
    }
}

