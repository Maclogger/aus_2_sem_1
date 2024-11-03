using System;

namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures;

public class Utils
{
    private static readonly Random RandomInstance = new Random();
    private static int Uid = 0;
    private static int Index = 0;
    private static int StringUid = 0;
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    // Inclusive min, exclusive max
    public static double GetRandomDoubleInRange(double pMin, double pMax, Random? pRand = null)
    {
        Random random = pRand ?? RandomInstance;
        return pMin + (pMax - pMin) * random.NextDouble();
    }

    // Inclusive min, exclusive max
    public static int GetRandomIntInRange(int pMin, int pMax, Random? pRand = null)
    {
        Random random = pRand ?? RandomInstance;
        return random.Next(pMin, pMax);
    }

    // Inclusive min, exclusive max
    public static long GetRandomLongInRange(long pMin, long pMax, Random? pRand = null)
    {
        Random random = pRand ?? RandomInstance;
        return random.NextInt64(pMin, pMax);
    }

    public static string GetNextStringValOfLentgth(int length)
    {
        int uniqueNumber = GetNextStringVal();
        return EncodeNumberToBase(uniqueNumber, length);
    }

    private static string EncodeNumberToBase(int number, int length)
    {
        char[] result = new char[length];
        int baseSize = Chars.Length;

        for (int i = length - 1; i >= 0; i--)
        {
            result[i] = Chars[number % baseSize];
            number /= baseSize;
        }

        return new string(result);
    }

    public static int GetNextVal()
    {
        int currentUid = Uid;
        Uid++;
        return currentUid;
    }
    
    public static int GetNextIndex()
    {
        int currentIndex = Index;
        Index++;
        return currentIndex;
    }

    public static int GetNextStringVal()
    {
        int currentUid = StringUid;
        StringUid++;
        return currentUid;
    }
}