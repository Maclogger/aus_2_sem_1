namespace My.DataStructures;

public class Utils
{
    private static readonly Random RandomInstance = new Random();
    private static int Uid = 0;

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

    public static int GetNextVal()
    {
        int currentUid = Uid;
        Uid++;
        return currentUid;
    }
}
