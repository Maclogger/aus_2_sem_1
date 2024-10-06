using Entities;

namespace Trees
{

    class Node
    {
        private GpsPosition gpsPosition;

        public Node(GpsPosition pGpsPosition)
        {
            gpsPosition = pGpsPosition;
        }
    }

    class KDTree
    {
        Node? root;
        public KDTree()
        {

        }

        public void randomFillUp(int pCountOfNodes)
        {

        }
    }

}
