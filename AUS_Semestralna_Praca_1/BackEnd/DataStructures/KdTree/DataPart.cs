using System.Collections.Generic;

namespace AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;

public class DataPart<T>
{
    private int _uid;
    private T _value;

    public DataPart(T value)
    {
        _value = value;
        _uid = Utils.GetNextVal();
    }

    public int Uid => _uid;

    public T Value
    {
        get => _value;
        set => _value = value;
    }


    public static T? GetValue<T>(List<DataPart<T>> list, int pUid)
    {
        foreach (DataPart<T> dataPart in list)
        {
            if (dataPart.Uid == pUid)
            {
                return dataPart.Value;
            }
        }

        return default;
    }

    public static int? GetUid<T>(List<DataPart<T>> list, T t)
    {
        foreach (DataPart<T> dataPart in list)
        {
            if (dataPart.Value?.Equals(t) ?? false)
            {
                return dataPart.Uid;
            }
        }

        return default;
    }
}