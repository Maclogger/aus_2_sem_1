using System.Globalization;
using System.IO;

namespace AUS_Semestralna_Praca_1.BackEnd.Files;

public class CsvWriter
{
    private readonly StreamWriter _writer;
    private readonly char _separator = Config.Instance.SeparatorInCSV;
    private readonly char _lineSeparator = '\n';

    public CsvWriter(StreamWriter writer)
    {
        _writer = writer;
    }

    public void Write(string key, string value)
    {
        _writer.Write($"{key}{_separator}{value}{_lineSeparator}");
    }

    public void Write(string key, int value)
    {
        Write(key, value.ToString());
    }

    public void Write(string key, double value)
    {
        Write(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void Write(string key, char value)
    {
        Write(key, value.ToString());
    }

    public void Write(string key, bool value)
    {
        Write(key, value.ToString());
    }
}