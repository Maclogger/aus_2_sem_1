using My.CoreGui;
using My.DataStructures.KdTree;

namespace My.Core;

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
            foreach (Realestate realestate in _realestatesTree.Find(pos1) ?? new List<Realestate>())
            {
                realestate.AddParcel(parcel);
                parcel.AddRealestate(realestate);
            }

            foreach (Realestate realestate in _realestatesTree.Find(pos2) ?? new List<Realestate>())
            {
                realestate.AddParcel(parcel);
                parcel.AddRealestate(realestate);
            }

            _parcelasTree.Add(pos1, parcel);
            _parcelasTree.Add(pos2, parcel);
        }
        catch (Exception e)
        {
            return new Answer($"Pridanie prvku ({pos1}-{pos2})-{parcel} sa nepodarilo. {e.Message}", AnswerState.Error);
        }

        return new Answer($"Pridanie parcely ({pos1}-{pos2})-{parcel} bolo úspešné.", AnswerState.Ok);
    }

    public Answer RemoveParcel(Position position, int? pUid = null)
    {
        try
        {
            List<DataPart<Parcel>> dupDataPartsParcels = _parcelasTree.FindDataParts(position);

            if (dupDataPartsParcels.Count == 0)
            {
                return new Answer($"Nenašla sa žiadna parcela na súradnici {position}", AnswerState.Info);
            }

            int uid = pUid ?? _application.GetUidFromUserByChoosingFromList(dupDataPartsParcels);



            Parcel? parcelToDelete = DataPart<Parcel>.GetValue(dupDataPartsParcels, uid);

            if (parcelToDelete == null)
            {
                return new Answer($"ERROR: Parcela na súradnici {position} s UID: {uid} sa nenašla!!!", AnswerState.Error);
            }

            List<Realestate> realestatesAtThatPosition = _realestatesTree.Find(position);
            foreach (Realestate realestate in realestatesAtThatPosition)
            {
                realestate.RemoveParcel(parcelToDelete);
            }

            _parcelasTree.Remove(position, uid);

            return new Answer($"Parcela na súradnici {position} bola úspešne odstránená.", AnswerState.Ok);
        }
        catch (Exception e)
        {
            return new Answer($"Odstránenie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error);
        }
    }

    public Answer UpdateParcel(Position oldPos1, Position oldPos2, Position newPos1, Position newPos2, Parcel newparcel, int index)
    {
        try
        {
            Parcel parcel = _parcelasTree.Find(oldPos1)![index];
            if (oldPosition.Equals(newPosition))
            {

                parcel.Description = newparcel.Description;
                parcel.ParcelNum = newparcel.ParcelNum;
                return new Answer($"Parcela na súradnici {oldPosition} bola úspešne aktualizovaná.", AnswerState.Ok);
            }

            RemoveParcel(oldPosition, index);


            AddParcel()



        }
        catch (Exception e)
        {
            return new Answer($"Odstránenie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error);
        }



        if (parcels.Count <= 0)
        {
            return new Answer($"Neexistuje žiadna parcela na súradnici {oldPosition}", AnswerState.Info);
        }




        throw new NotImplementedException();
    }

    public Tuple<Answer, List<Parcel>> FindParcel(Position position)
    {
        try
        {
            List<Parcel> parcels = _parcelasTree.Find(position) ?? new List<Parcel>();

            if (parcels.Count <= 0)
            {
                return new Tuple<Answer, List<Parcel>>(new Answer($"Neexistuje žiadna parcela na súradnici {position}", AnswerState.Info), parcels);
            }

            return new Tuple<Answer, List<Parcel>>(new Answer($"Našlo sa {parcels.Count} parciel.", AnswerState.Ok), parcels);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<Parcel>>(new Answer($"Odstránenie parcely na súradnici {position} sa nepodarilo. {e.Message}", AnswerState.Error), new List<Parcel>());
        }
    }





}