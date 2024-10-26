namespace My.CoreGui;

public class GUI
{
    private Application _application;

    public GUI(Application application)
    {
        _application = application;
    }

    public int ChooseFromList(List<string?> options)
    {
        for (var i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"[i]: {options[i]}");
        }

        Console.WriteLine("Choose: ");
        return Int32.Parse(Console.ReadLine() ?? string.Empty);
    }
}