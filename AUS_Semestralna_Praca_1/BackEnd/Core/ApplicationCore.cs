using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class ApplicationCore
{
    private readonly Application _application;
    private KdTree<Position, Realestate> _realestatesTree = new(2);
    private KdTree<Position, Parcel> _parcelasTree = new(2);

    public ApplicationCore(Application pApplication)
    {
        _application = pApplication;
    }

    public Answer AddParcel(Position pos1, Position pos2,  Parcel parcel)
    {
        try
        {
            // adding new parcel to all realestates at pos1 and pos2
            foreach (Realestate realestate in _realestatesTree.Find(pos1))
            {
                realestate.AddParcel(parcel);
                parcel.AddRealestate(realestate);
            }

            foreach (Realestate realestate in _realestatesTree.Find(pos2))
            {
                realestate.AddParcel(parcel);
                parcel.AddRealestate(realestate);
            }

            int uid1 = _parcelasTree.Add(pos1, parcel);
            int uid2 = _parcelasTree.Add(pos2, parcel);

            parcel.Uid1 = uid1;
            parcel.Uid2 = uid2;
        }
        catch (Exception e)
        {
            return new Answer($"Pridanie prvku ({pos1}-{pos2})-{parcel} sa nepodarilo. {e.Message}", AnswerState.Error);
        }

        return new Answer($"Pridanie parcely ({pos1}-{pos2})-{parcel} bolo úspešné.", AnswerState.Ok);
    }

    public Answer RemoveParcel(Position pos1, int pUid1, Position pos2)
    {
        /*try
        {
            List<DataPart<Parcel>> dupParcelas1 = _parcelasTree.FindDataParts(pos1);
            List<DataPart<Parcel>> dupParcelas2 = _parcelasTree.FindDataParts(pos2);

            if (dupParcelas1.Count == 0 || dupParcelas2.Count == 0)
            {
                return new Answer($"!!! ERROR: Parcela na súradniciach ({pos1}-{pos2}) sa už nenachádzala v systéme!!! ERROR", AnswerState.Error);
            }

            Parcel parcelToDelete = DataPart<Parcel>.GetValue(dupParcelas1, pUid1)!;

            List<Realestate> realestatesAtPos1 = _realestatesTree.Find(pos1);
            List<Realestate> realestatesAtPos2 = _realestatesTree.Find(pos2);
            foreach (Realestate realestate in realestatesAtPos1)
            {
                realestate.RemoveParcel(parcelToDelete);
            }
            foreach (Realestate realestate in realestatesAtPos2)
            {
                realestate.RemoveParcel(parcelToDelete);
            }

            int uid2 = (int)DataPart<Parcel>.GetUid(dupParcelas2, parcelToDelete)!;
            _parcelasTree.Remove(pos1, pUid1);
            _parcelasTree.Remove(pos1, uid2);

            return new Answer($"Parcela na súradniciach ({pos1}-{pos2}) bola úspešne odstránená.", AnswerState.Ok);
        }
        catch (Exception e)
        {
            return new Answer($"Odstránenie parcely na súradniciach ({pos1}-{pos2}) sa nepodarilo. {e.Message}", AnswerState.Error);
        }*/
        throw new NotImplementedException();
    }

    public Answer UpdateParcel(Position oldPos1, Position oldPos2, Position newPos1, Position newPos2, Parcel newParcel, int pUid)
    {
        /*try
        {
            List<DataPart<Parcel>> foundDataParts = _parcelasTree.FindDataParts(oldPos1);
            Parcel? parcel = DataPart<Parcel>.GetValue(foundDataParts, pUid);

            if (parcel == null)
            {
                return new Answer($"Parcela na súradniciach {oldPos1}-{oldPos2} sa nenašla aj keď sa mala. CHYBA", AnswerState.Error);
            }

            if ((oldPos1.Equals(newPos1) && oldPos2.Equals(newPos2)) || (oldPos1.Equals(newPos2) && oldPos2.Equals(newPos1)))
            {
                // if only data is changed
                parcel.Description = newParcel.Description;
                parcel.ParcelNum = newParcel.ParcelNum;
                return new Answer($"Parcela na súradniciach {oldPos1}-{oldPos2} bola úspešne aktualizovaná.", AnswerState.Ok);
            }


            RemoveParcel();



            RemoveParcel(oldPosition, index);


            AddParcel();
        }
        catch (Exception e)
        {
            return new Answer($"Odstránenie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error);
        }



        if (parcels.Count <= 0)
        {
            return new Answer($"Neexistuje žiadna parcela na súradnici {oldPosition}", AnswerState.Info);
        }
        */




        throw new NotImplementedException();
    }

    public Tuple<Answer, List<DataPart<Parcel>>> FindParcelas(Position position)
    {
        try
        {
            List<DataPart<Parcel>> dpParcelas = _parcelasTree.FindDataParts(position);

            if (dpParcelas.Count <= 0)
            {
                return new Tuple<Answer, List<DataPart<Parcel>>>(new Answer($"Neexistuje žiadna parcela na súradnici {position}", AnswerState.Info), dpParcelas);
            }

            return new Tuple<Answer, List<DataPart<Parcel>>>(new Answer($"Našlo sa {dpParcelas.Count} parciel.", AnswerState.Ok), dpParcelas);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<DataPart<Parcel>>>(new Answer($"Vyhľadanie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error), new  List<DataPart<Parcel>>());
        }
    }

    public void PrintParcelTree()
    {
        _parcelasTree.Print();
    }
}