using My.DataStructures.List;

namespace Entities
{
    class Nehnutelnost : IEquatable<Nehnutelnost>
    {
        private int parcelNum;
        private string description;
        private ExplicitList<Parcela> parcely;
        private Position topLeft, bottomRigth;
        public Nehnutelnost(int pParcelNum, string pDescription, Position pTopLeft, Position pBottomRight)
        {
            this.parcelNum = parcelNum;
            this.description = pDescription;
            this.topLeft = pTopLeft;
            this.bottomRigth = pBottomRight;
            this.parcely = new ExplicitList<Parcela>();
        }

        public bool Equals(Nehnutelnost? other)
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

