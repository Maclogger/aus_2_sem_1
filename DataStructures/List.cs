namespace Lists
{
    class Test
    {
        public static void ImplicitListTest()
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

    abstract class List<T>
    {
        protected int size { get; set; } = 0;
        public abstract void Add(T pItem);
        public abstract void Remove(T pItem);
        public abstract T? Remove(int pIndex);
        public abstract T? Get(int pIndex);
        public abstract int? Find(T pItem);
        public abstract void Clear();
        public abstract void Set(int pIndex, T pValue);
    }
}
