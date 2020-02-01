﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosPiece : MonoBehaviour
{
    private Vector3 snappedPosition;
    private SpriteRenderer mySprite;

    public Vector3 GetClosestVector()
    {
        return new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }

    public void SnapToPlaceIfPossible()
    {
        var closestPos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);

        snappedPosition = closestPos;
        transform.position = closestPos;

        ChangeSpriteColor(GameSettingFetcher.instance.GetSettings.FIXED_TETROMINO_COLOR);
    }

    public void ChangeSpriteColor(Color32 colorToChange)
    {
        mySprite = transform.GetComponentInChildren<SpriteRenderer>();
        mySprite.color = colorToChange;
    }
}
