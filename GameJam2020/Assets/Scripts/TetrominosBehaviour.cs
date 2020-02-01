using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosBehaviour : MonoBehaviour
{
    [SerializeField] List<TetrominosPiece> tetrominosPieces;

    public void SnapTetrominoToPlace()
    {
        foreach(var piece in tetrominosPieces)
        {
            piece.SnapToPlaceIfPossible();
        }
    }
}
