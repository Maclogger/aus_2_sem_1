namespace Entities
{
    public class GpsPosition : IEquatable<GpsPosition>
    {
        private char latitudeSign { get; set; } // N or S
        private double latitude { get; set; }
        private char longitudeSign { get; set; } // W or E
        private double longitude { get; set; }

        public GpsPosition(double pLatitude, double pLongitude)
        {
            ValidateCoordinates(pLatitude, pLongitude);
            CreateInstance(pLatitude, pLongitude);
        }

        public GpsPosition(Random rnd)
        {
            // Generate random coordinates within valid range
            double pLongitude = (rnd.NextDouble() - 0.5) * 360;
            double pLatitude = (rnd.NextDouble() - 0.5) * 180;
            ValidateCoordinates(pLatitude, pLongitude);
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

        public override bool Equals(object? obj)
        {
            if (obj is GpsPosition other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(GpsPosition? other)
        {
            if (other == null)
            {
                return false;
            }

            return this.latitude == other.latitude && this.longitude == other.longitude && this.latitudeSign == other.latitudeSign && this.longitudeSign == other.longitudeSign;
        }

        // Override object.GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(latitude, longitude, latitudeSign, longitudeSign);
        }

        public override string ToString()
        {
            return $"{this.longitudeSign} {Math.Abs(this.longitude)} and {this.latitudeSign} {Math.Abs(this.latitude)}]";
        }
    }
}
