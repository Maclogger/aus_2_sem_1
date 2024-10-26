namespace My.DataStructures.KdTree;

public interface IKey
{
    int CompareTo(IKey pOther, int pDimension);
    bool Equals(IKey pOther);
}
