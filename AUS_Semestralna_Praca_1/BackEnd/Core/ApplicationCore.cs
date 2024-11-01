using System;
using System.Collections.Generic;
using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using AUS_Semestralna_Praca_1.BackEnd.DataStructures.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree;
using AUS_Semestralna_Praca_1.BackEnd.Tests.KdTree.Keys;
using Avalonia.Controls;

namespace AUS_Semestralna_Praca_1.BackEnd.Core;

public class ApplicationCore
{
    public MainApplication MainApplication { get; set; }
    public KdTree<Position, Realestate> RealestatesTree { get; set; } = new(2);

    public KdTree<Position, Parcel> ParcelsTree { get; set; } = new(2);
    public KdTree<Position, Asset> AssetsTree { get; set; } = new(2);


    public int ParcelsCount { get; set; } = 0;
    public int RealestatesCount { get; set; } = 0;
    public int AssetsCount { get; set; } = 0;


    public ApplicationCore(MainApplication pMainApplication)
    {
        MainApplication = pMainApplication;
    }

    public Answer AddAsset(Position pos1, Position pos2, Asset asset)
    {
        string message = "";
        if (asset is Realestate realestate)
        {
            try
            {
                // adding new parcel to all realestates at pos1 and pos2
                foreach (Parcel parcel in ParcelsTree.Find(pos1))
                {
                    parcel.AddRealestate(realestate);
                    realestate.AddParcel(parcel);
                }

                foreach (Parcel parcel in ParcelsTree.Find(pos2))
                {
                    parcel.AddRealestate(realestate);
                    realestate.AddParcel(parcel);
                }

                RealestatesTree.Add(pos1, realestate);
                RealestatesTree.Add(pos2, realestate);
            }
            catch (Exception e)
            {
                return new Answer($"Pridanie nehnuteľnosti ({pos1}-{pos2})-{realestate} sa nepodarilo. {e.Message}",
                    AnswerState.Error);
            }

            RealestatesCount++;
            AssetsCount++;
            message = $"Pridanie parcely ({pos1}-{pos2})-{realestate} bolo úspešné.";
        }
        /*
        else if (asset is Parcel parcel)
        {
        }
        */
        else
        {
            throw new ArgumentException($"Asset type {asset.GetType()} is not supported.");
        }

        AssetsTree.Add(pos1, asset);
        AssetsTree.Add(pos2, asset);
        return new Answer(message, AnswerState.Ok);
    }



    public void RunSimTest(TextBlock block)
    {
        // TODO SimulationTester.RunSimTests();
    }
}