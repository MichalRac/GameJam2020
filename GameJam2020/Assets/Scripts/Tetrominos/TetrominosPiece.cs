using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosPiece : MonoBehaviour
{
    public bool isPernamentlySnapped { get; private set; }
    private Vector2Int snappedPosition;
    private SpriteRenderer mySprite;


    private void Awake()
    {
        mySprite = GetComponentInChildren<SpriteRenderer>();
    }

    public Vector3 GetCenterOfSprite()
    {
        return mySprite.bounds.center;
    }
    
    public void SnapToPlaceIfPossible(bool pernamently)
    {
        var closestPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        snappedPosition = closestPos;
        transform.position = new Vector3(closestPos.x, closestPos.y);
        isPernamentlySnapped = pernamently;

        if(pernamently)
        {
            ChangeSpriteColor(GameSettingFetcher.instance.GetSettings.FIXED_TETROMINO_COLOR);
            GridManager.AddTetrominoAtPosition(snappedPosition, this);
        }
    }

    public void ChangeSpriteColor(Color32 colorToChange)
    {
        mySprite.color = colorToChange;
    }
}
