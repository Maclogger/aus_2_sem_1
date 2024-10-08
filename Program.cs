using Entities;
using My.DataStructures;

namespace My
{
    class MyIntKey : IKey
    {
        private int _value;

        public MyIntKey(int value)
        {
            _value = value;
        }
        
        public int CompareTo(IKey other, int dimension)
        {
            if (other is not MyIntKey myIntKey)
            {
                throw new ArgumentException("Object is not an IntItem");
            }

            if (_value == myIntKey._value)
            {
                return 0;
            }

            return _value < myIntKey._value ? -1 : 1;
        }
    }
    
    class Program
    {
        public static void Main(string[] args)
        {
            /*
            setUpRandom2DTree();
            setUpiPadTestCase();
            */
            KdTree<MyIntKey, int> tree = new(1);

            for (int i = 0; i < 1000; i++)
            {
                tree.Add(new MyIntKey(i), i);
            }

            foreach (int item in tree)
            {
                Console.WriteLine(item);
            }
        }

        /*
        private static void setUpiPadTestCase()
        {
            List<Position> positions = new List<Position>();
            positions.Add(new Position(0, 0)); // 0
            positions.Add(new Position(0, 6)); // 1
            positions.Add(new Position(1, 2)); // 2
            positions.Add(new Position(1, 4)); // 3
            positions.Add(new Position(2, 3)); // 4
            positions.Add(new Position(3, 1)); // 5
            positions.Add(new Position(4, 3)); // 6 -- allowing for duplicity at this point
            positions.Add(new Position(4, 3)); // 7 -- allowing for duplicity at this point
            positions.Add(new Position(5, 1)); // 8
            positions.Add(new Position(5, 5)); // 9
            List<Parcela> parcelas = new List<Parcela>();
            parcelas.Add(new Parcela(0, "Parcela 0", positions[0], positions[2]));
            parcelas.Add(new Parcela(1, "Parcela 1", positions[1], positions[3]));
            parcelas.Add(new Parcela(2, "Parcela 2", positions[4], positions[5]));
            parcelas.Add(new Parcela(3, "Parcela 3", positions[6], positions[8]));
            parcelas.Add(new Parcela(4, "Parcela 4", positions[7], positions[9]));
        }

        private static void setUpRandom2DTree()
        {
            List<Parcela> randomParcelas = Setup.generateRandomParcelas(20);
            List<Node> nodes = [];
            foreach (Parcela randomParcela in randomParcelas)
            {
                Node temp = new Node(randomParcela.Pos1);
                nodes.Add(temp);
                temp = new Node(randomParcela.Pos2);
                nodes.Add(temp);
            }
        }
        */

    }
}

