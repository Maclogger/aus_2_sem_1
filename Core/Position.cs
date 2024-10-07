using My.DataStructures;

namespace Entities
{
    public class Position : IEquatable<Position>
    {
        private char latitudeSign { get; set; } // N or S
        private double latitude { get; set; }
        private char longitudeSign { get; set; } // W or E
        private double longitude { get; set; }

        public Position(double pLatitude, double pLongitude)
        {
            ValidateCoordinates(pLatitude, pLongitude);
            CreateInstance(pLatitude, pLongitude);
        }

        public Position()
        {
            // Generate random coordinates within valid range
            double pLatitude = Utils.GetRandomDoubleInRange(-90.0, 90.0);
            double pLongitude = Utils.GetRandomDoubleInRange(-180.0, 180.0);
            ValidateCoordinates(pLatitude, pLongitude);
            CreateInstance(pLatitude, pLongitude);
        }

        public Position(double pMinLatitude, double pMaxLatitude, double pMinLongitude, double pMaxLongitude, bool pOnlyIntegers = false)
        {
            // Generate random coordinates within a given range
            double pLatitude;
            double pLongitude;

            if (pOnlyIntegers)
            {
                pLatitude = Utils.GetRandomIntInRange((int)pMinLatitude, (int)pMaxLatitude);
                pLongitude = Utils.GetRandomIntInRange((int)pMinLongitude, (int)pMaxLongitude);
            }
            else
            {
                pLatitude = Utils.GetRandomDoubleInRange((int)pMinLatitude, (int)pMaxLatitude);
                pLongitude = Utils.GetRandomDoubleInRange((int)pMinLongitude, (int)pMaxLongitude);
            }

            CreateInstance(pLatitude, pLongitude);
        }


        private void CreateInstance(double pLatitude, double pLongitude)
        {
            this.latitude = pLatitude;
            this.longitude = pLongitude;
            this.latitudeSign = this.latitude >= 0 ? 'N' : 'S';
            this.longitudeSign = this.longitude >= 0 ? 'E' : 'W';
        }

        private void ValidateCoordinates(double pLatitude, double pLongitude)
        {
            if (pLatitude < -90 || pLatitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(pLatitude), "Latitude must be between -90 and 90.");
            }

            if (pLongitude < -180 || pLongitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(pLongitude), "Longitude must be between -180 and 180.");
            }
        }

        public double GetX()
        {
            return this.longitude;
        }

        public double GetY()
        {
            return this.latitude;
        }

        public bool Equals(Position? other)
        {
            if (other == null)
            {
                return false;
            }

            return this.latitude == other.latitude && this.longitude == other.longitude && this.latitudeSign == other.latitudeSign && this.longitudeSign == other.longitudeSign;
        }

        public override string ToString()
        {
            return $"{this.longitudeSign} {Math.Abs(this.longitude)} and {this.latitudeSign} {Math.Abs(this.latitude)}]";
        }
    }
}
