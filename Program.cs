namespace Semestralka
{
    class Program
    {
        static void Main(string[] args)
        {
            Lists.ImplicitList<int> zoznam = new Lists.ImplicitList<int>(3);
            zoznam.PrintOut();
            zoznam.Add(14);
            zoznam.Add(18);
            zoznam.Add(14);
            zoznam.PrintOut();
            zoznam.Add(-4);
            zoznam.PrintOut();
            zoznam.Add(21);
            zoznam.PrintOut();
            zoznam.Remove(0);
            zoznam.PrintOut();
            zoznam.Remove(3);
            zoznam.PrintOut();
            zoznam.Add(-4);
            zoznam.PrintOut();
            Console.WriteLine(zoznam.Get(3));
            zoznam.Set(2, 10);
            zoznam.PrintOut();
            zoznam.Clear();
            zoznam.PrintOut();
        }
    }
}

