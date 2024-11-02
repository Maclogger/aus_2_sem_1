using System;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class Position : IKey
{
    public char LatitudeSign; // N or S
    public double Latitude;
    public char LongitudeSign; // W or E
    public double Longitude;
    public int? Uid { get; set; }
    public double X { get; set; }

    public double Y { get; set; }


    public Position(double pLatitude, char pLatitudeSign, double pLongitude, char pLongitudeSign)
    {
        Initialize(pLatitude, pLatitudeSign, pLongitude, pLongitudeSign);
    }

    public Position(string attr)
    {
        double latitude = (double)ClientSys.GetDoubleFromAttr(attr, "LAT")!;
        char latitudeSign = (char)ClientSys.GetStringFromAttr(attr, "LAT_SIGN")?[0]!;
        double longitude = (double)ClientSys.GetDoubleFromAttr(attr, "LON")!;
        char longitudeSign = (char)ClientSys.GetStringFromAttr(attr, "LON_SIGN")?[0]!;

        Initialize(latitude, latitudeSign, longitude, longitudeSign);
    }

    public Position(Random random)
    {
        double latitude =
            Utils.GetRandomDoubleInRange(Config.Instance.MinLatitude, Config.Instance.MaxLatitude, random);
        char latitudeSign = random.NextDouble() < 0.5 ? 'N' : 'S';
        double longitude =
            Utils.GetRandomDoubleInRange(Config.Instance.MinLatitude, Config.Instance.MaxLatitude, random);
        char longitudeSign = random.NextDouble() < 0.5 ? 'E' : 'W';

        Initialize(latitude, latitudeSign, longitude, longitudeSign);
    }

    public Position(Position other)
    {
        Initialize(other.Latitude, other.LatitudeSign, other.Longitude, other.LongitudeSign);
    }

    private void Initialize(double pLatitude, char pLatitudeSign, double pLongitude, char pLongitudeSign)
    {
        X = pLatitudeSign == 'W' ? -pLatitude : pLatitude;

        Y = pLongitudeSign == 'S' ? -pLongitude : pLongitude;

        Latitude = pLatitude;
        LatitudeSign = pLatitudeSign;
        Longitude = pLongitude;
        LongitudeSign = pLongitudeSign;
        Uid = Utils.GetNextVal();
    }


    public int CompareTo(IKey pOther, int pDimension)
    {
        if (pOther is not Position pOtherPos)
        {
            throw new ArgumentException($"IKey in comparator was not an instance of Position: {nameof(pOther)}");
        }

        // Získaj hodnoty podľa dimenzie bez vytvárania polí
        double coord = pDimension == 0 ? X : Y;
        double coordOther = pDimension == 0 ? pOtherPos.X : pOtherPos.Y;

        if (Math.Abs(coord - coordOther) < Config.Instance.Tolerance)
        {
            return 0;
        }

        return coord < coordOther ? -1 : 1;
    }


    public bool Equals(IKey pOther)
    {
        if (pOther is not Position pOtherPos) return false;
        return Math.Abs(X - pOtherPos.X) < Config.Instance.Tolerance &&
               Math.Abs(Y - pOtherPos.Y) < Config.Instance.Tolerance &&
               Uid == pOtherPos.Uid && Uid != null;
    }

    public override string ToString()
    {
        return $"({Uid}: {X} ; {Y} )";
    }

    public void SetUidNull()
    {
        Uid = null;
    }


    public void AddToAttr(ref string sol, int postFix)
    {
        ClientSys.AddToAttr(ref sol, $"LAT_{postFix}", Latitude);
        ClientSys.AddToAttr(ref sol, $"LAT_SIGN_{postFix}", LatitudeSign.ToString());
        ClientSys.AddToAttr(ref sol, $"LON_{postFix}", Longitude);
        ClientSys.AddToAttr(ref sol, $"LON_SIGN_{postFix}", LongitudeSign.ToString());
        ClientSys.AddToAttr(ref sol, $"UID_{postFix}", Uid ?? -1);
    }

    public static string ToFormattedString(int uid, double latitude, string latitudeSign, double longitude,
        string longitudeSign)
    {
        if (Config.Instance.FormattedOutput)
        {
            return $"([{Math.Abs(latitude)}{latitudeSign}, {Math.Abs(longitude)}{longitudeSign}])";
        }

        return $"(UID:{uid}, [{Math.Abs(latitude)}{latitudeSign}, {Math.Abs(longitude)}{longitudeSign}])";
    }

    public static Position GetDeepCopy(Position realestatePos1)
    {
        return new(realestatePos1.Latitude, realestatePos1.LatitudeSign, realestatePos1.Longitude,
            realestatePos1.LongitudeSign);
    }
}