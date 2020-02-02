using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bgHolder;
    [SerializeField] private GameObject borderBlockPrefab;
    [SerializeField] private SpriteRenderer emptySpotPrefab;
    [SerializeField] private Camera levelCamera;
    [SerializeField] private Color32 borderColor;
    private GameSettingSO gameSettingSO;


    private void Start()
    {
        gameSettingSO = GameSettingFetcher.instance.GetSettings;

        SetupLevel();
        CreateBorder();

        GridManager.ClearOnStartUp();
    }

    private void SetupLevel()
    {
        var spotsVertical = gameSettingSO.LEVEL_HEIGHT;
        var spotsHorizontal = gameSettingSO.LEVEL_WIDTH;
        var pieceSize = gameSettingSO.PIECE_SIZE;

        Vector3 creationPos = Vector3.zero;

        var bg = Instantiate(emptySpotPrefab, bgHolder.transform);
        bg.size = new Vector2(spotsHorizontal + 2, spotsVertical + 2);
        bg.transform.Translate(new Vector3(-1, -1, 0));
        levelCamera.transform.position = new Vector3((spotsHorizontal) / 2, levelCamera.transform.position.y, levelCamera.transform.position.z);
    }

    private void CreateBorder()
    {
        for(int i = 0; i < gameSettingSO.LEVEL_WIDTH; i++)
        {
            var borderPiece = Instantiate(borderBlockPrefab, new Vector3(i, -1, 0), Quaternion.identity, bgHolder.transform);
            borderPiece.GetComponent<TetrominosPiece>().ChangeSpriteColor(borderColor);
        }

        for(int i = 0; i < gameSettingSO.LEVEL_HEIGHT * 2; i++)
        {
            var borderPiece = Instantiate(borderBlockPrefab, new Vector3(-1, i, 0), Quaternion.identity, bgHolder.transform);
            borderPiece.GetComponent<TetrominosPiece>().ChangeSpriteColor(borderColor);
            borderPiece = Instantiate(borderBlockPrefab, new Vector3(gameSettingSO.LEVEL_WIDTH, i, 0), Quaternion.identity, bgHolder.transform);
            borderPiece.GetComponent<TetrominosPiece>().ChangeSpriteColor(borderColor);
        }
    }
}
