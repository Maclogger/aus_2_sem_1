using System;
using System.IO;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public abstract class Asset
{
    public abstract void ToAttr(ref string attr);

    public abstract void Save(BinaryWriter binaryWriter);

    public static Asset Load(BinaryReader reader, Position[] positions)
    {
        char type = reader.ReadChar();

        return type == 'P' ? Parcel.Load(reader, positions) : Realestate.Load(reader, positions);
    }

    public int Index { get; set; }
}