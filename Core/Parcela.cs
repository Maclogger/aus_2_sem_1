using My.DataStructures.List;

namespace Entities
{
    public class Parcela : IEquatable<Parcela>
    {
        private int parcelNum;
        private string description;
        private ExplicitList<Nehnutelnost> nehnutelnosti;
        private Position pos1;
        private Position pos2;

        public Position Pos1
        {
            get => pos1;
            set => pos1 = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Position Pos2
        {
            get => pos2;
            set => pos2 = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Parcela(int parcelNum, string description, Position pos1, Position pos2)
        {
            this.parcelNum = parcelNum;
            this.description = description;
            this.pos1 = pos1;
            this.pos2 = pos2;
            this.nehnutelnosti = new ExplicitList<Nehnutelnost>();
        }

        public bool Equals(Parcela? other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.parcelNum == other.parcelNum && this.pos2 == other.pos2 &&
                this.description == other.description && this.pos1 == other.pos1) ;
        }
    }
}

