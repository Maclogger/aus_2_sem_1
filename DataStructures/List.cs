namespace Lists
{
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
