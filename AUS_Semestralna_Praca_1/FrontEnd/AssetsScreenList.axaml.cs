using System.Collections.Generic;
using System.Collections.ObjectModel;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AUS_Semestralna_Praca_1.FrontEnd;

public class AssetData
{
    public string Type { get; set; }
    public string TypeClient { get; set; }
    public int Num { get; set; }
    public string Description { get; set; }
    public int Uid1 { get; set; }
    public int Uid2 { get; set; }
    public string Pos1 { get; set; }
    public string Pos2 { get; set; }

    public AssetData(int num, string description, int uid1, int uid2, string pos1, string pos2, string type)
    {
        Type = type;
        Num = num;
        Description = description;
        Uid1 = uid1;
        Uid2 = uid2;
        Pos1 = pos1;
        Pos2 = pos2;
        TypeClient = type == "RS" ? "Nehnuteľnosť" : "Parcela";
    }
}
public partial class AssetsScreenList : UserControl
{
    public ObservableCollection<AssetData> ListAssets { get; set; }
    public AssetsScreenList(List<string> listOfAssets)
    {
        InitializeComponent();

        List<AssetData> list = new(listOfAssets.Count);
        foreach (string attr in listOfAssets)
        {
            string type = ClientSys.GetStringFromAttr(attr, "TYPE")!;

            int num = (int)ClientSys.GetIntFromAttr(attr, type == "RS" ? "REALESTATE_NUM" : "PARCEL_NUM")!;
            string pDescription = ClientSys.GetStringFromAttr(attr, "DESCRIPTION")!;
            int pUid1 = (int)ClientSys.GetIntFromAttr(attr, "UID_1")!;
            int pUid2 = (int)ClientSys.GetIntFromAttr(attr, "UID_2")!;
            string pPos1 = ClientSys.GetStringFromAttr(attr, "POS_1")!;
            string pPos2 = ClientSys.GetStringFromAttr(attr, "POS_2")!;

            list.Add(new AssetData(num, pDescription, pUid1, pUid2, pPos1, pPos2, type));
        }

        ListAssets = new ObservableCollection<AssetData>(list);
        DataContext = this;
    }
}