using Lists;

namespace Entities
{
    class Parcela : IEquatable<Parcela>
    {
        private int parcelNum;
        private string description;
        private ExplicitList<Nehnutelnost> nehnutelnosti;
        private GpsPosition topLeft;
        private GpsPosition bottomRigth;
        public Parcela(int parcelNum, string description, GpsPosition topLeft, GpsPosition bottomRigth)
        {
            this.parcelNum = parcelNum;
            this.description = description;
            this.topLeft = topLeft;
            this.bottomRigth = bottomRigth;
            this.nehnutelnosti = new ExplicitList<Nehnutelnost>();
        }

        public bool Equals(Parcela? other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.parcelNum == other.parcelNum && this.bottomRigth == other.bottomRigth &&
                this.description == other.description && this.topLeft == other.topLeft) ;
        }
    }
}

