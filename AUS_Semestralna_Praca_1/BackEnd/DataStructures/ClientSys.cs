using System;
using System.Collections.Generic;
using System.Globalization;

namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures;

public static class ClientSys
{
    private const char PairSeparator = '@';
    private const char KeyValueSeparator = '$';

    ////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////// ADD ////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public static void AddToAttr(ref string attr, string key, string value)
    {
        attr = $"{attr}{key}{KeyValueSeparator}{value}{PairSeparator}";
    }

    public static void AddToAttr(ref string attr, string key, int value)
    {
        attr = $"{attr}{key}{KeyValueSeparator}{value.ToString()}{PairSeparator}";
    }

    public static void AddToAttr(ref string attr, string key, double value)
    {
        attr = $"{attr}{key}{KeyValueSeparator}{value.ToString(CultureInfo.InvariantCulture)}{PairSeparator}";
        attr = $"{attr}{key}{KeyValueSeparator}{value.ToString(CultureInfo.InvariantCulture)}{PairSeparator}";
    }

    public static void AddToAttr(ref string attr, string key, bool value)
    {
        attr = $"{attr}{key}{KeyValueSeparator}{value.ToString()}{PairSeparator}";
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
        attr = attr.Substring(0, attr.Length - 1);
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