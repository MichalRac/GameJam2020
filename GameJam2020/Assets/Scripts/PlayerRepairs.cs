using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        TryRepair(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryRepair(other);
    }

    private static void TryRepair(Collider2D other)
    {
        var tetromino = other.transform.GetComponent<TetrominosBehaviour>();
        if (tetromino != null)
        {
            if (tetromino.isBroken)
            {
                tetromino.RepairBlocks();
            }
        }
    }


}
