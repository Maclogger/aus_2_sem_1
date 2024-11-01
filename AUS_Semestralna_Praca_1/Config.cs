using System;

namespace AUS_Semestralna_Praca_1;

public class Config
{
    private static readonly Config _instance = new();
    public static Config Instance => _instance;

    // double TOLERANCE
    public double Tolerance { get; set; } = 0.00001;

    // random generated coordination:
    public double MinLatitude { get; set; } = -90.0;
    public double MaxLatitude { get; set; } = 90.0;
    public double MinLongitude { get; set; } = -180.0;
    public double MaxLongitude { get; set; } = 180.0;

    // testing KdTree structure:
    public double ProbOfAdd { get; set; } = 0.25;
    public double ProbOfFind { get; set; } = 1.25;
    public double ProbOfRemove { get; set; } = 0.25;
    public double ProbOfUpdate { get; set; } = 0.25;

    public bool FormattedOutput { get; set; } = true;

    public double ProbOfDuplicate { get; set; } = 0.9;
    public bool ShoudPrint { get; set; } = true;

    public int Seed { get; set; } = 1;
    public int SeedCount { get; set; } = 100;
    public int OperationCount { get; set; } = 100;
    public int ElementCountBeforeTest { get; set; } = 0;
    public int CheckAfterOperationCount { get; set; } = 100;
    public bool UidPrint { get; set; } = true; // TODO change to false

    private Config()
    {
    }


    public double MinLatitudeAbs
    {
        get
        {
            double min = Math.Min(MaxLatitude, MinLatitude);
            return min < 0 ? 0.0 : min;
        }
    }

    public double MinLongitudeAbs
    {
        get
        {
            double min = Math.Min(MaxLongitude, MinLongitude);
            return min < 0 ? 0.0 : min;
        }
    }

    public double MaxLatitudeAbs
    {
        get => Math.Max(Math.Abs(MaxLatitude), Math.Abs(MinLatitude));
    }

    public double MaxLongitudeAbs
    {
        get => Math.Max(Math.Abs(MaxLongitude), Math.Abs(MinLongitude));
    }

}