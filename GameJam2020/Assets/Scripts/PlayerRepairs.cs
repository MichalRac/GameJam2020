using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairs : MonoBehaviour
{

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        TryAddTetrominos(other);
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{

    //}

    private void OnTriggerExit2D(Collider2D other)
    {
        var tetromino = other.transform.GetComponent<TetrominosBehaviour>();
        if (tetromino != null)
        {
            if (brokenTetrominosInRange.Contains(tetromino))
                brokenTetrominosInRange.Remove(tetromino);
        }
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

        for (int i = 0; i < brokenTetrominosInRange.Count; i++)
        {
            brokenTetrominosInRange[i].RepairBlocks();
        }

    }


}
