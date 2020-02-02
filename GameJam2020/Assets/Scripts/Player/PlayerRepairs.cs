using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairs : MonoBehaviour
{
    public float RepairRange = 5f;
    private List<TetrominoRoot> brokenTetrominosInRange = new List<TetrominoRoot>();

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
        var tetromino = other.transform.GetComponent<TetrominoRoot>();
        if (tetromino != null)
        {
            if (tetromino._tetrominoState == TetrominoState.Frozen)
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

        foreach (var frozenTetromino in brokenTetrominosInRange)
        {
            if (frozenTetromino._tetrominoState == TetrominoState.Frozen)
            {
                frozenTetromino.SetTetrominoState(TetrominoState.FallingPhysics);
                frozenTetromino.SetDefaultVisuals();
            }
            
        }
    }
}
