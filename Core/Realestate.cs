using Entities;

namespace My.Core
{
    public class Realestate
    {
        private int _realestateNum; // súpisné číslo
        private string _description; // popis
        private List<Parcel> _parcelas = new(); // parcely
        private Position _topLeft, _bottomRight;

        public Realestate(int pRealestateNum, string pDescription, Position pTopLeft, Position pBottomRight)
        {
            _realestateNum = pRealestateNum;
            _description = pDescription;
            _topLeft = pTopLeft;
            _bottomRight = pBottomRight;
        }
    }
}
