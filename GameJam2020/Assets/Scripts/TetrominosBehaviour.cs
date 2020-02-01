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
    private Color32 defaultColor;
    public float Distance = 1f;
    private float currentTime;
    private float pieceHeight;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer myRenderer;
    public bool isBroken;
    private float blockLifeTime;
    private bool wasBrokenThisGame;



    public void Start()
    {
        wasBrokenThisGame = false;
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultColor = myRenderer.color;
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.velocity = Vector2.zero;
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
        //Debug.Log($"Start {gameObject.name} myRigidbody.gravityScale {myRigidbody.gravityScale}");
    }

    public void Update()
    {

        if (!isBroken)
        {
            if (!CheckCanMoveToNextPosition())
            {
                myRigidbody.gravityScale = 1f;
                //Debug.Log($"Update !CheckCanMoveToNextPosition {gameObject.name} myRigidbody.gravityScale {myRigidbody.gravityScale}");
                return;
            }

            blockLifeTime += Time.deltaTime;
            
            if (blockLifeTime > Random.Range(10, 20))
            {
                
                blockLifeTime = 0f;
                BrokeBlocksRandom();
            }
            else
            
                UpdateTetrominosPositions();
        }
    }

    private bool IsRotated90Degrees()
    {
        if ((transform.eulerAngles.z % 90).Equals(0f))
            return true;
        return false;
    }

    private void UpdateTetrominosPositions()
    {

        currentTime += Time.deltaTime;
        if (currentTime > UpdateTimeInterval)
        {
            currentTime = 0f;
            MoveTetrominos();
        }
    }

    private bool canMoveNow;

    private bool CheckCanMoveToNextPosition()
    {
        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask(GameSettingFetcher.instance.GetSettings.TETROMINOS_LAYER_NAME, GameSettingFetcher.instance.GetSettings.DEFAULT_LAYER_NAME),
            useLayerMask = true
        };
        var results = new List<RaycastHit2D>();
        var rayDist = 0.5f;
        canMoveNow = false;
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
        SnapTetrominoToPlace(false);
        myRigidbody.velocity = Vector2.zero;
        transform.Translate(new Vector3(0,-Distance,0f),Space.World);
    }


    public void SnapTetrominoToPlace(bool pernament)
    {
        if (!IsRotated90Degrees())
            return;

        foreach (var piece in tetrominosPieces)
        {
            piece.SnapToPlaceIfPossible(pernament);
        }
    }

    //broken blocks features
    private int brokenesIndex;

    private void ChangeColorForBlocks(Color32 newColor)
    {
        for (int i = 0; i < tetrominosPieces.Count; i++)
        {
            tetrominosPieces[i].ChangeSpriteColor(newColor);
        }
    }

    public void BrokeBlocksRandom()
    {
        if(wasBrokenThisGame)
            return;

        var randomBrokeness = Random.Range(0, 2);
        brokenesIndex = randomBrokeness;
        switch (randomBrokeness)
        {
            case 0:
                if (!IsRotated90Degrees())
                    return;
                SnapTetrominoToPlace(false);
                StopBlocks();
            break;
            case 1:
                FallDownBlocks();
                break;
            default:
                StopBlocks();
            break;
        }
        wasBrokenThisGame = true;
        ChangeColorForBlocks(FrozenPieceColor);
    }

    public void RepairBlocks()
    {
        switch (brokenesIndex)
        {
            case 0:
                RepairStopBlocks();
                break;
            case 1:
                RepairFallDownBLocks();
                break;
            default:
                StopBlocks();
                break;

        }
        ChangeColorForBlocks(defaultColor);
        SnapTetrominoToPlace(false);
    }

    public void StopBlocks()
    {
        isBroken = true;
        //(myRigidbody.constraints & RigidbodyConstraints2D.FreezeAll) != RigidbodyConstraints2D.FreezeAll
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void RepairStopBlocks()
    {
        isBroken = false;
        //(myRigidbody.constraints & RigidbodyConstraints2D.FreezeAll) != RigidbodyConstraints2D.FreezeAll
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void FallDownBlocks()
    {
        isBroken = true;
        myRigidbody.gravityScale = 1f;
        //Debug.Log($"FallDownBlocks {gameObject.name} myRigidbody.gravityScale {myRigidbody.gravityScale}");
    }

    public void RepairFallDownBLocks()
    {
        
        isBroken = false;
    }
}
