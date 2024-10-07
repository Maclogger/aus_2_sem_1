
namespace Entities;

public class Setup
{
    public static List<Parcela> generateRandomParcelas(int pCount)
    {
        List<Parcela> listOfParcela = new List<Parcela>();
        for (int i = 0; i < pCount; i++)
        {
            Position position1 = new Position(0, 10, 0, 10, true);
            Position position2 = new Position(0, 10, 0, 10, true);
            Parcela parcela = new Parcela(i, $"parcela cislo {i}", position1, position2);
            listOfParcela.Add(parcela);
        }

        return listOfParcela;
    }
}