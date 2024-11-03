using System;
using System.IO;
using AUS_Semestralna_Praca_1.BackEnd.Files;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public abstract class Asset
{
    public abstract void ToAttr(ref string attr);
}