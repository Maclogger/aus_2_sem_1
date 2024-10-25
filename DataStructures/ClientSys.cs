using System.Globalization;

namespace My.DataStructures;

public static class ClientSys
{
    private const char PairSeparator = ';';
    private const char KeyValueSeparator = ',';

    ////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////// ADD ////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public static string AddToAttr(string attr, string key, string value)
    {
        return $"{attr}{key}{PairSeparator}{value}{PairSeparator}";
    }

    public static string AddToAttr(string attr, string key, int value)
    {
        return $"{attr}{key}{PairSeparator}{value.ToString()}{PairSeparator}";
    }

    public static string AddToAttr(string attr, string key, double value)
    {
        return $"{attr}{key}{PairSeparator}{value.ToString(CultureInfo.InvariantCulture)}{PairSeparator}";
    }

    public static string AddToAttr(string attr, string key, bool value)
    {
        return $"{attr}{key}{PairSeparator}{value.ToString()}{PairSeparator}";
    }

    ////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////// GET ////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public static string? GetStringFromAttr(string attr, string pKey)
    {
        return FindValue(attr, pKey);
    }

    public static int? GetIntFromAttr(string attr, string pKey)
    {
        string? value = FindValue(attr, pKey);
        return int.TryParse(value, out int result) ? result : null;
    }

    public static double? GetDoubleFromAttr(string attr, string pKey)
    {
        string? value = FindValue(attr, pKey);
        return double.TryParse(value, out double result) ? result : null;
    }
    
    public static bool? GetBoolFromAttr(string attr, string pKey)
    {
        string? value = FindValue(attr, pKey);
        return bool.TryParse(value, out bool result) ? result : null;
    }

    ////////////////////////////////////////////////////////////////////////////
    ////////////////////////////// IMPLEMENTATION //////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    private static List<Tuple<string, string>> Process(string attr)
    {
        List<Tuple<string, string>> sol = new();

        string[] pairs = attr.Split(PairSeparator);

        foreach (string pair in pairs)
        {
            string key = pair.Split(KeyValueSeparator)[0];
            string value = pair.Split(KeyValueSeparator)[1];

            sol.Add(new Tuple<string, string>(key, value));
        }

        return sol;
    }

    private static string? FindValue(string attr, string key)
    {
        foreach (Tuple<string, string> tuple in Process(attr))
        {
            if (tuple.Item1 == key)
            {
                return tuple.Item2;
            }
        }

        return null;
    }

}