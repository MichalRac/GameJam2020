using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bgHolder;
    [SerializeField] private SpriteRenderer emptySpotPrefab;


    private void Awake()
    {
        SetupLevel();
    }

    private void SetupLevel()
    {
        var spotsVertical = LevelConstants.LEVEL_HEIGHT;
        var spotsHorizontal = LevelConstants.LEVEL_WIDTH;
        var pieceSize = LevelConstants.PIECE_SIZE;

        Vector3 creationPos = Vector3.zero;

        var bg = Instantiate(emptySpotPrefab, bgHolder.transform);
        bg.size = new Vector2(spotsHorizontal, spotsVertical);
    }
}
