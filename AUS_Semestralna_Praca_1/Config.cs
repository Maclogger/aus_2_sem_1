using System;

namespace AUS_Semestralna_Praca_1;

public class Config
{
    private static readonly Config _instance = new();

    // double TOLERANCE
    private double _tolerance = 0.00001;

    // random generated coordination:
    private double _minLatitude = -90.0;
    private double _maxLatitude = 90.0;
    private double _minLongitude = -180.0;
    private double _maxLongitude = 180.0;

    // testing KdTree structure:
    private double _probOfAdd = 0.25;
    private double _probOfFind = 0.25;
    private double _probOfRemove = 0.25;
    private double _probOfUpdate = 0.25;

    // printing format
    private bool _formattedOutput = true;

    private double _probOfAddingExistingElement = 0.9;
    private bool _shoudPrint = true;

    private int _seed = 1;
    private int _seedCount = 100;
    private int _operationCount = 100;
    private int _elementCountBeforeTest = 0;

    private Config()
    {
    }

    public static Config Instance => _instance;

    public bool ProbabilityCheck()
    {
        return Math.Abs(_probOfAdd + _probOfFind + _probOfRemove + _probOfUpdate - 1) < Tolerance;
    }

    public double MinLatitude
    {
        get => _minLatitude;
        set => _minLatitude = value;
    }

    public double MaxLatitude
    {
        get => _maxLatitude;
        set => _maxLatitude = value;
    }


    public double MaxLatitudeAbs
    {
        get => Math.Max(Math.Abs(_maxLatitude), Math.Abs(_minLatitude));
    }

    public double MaxLongitudeAbs
    {
        get => Math.Max(Math.Abs(_maxLongitude), Math.Abs(_minLongitude));
    }

    public double MinLongitude
    {
        get => _minLongitude;
        set => _minLongitude = value;
    }

    public double MaxLongitude
    {
        get => _maxLongitude;
        set => _maxLongitude = value;
    }

    public double ProbOfAdd
    {
        get => _probOfAdd;
        set => _probOfAdd = value;
    }

    public double ProbOfFind
    {
        get => _probOfFind;
        set => _probOfFind = value;
    }

    public double ProbOfRemove
    {
        get => _probOfRemove;
        set => _probOfRemove = value;
    }

    public double ProbOfUpdate
    {
        get => _probOfUpdate;
        set => _probOfUpdate = value;
    }

    public bool FormattedOutput
    {
        get => _formattedOutput;
        set => _formattedOutput = value;
    }

    public double Tolerance
    {
        get => _tolerance;
        set => _tolerance = value;
    }

    public double MinLatitudeAbs
    {
        get
        {
            double min = Math.Min(_maxLatitude, _minLatitude);
            return min < 0 ? 0.0 : min;
        }
    }

    public double MinLongitudeAbs
    {
        get
        {
            double min = Math.Min(_maxLongitude, _minLongitude);
            return min < 0 ? 0.0 : min;
        }
    }

    public double ProbOfAddingExistingElement
    {
        get => _probOfAddingExistingElement;
        set => _probOfAddingExistingElement = value;
    }

    public bool ShoudPrint
    {
        get => _shoudPrint;
        set => _shoudPrint = value;
    }

    public int Seed
    {
        get => _seed;
        set => _seed = value;
    }

    public int SeedCount
    {
        get => _seedCount;
        set => _seedCount = value;
    }

    public int OperationCount
    {
        get => _operationCount;
        set => _operationCount = value;
    }

    public int ElementCountBeforeTest
    {
        get => _elementCountBeforeTest;
        set => _elementCountBeforeTest = value;
    }
}