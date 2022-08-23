using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class FuritSliceManager : MonoBehaviour
{
    [SerializeField]
    public FuritsInGameData FuritsInGame;

    private Cutter cut;
    void Start()
    {
        
        cut = GetComponent<Cutter>();
        cut.OnCut += Cut_OnCut;
        
    }

   

    [Button("find",ButtonSizes.Medium)]
    public void Cut_OnCut()
    {
        Clearpiecelist();
        var pieces = FindObjectsOfType<FruitPiece>();
        foreach (var piece in pieces)
        {
            AddPiecesFuritToListAfterSclice(piece.FuritTag, piece.Volume);
            Debug.Log("CUTTtTTT" + piece.FuritTag);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddFuritOnStartGame(string tag , float  vloume)
    {
       
        
            FuritsInGame.Furits.Add(new FuritData { Tag = tag, TotalVolume = vloume, slicedPieces = new List<SlicedPieceData>() });
        
    }
    private void AddPiecesFuritToListAfterSclice(string tag, float vloume)
    {
       // Clearpiecelist();

        foreach (var furit in FuritsInGame.Furits)
        {
           // furit.slicedPieces.Clear();
            if (tag == furit.Tag)
            {
                furit.slicedPieces.Add(new SlicedPieceData { Tag = tag, Volume = vloume });
            }
        }
    }
    private void Clearpiecelist()
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
}