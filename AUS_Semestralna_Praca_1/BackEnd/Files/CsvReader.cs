using System;
using System.IO;

namespace AUS_Semestralna_Praca_1.BackEnd.Files;

public class CsvReader
{
    private readonly StreamReader _reader;
    private readonly char _separator = Config.Instance.SeparatorInCSV;

    public CsvReader(StreamReader reader)
    {
        _reader = reader;
    }

    public string ReadString()
    {
        string riadok = _reader.ReadLine()!;

        string[] splitted = riadok.Split(_separator);

        return splitted[1];
    }

    public char ReadChar()
    {
        return _reader.ReadLine()!.Split(_separator)[1][0];
    }

    public int ReadInt()
    {
        return Int32.Parse(_reader.ReadLine()!.Split(_separator)[1]);
    }

    public double ReadDouble()
    {
        return Double.Parse(_reader.ReadLine()!.Split(_separator)[1]);
    }

    public bool ReadBool()
    {
        return Boolean.Parse(_reader.ReadLine()!.Split(_separator)[1]);
    }
}