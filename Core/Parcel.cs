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
            _parcelNum = pParcelNum;
            _description = pDescription;
            _topLeft = pTopLeft;
            _bottomRight = pBottomRight;
        }
    }
}

