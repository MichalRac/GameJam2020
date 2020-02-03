using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosPiece : MonoBehaviour
{
    public bool isPernamentlySnapped { get; private set; }

    [SerializeField]
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

      
        transform.position = new Vector3(closestPos.x, closestPos.y);

        Transform _spriteChild = transform.Find("Sprite");
        _spriteChild.SetPositionAndRotation(_spriteChild.position, Quaternion.Euler(0f, 0f, 0f));
        snappedPosition = new Vector2Int(Mathf.RoundToInt(_spriteChild.position.x-0.5f), Mathf.RoundToInt(_spriteChild.position.y-0.5f));

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
