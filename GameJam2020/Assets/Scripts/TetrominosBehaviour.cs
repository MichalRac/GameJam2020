using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosBehaviour : MonoBehaviour
{
    [SerializeField] List<TetrominosPiece> tetrominosPieces;

    public float UpdateTimeInterval = 1f; //in seconds
    public float Distance = 8f;
    private float currentTime;
    private bool canMoveNow;


    public void Start()
    {
        canMoveNow = true;
    }

    public void Update()
    {
        UpdateTetrominos();
    }


    private void UpdateTetrominos()
    {
        if(!canMoveNow)
            return;
        
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        canMoveNow = false;
        //Debug.Log($"OnCollisionEnter2D with {other.transform.name} ");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"OnTriggerEnter2D with {other.transform.name} ");
    }
}
