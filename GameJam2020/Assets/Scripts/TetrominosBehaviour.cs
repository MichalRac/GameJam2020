using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetrominosBehaviour : MonoBehaviour
{
    [SerializeField] List<TetrominosPiece> tetrominosPieces = new List<TetrominosPiece>();
    List<GameObject> tetrominosPiecesGos =  new List<GameObject>();
    public float UpdateTimeInterval = 1f; //in seconds
    public Color32 FrozenPieceColor = new Color32(255, 255, 255, 255);

    public float Distance = 1f;
    private float currentTime;
    private float pieceHeight;
    private Rigidbody2D myRigidbody;

    public void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.gravityScale = 0f;//turn off gravity for now
        tetrominosPieces.Clear();
        tetrominosPiecesGos.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                tetrominosPiecesGos.Add(child.gameObject);
                tetrominosPieces.Add(child.GetComponent<TetrominosPiece>());
            }
        }
        pieceHeight = tetrominosPieces.FirstOrDefault().GetComponent<BoxCollider2D>().size.y;

    }

    public void Update()
    {
        UpdateTetrominos();
    }


    private void UpdateTetrominos()
    {
        if (!CheckCanMoveToNextPosition())
        {

            myRigidbody.gravityScale = 1f;
            return;
        }

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
        var rayDist = 0.5f;
        var canMoveNow = false;
        for (int i = 0; i < tetrominosPieces.Count; i++)
        {
            var pos = tetrominosPieces[i].transform.position;
            if(transform.eulerAngles.z.Equals(270))
                pos.y -= pieceHeight;
            else if (transform.eulerAngles.z.Equals(180))
                pos.y -= pieceHeight;
            Physics2D.Raycast(pos, Vector2.down, filter, results, rayDist);

            Debug.DrawLine(pos, pos + Vector3.down * rayDist, Color.green);

            if (results.Count > 0 && IsThisOneOfOurPieces(results[0].transform.gameObject))
                continue;

            canMoveNow = CheckResults(results);
            if (!canMoveNow)
                return false;
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
        if (go == gameObject)
            return true;
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
        transform.Translate(new Vector3(0,-Distance,0f),Space.World);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log($"OnCollisionEnter2D with {other.transform.name} ");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"OnTriggerEnter2D with {other.transform.name} ");
    }

    public void SnapTetrominoToPlace()
    {
        foreach (var piece in tetrominosPieces)
        {
            piece.SnapToPlaceIfPossible();
        }
    }

}
