using My.DataStructures.KdTree;

namespace My.Core;

public class ApplicationCore
{
    private KdTree<Position, Realestate> _realestatesTree = new(2);
    private KdTree<Position, Parcel> _parcelasTree = new(2);

    public ApplicationCore()
    {

    }

    public void AddElement(int x, int y)
    {
        
    }




}