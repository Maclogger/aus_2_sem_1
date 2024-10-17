using My.DataStructures.KdTree;

namespace My;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(!false);
        /*
        setUpRandom2DTree();
        setUpiPadTestCase();


        KdTree<MyIntKey, int> tree = new(1);

        for (int i = 0; i < 1000; i++)
        {
            tree.Add(new MyIntKey(i), i);
        }



        foreach (int item in tree)
        {
            Console.WriteLine(item);
        }

        KdTree<Cord, string> tree = new(2);
        tree.Add(new Cord(23, 35), "Nitra");
        tree.Add(new Cord(20, 33), "Sereď");
        tree.Add(new Cord(25, 36), "Topoľčianky");
        tree.Add(new Cord(16, 31), "Galanta");
        tree.Add(new Cord(14, 39), "Senica");
        tree.Add(new Cord(28, 34), "Tlmače");
        tree.Add(new Cord(24, 40), "Bošany");
        tree.Add(new Cord(13, 32), "Bratislava");
        tree.Add(new Cord(12, 41), "Hodonín");
        tree.Add(new Cord(17, 42), "Trnava");
        tree.Add(new Cord(26, 35), "Moravce");
        tree.Add(new Cord(30, 33), "Levice");
        tree.Add(new Cord(29, 46), "Bojnice");
        tree.Add(new Cord(27, 43), "Nováky");
        Console.WriteLine("\nInOrder prehliadka pomocou iterátora:\n");
        foreach (string s in tree)
        {
            Console.WriteLine(s);
        }

        Cord hladanyCord = new Cord(23, 35);

        Console.WriteLine($"\n\nVyhľadanie prvku {hladanyCord}");
        foreach (string s in tree.Find(hladanyCord))
        {
            Console.WriteLine(s);
        }*/
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