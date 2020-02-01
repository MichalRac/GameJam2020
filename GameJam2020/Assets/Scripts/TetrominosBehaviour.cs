using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosBehaviour : MonoBehaviour
{
    [SerializeField] List<TetrominosPiece> tetrominosPieces;

    public float UpdateTimeInterval = 1f; //in seconds
    public float Distance = 8f;
    private float currentTime;

    public void Start()
    {

    }

    public void Update()
    {
        UpdateTetrominos();
    }


    private void UpdateTetrominos()
    {
        currentTime += Time.deltaTime;
        if (currentTime > UpdateTimeInterval)
        {
            currentTime = 0f;
            MoveTetrominos();
        }
    }

    private void MoveTetrominos()
    {
        transform.Translate(new Vector3(0,-Distance,0f));
    }
}
