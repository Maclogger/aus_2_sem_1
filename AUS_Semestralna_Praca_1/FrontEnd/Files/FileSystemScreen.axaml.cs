using System;
using System.Collections.Generic;
using System.IO;
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
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        TopLevel? topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        IStorageFile? file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Binary File",
            FileTypeChoices = new[] { new FilePickerFileType("Binary Files") { Patterns = new[] { "*.bin" } } },
            SuggestedFileName = "DataFile"
        });

        if (file is not null)
        {
            await using var stream = await file.OpenWriteAsync();
            using var binaryWriter = new BinaryWriter(stream);

            MainApplication.Instance.SaveSystem(binaryWriter);

            binaryWriter.Flush();
        }
    }

    private async void OnLoadSystemClicked(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        IReadOnlyList<IStorageFile?> files = await topLevel!.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Vyberte súbor",
                AllowMultiple = false,
                FileTypeFilter = [new FilePickerFileType(".bin")]
            });

        IStorageFile? file = files[0];
        if (file != null)
        {
            await using var stream = await file.OpenReadAsync();
            using var binaryReader = new BinaryReader(stream);

            MainApplication.Instance.LoadSystem(binaryReader);

            binaryReader.Close();
        }


    }
}