using My.Core;
using My.DataStructures;
using My.DataStructures.KdTree;

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
    public Answer AddParcel(string pos1Attr, string pos2Attr, string parcelAttr)
    {
        int? parcelNum = ClientSys.GetIntFromAttr(parcelAttr, "PARCEL_NUM");
        string description = ClientSys.GetStringFromAttr(parcelAttr, "DESCRIPTION")!;

        Position? pos1;
        Position? pos2;
        try
        {
            pos1 = new(pos1Attr); // it can throw inside
            pos2 = new(pos2Attr); // it can throw inside
            int dummy = (int)parcelNum!; // if parcelNum is not a number => crash
        }
        catch
        {
            return new Answer("Some of the attributes are missing or invalid.", AnswerState.Error);
        }

        Parcel parcel = new((int)parcelNum, description, pos1, pos2);

        return _core.AddParcel(pos1, pos2, parcel);
    }

    public Answer RemoveParcel(string attr)
    {

    }



    public void Run()
    {
        throw new NotImplementedException();
    }



    public int GetUidFromUserByChoosingFromList<T>(List<DataPart<T>> list)
    {
        List<string?> optionsForUser = new();
        foreach (DataPart<T> dataPart in list)
        {
            string option = dataPart.Value?.ToString() ?? "NULL";
            optionsForUser.Add(option);
        }
        
        int index = _gui.ChooseFromList(optionsForUser); // TODO implement GUI

        return list[index].Uid;
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