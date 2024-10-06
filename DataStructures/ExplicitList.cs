namespace Lists
{
    class Node<T> where T : IEquatable<T>
    {
        private T dataPart { get; set; }
        private Node<T>? next { get; set; }

        public Node(T pDataPart)
        {
            this.dataPart = pDataPart;
        }

        public T DataPart
        {
            get => this.dataPart;
            set => this.dataPart = value;
        }

        public Node<T>? Next
        {
            get => this.next;
            set => this.next = value;
        }
    }

    class ExplicitList<T> : List<T> where T : IEquatable<T>
    {
        Node<T>? firstNode;
        Node<T>? lastNode;

        public ExplicitList()
        {
        }

        public override int? Find(T pItem)
        {
            Node<T>? currentNode = this.firstNode;
            int index = 0;
            while (currentNode != null)
            {
                if (currentNode.DataPart.Equals(pItem))
                {
                    return index;
                }
                currentNode = currentNode.Next;
                index++;
            }
            return null;
        }

        public override void Add(T pItem)
        {
            if (this.IsEmpty())
            {
                this.firstNode = new Node<T>(pItem);
                this.lastNode = this.firstNode;
                this.size++;
                return;
            }
            this.lastNode!.Next = new Node<T>(pItem);
            this.lastNode = this.lastNode!.Next;
            this.size++;
        }

        public override void Remove(T pItem)
        {
            if (this.IsEmpty())
            {
                return;
            }

            // if firstNode is the one we want to delete
            if (this.firstNode!.DataPart.Equals(pItem))
            {
                this.firstNode = this.firstNode!.Next;
                this.size--;
                return;
            }

            // if there is only 1 element and the one we want to delete is not the first one
            if (this.size <= 1)
            {
                return;
            }

            // it is guaranteed that there are at least 2 elements in list
            Node<T>? previousNode = this.firstNode;
            Node<T>? currentNode = this.firstNode!.Next;

            while (currentNode != null)
            {
                if (currentNode.DataPart.Equals(pItem))
                {
                    previousNode.Next = currentNode.Next;
                    this.size--;
                    return;
                }
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }
        }

        public override T? Remove(int pIndex)
        {
            if (pIndex < 0 || pIndex >= this.size)
            {
                return default(T?);
            }

            if (pIndex == 0)
            {
                T dataPart = this.firstNode!.DataPart;
                this.firstNode = this.firstNode!.Next;
                this.size--;
                return dataPart;
            }

            Node<T>? previousNode = this.firstNode;
            Node<T>? currentNode = this.firstNode!.Next;
            for (int i = 0; i < pIndex - 1; i++)
            {
                previousNode = currentNode;
                currentNode = currentNode!.Next;
            }
            T? temp = currentNode!.DataPart;
            previousNode!.Next = currentNode!.Next;
            this.size--;
            return temp;
        }

        public override T? Get(int pIndex)
        {
            if (pIndex < 0 || pIndex >= this.size)
            {
                return default(T?);
            }

            Node<T>? currentNode = this.firstNode!;
            for (int i = 0; i < pIndex; i++)
            {
                currentNode = currentNode!.Next;
            }

            return currentNode!.DataPart;
        }

        public override void Clear()
        {
            this.firstNode = null;
            this.lastNode = null;
            this.size = 0;
        }

        public override void Set(int pIndex, T pValue)
        {
            throw new Exception("Not implemented yet!");
        }

        public void PrintOut()
        {
            if (this.size <= 0)
            {
                Console.WriteLine("The explicit list is empty.");
                return;
            }

            string output = "ExplicitList = [";

            Node<T>? currentNode = this.firstNode;
            while (currentNode != null)
            {
                output += currentNode!.DataPart;
                if (currentNode.Next != null)
                {
                    output += "\n";
                }
                currentNode = currentNode.Next;
            }
            output += "]";
            Console.WriteLine(output);
        }

    }
}
