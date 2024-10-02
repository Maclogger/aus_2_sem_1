
namespace Lists
{
    class ImplicitList<T> : List<T> where T : IEquatable<T>
    {
        private int capacity;
        private T[] array;
        private Func<int, int> enlargingStrategy;

        public ImplicitList(int pInitialCapacity = 10, Func<int, int>? pEnlargingStrategy = null)
        {
            this.capacity = pInitialCapacity;
            this.array = new T[this.capacity];

            // setting up default enlargingStrategy
            if (enlargingStrategy == null)
            {
                this.enlargingStrategy = (int pCurrentCapacity) =>
                {
                    return 2 * pCurrentCapacity; // default enlargingStrategy
                };
            }
            else
            {
                this.enlargingStrategy = pEnlargingStrategy!;
            }
        }

        public override int? Find(T pItem)
        {
            for (int i = 0; i < this.size; i++)
            {
                if (this.array[i].Equals(pItem))
                {
                    return i;
                }
            }
            return null;
        }

        public override void Add(T pItem)
        {
            if (this.size >= this.capacity)
            {
                // case => the capacity run out => have to create new bigger array => enlarge using enlargingStrategy
                int newCapacity = this.enlargingStrategy(this.capacity);
                if (newCapacity <= this.capacity)
                {
                    throw new Exception("Enlarging strategy has to output more than the input capacity.");
                }
                this.Enlarge(newCapacity);
            }

            this.array[this.size] = pItem;
            this.size++;
        }

        public override void Remove(T pItem)
        {
            int? itemIndex = this.Find(pItem);
            if (itemIndex == null)
            {
                return;
            }
            this.Remove((int)itemIndex);
        }

        public override T? Remove(int pIndex)
        {
            // It removes the item at index => moves all items at right to 1 position left
            if (pIndex < 0 || pIndex >= this.size)
            {
                return default(T?);
            }
            T temp = this.array[pIndex];
            for (int i = pIndex + 1; i < this.size; i++)
            {
                this.array[i - 1] = this.array[i];
            }
            this.size--;
            return temp;
        }

        public override T? Get(int pIndex)
        {
            if (pIndex < 0 || pIndex >= this.size)
            {
                return default(T?);
            }
            return this.array[pIndex];
        }

        public override void Clear()
        {
            this.size = 0;
        }

        public override void Set(int pIndex, T pValue)
        {
            if (pIndex < 0 || pIndex >= this.size)
            {
                return;
            }
            this.array[pIndex] = pValue;
        }

        public void Enlarge(int pNewCapacity)
        {
            T[] newArray = new T[pNewCapacity];
            for (int i = 0; i < this.size; i++)
            {
                newArray[i] = this.array[i];
            }
            this.array = newArray;
        }

        public void PrintOut()
        {
            if (this.size <= 0)
            {
                Console.WriteLine("The implicit list is empty.");
                return;
            }
            string output = "ImplicitList = [";
            for (int i = 0; i < this.size; i++)
            {
                output += this.array[i];
                if (i != this.size - 1)
                {
                    output += ", ";
                }
            }
            output += "]";
            Console.WriteLine(output);
        }

    }
}
