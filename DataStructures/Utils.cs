namespace My.DataStructures;

public class Utils
{
    private static readonly Random RandomInstance = new Random();

    public static double GetRandomDoubleInRange(double pMin, double pMax)
    {
        return pMin + (pMax - pMin) * (RandomInstance.NextDouble());
    }

    public static int GetRandomIntInRange(int pMin, int pMax)
    {
        return pMin + (pMax - pMin) * (RandomInstance.Next());
    }

    public static long GetRandomLongInRange(long pMin, long pMax)
    {
        return pMin + (pMax - pMin) * (RandomInstance.NextInt64());
    }
}