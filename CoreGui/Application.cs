using My.Core;

namespace My.CoreGui;

public class Application
{
    private ApplicationCore _core;
    private GUI _gui;

    public Application()
    {
        _core = new ApplicationCore(this);
        _gui = new GUI(this); // TODO add GUI
    }

    public ApplicationCore? Core
    {
        get => _core;
        set => _core = value;
    }


    // GUI => Core
    public void AddParcel(string[] data)
    {
        

    }


    public void Run()
    {
        throw new NotImplementedException();
    }

    public int AskUserToChooseFromList(List<object> list)
    {
        string?[] options = new string?[list.Count];

        for (var i = 0; i < list.Count; i++)
        {
            options[i] = list[i].ToString();
        }

        return _gui.ChooseFromList(list); // TODO implement GUI
    }
}