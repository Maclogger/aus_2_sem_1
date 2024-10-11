namespace My.DataStructures.List
{
    abstract class List<T>
    {
        protected int size { get; set; } = 0;
        public int Size => this.size;
        public abstract void Add(T pItem);
        public abstract void Remove(T pItem);
        public abstract T? Remove(int pIndex);
        public abstract T? Get(int pIndex);
        public abstract int? Find(T pItem);
        public abstract void Clear();
        public abstract void Set(int pIndex, T pValue);
        public bool IsEmpty()
        {
            return this.size <= 0;
        }
    }
}
