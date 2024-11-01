using System.Collections.Generic;
using System.Collections.ObjectModel;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using Avalonia.Controls;

namespace AUS_Semestralna_Praca_1.FrontEnd.Realestates;

public class RealestateData
{
    public int RealestateNum { get; set; }
    public string Description { get; set; }
    public int Uid1 { get; set; }
    public int Uid2 { get; set; }
    public string Pos1 { get; set; }
    public string Pos2 { get; set; }

    public RealestateData(int realestateNum, string description, int uid1, int uid2, string pos1, string pos2)
    {
        RealestateNum = realestateNum;
        Description = description;
        Uid1 = uid1;
        Uid2 = uid2;
        Pos1 = pos1;
        Pos2 = pos2;
    }
}

public partial class RealestatesListScreen : UserControl
{
    public ObservableCollection<RealestateData> ListRealestates { get; set; }

    public RealestatesListScreen(List<string> listOfRealestates)
    {
        InitializeComponent();

        List<RealestateData> list = new(listOfRealestates.Count);
        foreach (string attr in listOfRealestates)
        {
            int pRealestateNum = (int)ClientSys.GetIntFromAttr(attr, "NUM")!;
            string pDescription = ClientSys.GetStringFromAttr(attr, "DESCRIPTION")!;
            int pUid1 = (int)ClientSys.GetIntFromAttr(attr, "UID_1")!;
            int pUid2 = (int)ClientSys.GetIntFromAttr(attr, "UID_2")!;
            string pPos1 = ClientSys.GetStringFromAttr(attr, "POS_1")!;
            string pPos2 = ClientSys.GetStringFromAttr(attr, "POS_2")!;

            list.Add(new RealestateData(pRealestateNum, pDescription, pUid1, pUid2, pPos1, pPos2));
        }

        ListRealestates = new ObservableCollection<RealestateData>(list);
        DataContext = this;
    }

}
