using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class FuritSliceManager : MonoBehaviour
{
    [SerializeField]
    public FuritsInGameData FuritsInGame;


    public void AddToListSlicedFruit()
    {
        Clearpiecelist();
        var pieces = FindObjectsOfType<FruitPiece>();
        foreach (var piece in pieces)
        {
            AddPiecesFuritToListAfterSclice(piece.FuritTag, piece.Volume, piece.PercentVolume);
            // Debug.Log("CUTTtTTT" + piece.FuritTag);
        }
    }
    public void AddFuritOnStartGame(string tag , float  vloume)
    {

            FuritsInGame.Furits.Clear();
        
            FuritsInGame.Furits.Add(new FuritData { Tag = tag, TotalVolume = vloume, slicedPieces = new List<SlicedPieceData>() });
        
    }
    private void AddPiecesFuritToListAfterSclice(string tag, float vloume, float percent)
    {
       // Clearpiecelist();

        foreach (var furit in FuritsInGame.Furits)
        {
           // furit.slicedPieces.Clear();
            if (tag == furit.Tag)
            {
                // var  unit = furit.TotalVolume ;
               // var percent = (vloume / furit.TotalVolume) * 100;
                furit.slicedPieces.Add(new SlicedPieceData { Tag = tag, Volume = vloume , Percent = percent});
            }
        }
    }
   
    public void Clearpiecelist()
    {
        foreach (var f in FuritsInGame.Furits)
        {
            f.slicedPieces.Clear();
        }
        
    }
}
[Serializable]
public struct FuritsInGameData
{

    public List<FuritData> Furits;
}
[Serializable]
public struct FuritData
{

    public string Tag;
    public float TotalVolume;
    public List<SlicedPieceData> slicedPieces;


}

[Serializable]
public struct SlicedPieceData
{
    public string Tag;
    public float Volume;
    public float Percent;
}