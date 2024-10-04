namespace Semestralka
{
    class Program
    {

        static void Main(string[] args)
        {
            //Trees.KDTree tree = new Trees.KDTree();
            Random rand = new Random();
            Lists.ImplicitList<Entities.GpsPosition> loc = new Lists.ImplicitList<Entities.GpsPosition>();
            for (int i = 0; i < 10; i++)
            {
                loc.Add(new Entities.GpsPosition(rand));
            }

            loc.PrintOut();
        }
    }
}

