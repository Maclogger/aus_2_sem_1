namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

public interface IKey
{
    int CompareTo(IKey pOther, int pDimension);
    bool Equals(IKey pOther);
}
