using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetrominosBehaviour : MonoBehaviour
{
    [SerializeField] List<TetrominosPiece> tetrominosPieces;
    List<GameObject> tetrominosPiecesGos =  new List<GameObject>();
    public float UpdateTimeInterval = 1f; //in seconds
    public float Distance = 8f;
    private float currentTime;
    private float pieceHalfHeight;

    public void Start()
    {
        pieceHalfHeight = tetrominosPieces.FirstOrDefault().GetComponent<BoxCollider2D>().size.y / 2;
        for (int i = 0; i < tetrominosPieces.Count; i++)
        {
            tetrominosPiecesGos.Add(tetrominosPieces[i].gameObject);
        }
        //tetrominosPiecesGos = tetrominosPieces.Select(p => p.gameObject).ToList();
    }

    public void Update()
    {
        UpdateTetrominos();
    }


    private void UpdateTetrominos()
    {
        if(!CheckCanMoveToNextPosition())
            return;
        
        currentTime += Time.deltaTime;
        if (currentTime > UpdateTimeInterval)
        {
            currentTime = 0f;
            MoveTetrominos();
        }
    }

    private bool CheckCanMoveToNextPosition()
    {
        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("Default"),
            useLayerMask = true
        };
        var results = new List<RaycastHit2D>();
        var rayDist = 1f;
        var canMoveNow = false;
        for (int i = 0; i < tetrominosPieces.Count; i++)
        {
            var pos = tetrominosPieces[i].transform.position;
            pos.y -= pieceHalfHeight;
            Physics2D.Raycast(pos, Vector2.down, filter, results, rayDist);
            Debug.DrawLine(pos, pos + Vector3.down * rayDist, Color.green);
            if (results.Count > 0 && IsThisOneOfOurPieces(results[0].transform.gameObject))
                continue;
            
            canMoveNow =  CheckResults(results) || canMoveNow;
        }

        return canMoveNow;
    }

    private bool CheckResults(List<RaycastHit2D> results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].distance <= 1f)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsThisOneOfOurPieces(GameObject go)
    {
        for (int i = 0; i < tetrominosPiecesGos.Count; i++)
        {
            if (go == tetrominosPiecesGos[i])
            {
                return true;
            }
        }
        
        return false;
    }

    private void MoveTetrominos()
    {
        transform.Translate(new Vector3(0,-Distance,0f));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log($"OnCollisionEnter2D with {other.transform.name} ");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"OnTriggerEnter2D with {other.transform.name} ");
    }
}
