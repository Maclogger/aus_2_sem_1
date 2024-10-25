namespace My.Core
{
    public class Parcel
    {
        private int _parcelNum; // číslo parcely
        private string _description; // popis
        private List<Realestate> _realestates = new(); // nehnuteľnosti
        private Position _topLeft, _bottomRight;

        public Parcel(int pParcelNum, string pDescription, Position pTopLeft, Position pBottomRight)
        {
            ParcelNum = pParcelNum;
            Description = pDescription;
            TopLeft = pTopLeft;
            BottomRight = pBottomRight;
        }

        public int ParcelNum
        {
            get => _parcelNum;
            set => _parcelNum = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public List<Realestate> Realestates
        {
            get => _realestates;
            set => _realestates = value;
        }

        public Position TopLeft
        {
            get => _topLeft;
            set => _topLeft = value;
        }

        public Position BottomRight
        {
            get => _bottomRight;
            set => _bottomRight = value;
        }
    }
}

