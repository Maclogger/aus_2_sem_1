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
    private int? Uid { get; set; }
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

        double[] cords = { X, Y };
        double[] cordsOther = { pOtherPos.X, pOtherPos.Y };


        if (Math.Abs(cords[pDimension] - cordsOther[pDimension]) < Config.Instance.Tolerance)
        {
            return 0;
        }

        if (cords[pDimension] < cordsOther[pDimension])
        {
            return -1;
        }

        return 1;
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
        if (Config.Instance.FormattedOutput)
        {
            return $"( {Math.Abs(Latitude)} {LatitudeSign} ; {Math.Abs(Longitude)} {LongitudeSign})";
        }

        return $"( {X} ; {Y} )";
    }

    public void SetUidNull()
    {
        Uid = null;
    }
}