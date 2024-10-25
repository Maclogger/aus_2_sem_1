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
            }

            foreach (Realestate realestate in _realestatesTree.Find(pos2) ?? new List<Realestate>())
            {
                realestate.AddParcel(parcel);
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

    public Answer RemoveParcel(Position position)
    {
        try
        {
            List<Parcel>? duplicateParcels = _parcelasTree.Find(position);

            if (duplicateParcels == null)
            {
                return new Answer($"Neexistuje žiadny prvok s kľúčom {position}", AnswerState.Info);
            }

            if (duplicateParcels.Count == 1)
            {
                _parcelasTree.Remove(position, 0);
                return new Answer($"Prvok s kľúčom {position} bol úspešné odstránený.", AnswerState.Ok);
            }

            int indexFromUser = _application.AskUserToChooseFromList(duplicateParcels);

            _parcelasTree.Remove(position, indexFromUser);
            return new Answer($"Prvok s kľúčom {position} bol úspešné odstránený.", AnswerState.Ok);
        }
        catch (Exception e)
        {
            return new Answer($"Odstránenie prvku {position} sa nepodarilo. {e.Message}", AnswerState.Error);
        }
    }

    public Answer UpdateParcel(Position position, Parcel newparcel)
    {
        throw new NotImplementedException();
    }

    public Tuple<Answer, List<Parcel>> FindParcel(Position position)
    {
        try
        {
            List<Parcel> parcels = _parcelasTree.Find(position) ?? new List<Parcel>();

            if (parcels.Count <= 0)
            {
                return new Tuple<Answer, List<Parcel>>(new Answer($"Neexistuje žiadny prvok s kľúčom {position}", AnswerState.Info), parcels);
            }

            return new Tuple<Answer, List<Parcel>>(new Answer($"Našlo sa {parcels.Count} parciel.", AnswerState.Ok), parcels);
        }
        catch (Exception e)
        {
            return new Tuple<Answer, List<Parcel>>(new Answer($"Odstránenie prvku {position} sa nepodarilo. {e.Message}", AnswerState.Error), new List<Parcel>());
        }
    }





}