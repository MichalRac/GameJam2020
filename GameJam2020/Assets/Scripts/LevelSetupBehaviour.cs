using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bgHolder;
    [SerializeField] private GameObject borderBlockPrefab;
    [SerializeField] private SpriteRenderer emptySpotPrefab;
    private GameSettingSO gameSettingSO;


    private void Awake()
    {
        gameSettingSO = GameSettingFetcher.instance.GetSettings;
    }

    private void Start()
    {
        SetupLevel();
        CreateBorder();
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
    }

    private void CreateBorder()
    {
        for(int i = 0; i < gameSettingSO.LEVEL_WIDTH; i++)
        {
            Instantiate(borderBlockPrefab, new Vector3(i, -1, 0), Quaternion.identity, bgHolder.transform);
        }
    }
}
