using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairs : MonoBehaviour
{
    public float RepairRange = 5f;
    private List<TetrominosBehaviour> brokenTetrominosInRange = new List<TetrominosBehaviour>();

    void Start()
    {
        brokenTetrominosInRange.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        TryRepair();
    }

    private void TryAddTetrominos(Collider2D other)
    {
        var tetromino = other.transform.GetComponent<TetrominosBehaviour>();
        if (tetromino != null)
        {
            if (tetromino.isBroken)
            {
                if(!brokenTetrominosInRange.Contains(tetromino))
                    brokenTetrominosInRange.Add(tetromino);
            }
        }
    }

    private void TryRepair()
    {
        var isRepairBtnDown = Input.GetButtonDown("Repair");
        //Debug.Log($"Repair isRepairBtnDown {isRepairBtnDown}");
        if (!isRepairBtnDown)
            return;

        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask(GameSettingFetcher.instance.GetSettings.TETROMINOS_LAYER_NAME, GameSettingFetcher.instance.GetSettings.DEFAULT_LAYER_NAME),
            useLayerMask = true
        };
        var results = new List<RaycastHit2D>();

        Physics2D.CircleCast(transform.position, RepairRange, Vector2.down, filter, results);

        for (int i = 0; i < results.Count; i++)
        {
            TryAddTetrominos(results[i].collider);
        }

        for (int i = 0; i < brokenTetrominosInRange.Count; i++)
        {
            if(brokenTetrominosInRange[i].isBroken && !brokenTetrominosInRange[i].IsSnappedPermanently)
                brokenTetrominosInRange[i].RepairBlocks();
        }

    }


}
