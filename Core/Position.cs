using Entities;
using My.DataStructures;
using My.DataStructures.KdTree;

namespace My.Core
{
    public class Position : IKey
    {
        private char _latitudeSign; // N or S
        private double _latitude;
        private char _longitudeSign; // W or E
        private double _longitude;
        private double _x;
        private double _y;

        public Position(double pLatitude, char pLatitudeSign, double pLongitude, char pLongitudeSign)
        {
            Initialize(pLatitude, pLatitudeSign, pLongitude, pLongitudeSign);
        }

        public Position(string attr)
        {
            char latitude = (char)ClientSys.GetStringFromAttr(attr, "LAT")?[0]!;
            char latitudeSign = (char)ClientSys.GetStringFromAttr(attr, "LAT_SIGN")?[0]!;
            double longitude = (double)ClientSys.GetDoubleFromAttr(attr, "LON")!;
            char longitudeSign = (char)ClientSys.GetStringFromAttr(attr, "LON_SIGN")?[0]!;

            Initialize(latitude, latitudeSign, longitude, longitudeSign);
        }

        private void Initialize(double pLatitude, char pLatitudeSign, double pLongitude, char pLongitudeSign)
        {
            if (pLatitudeSign == 'S')
            {
                X = -pLatitude;
            }

            if (pLongitudeSign == 'E')
            {
                Y = -pLongitude;
            }

            Latitude = pLatitude;
            LatitudeSign = pLatitudeSign;
            Longitude = pLongitude;
            LongitudeSign = pLongitudeSign;
        }

        public Position(Random? pRandom = null)
        {
            Random random = pRandom ?? new Random();

            // Generate random coordinates within valid range
            double pLatitude = Utils.GetRandomDoubleInRange(Config.Instance.MinLatitude, Config.Instance.MaxLatitude);
            double pLongitude = Utils.GetRandomDoubleInRange(Config.Instance.MinLongitude, Config.Instance.MaxLongitude);
        }

        public double X
        {
            get => _x;
            set => _x = value;
        }

        public double Y
        {
            get => _y;
            set => _y = value;
        }

        public char LatitudeSign
        {
            get => _latitudeSign;
            set => _latitudeSign = value;
        }

        public double Latitude
        {
            get => _latitude;
            set => _latitude = value;
        }

        public char LongitudeSign
        {
            get => _longitudeSign;
            set => _longitudeSign = value;
        }

        public double Longitude
        {
            get => _longitude;
            set => _longitude = value;
        }

        public override string ToString()
        {
            if (Config.Instance.FormattedOutput)
            {
                return $"( {LatitudeSign}{Latitude} ; {LongitudeSign}{Longitude} )";
            }
            return $"( {X} ; {Y} )";
        }

        public int CompareTo(IKey pOther, int pDimension)
        {
            if (pOther is not Position pOtherPos)
            {
                throw new ArgumentException($"IKey in comparator was not an instance of Position: {nameof(pOther)}");
            }

            double[] cords = { X, Y };
            double[] cordsOther = { pOtherPos.X, pOtherPos.Y };


            if (Math.Abs(cords[pDimension] - cordsOther[pDimension]) < Config.Instance.DoubleTolerance)
            {
                return 0;
            }
            
            if (cords[pDimension] < cordsOther[pDimension])
            {
                return -1;
            }

            return 1;
        }
    }
}
