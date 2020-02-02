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
    public Color32 ExplodePieceColor = new Color32(255, 100, 0, 255);
    private Color32 defaultColor;
    public float Distance = 1f;
    private float currentTime;
    private float pieceHeight;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer myRenderer;
    public GameObject explodeFx;

    public bool isBroken;
    public bool IsSnappedPermanently;
    private float blockLifeTime;
    private bool wasBrokenThisGame;
    private bool canMoveNow;
    public bool isSnappedBlockBelowUs;

    [SerializeField]
    private List<GameObject> TetrominoFX = new List<GameObject>();
    private float explodeDelay = 3f;//in seconds
    private float explosionForceStrength = 50f;
    private float explosionRadius = 2f;

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
            if (child.gameObject.activeSelf && child.GetComponent<TetrominosPiece>()!=null)
            {
                tetrominosPieces.Add(child.GetComponent<TetrominosPiece>());
                tetrominosPiecesGos.Add(child.gameObject);
            }
        }
        pieceHeight = tetrominosPieces.FirstOrDefault().GetComponent<BoxCollider2D>().size.y;
        //Debug.Log($"Start {gameObject.name} myRigidbody.gravityScale {myRigidbody.gravityScale}");
    }

    public void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Started)
            return;
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



    private bool CheckCanMoveToNextPosition()
    {
        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask(GameSettingFetcher.instance.GetSettings.TETROMINOS_LAYER_NAME, GameSettingFetcher.instance.GetSettings.DEFAULT_LAYER_NAME),
            useLayerMask = true
        };
        var results = new List<RaycastHit2D>();
        var results2 = new List<RaycastHit2D>();
        var results3 = new List<RaycastHit2D>();

        var rayDist = 0.5f;
        var bufferForRaysOrigins = 0.1f;
        canMoveNow = true;
        isSnappedBlockBelowUs = false;
        for (int i = 0; i < tetrominosPieces.Count; i++)
        {
            var pos = tetrominosPieces[i].transform.position;
            //pos.y += bufferForRaysOrigins;
            var pivotAtRightSide = false;
            if (transform.eulerAngles.z.Equals(90))
            {
                pivotAtRightSide = true;
            }
            else if (transform.eulerAngles.z.Equals(180))
            {
                pos.y -= pieceHeight;
                pivotAtRightSide = true;
            }
            else if (transform.eulerAngles.z.Equals(270))
            {
                pos.y -= pieceHeight;
                pivotAtRightSide = false;
            }

            var pos2 = pos;
            var pos3 = pos;

            if (pivotAtRightSide)
            {
                pos.x -= bufferForRaysOrigins;
                pos2.x -= pieceHeight / 2;
                pos3.x -= pieceHeight - bufferForRaysOrigins;
            }
            else
            {
                pos.x += bufferForRaysOrigins;
                pos2.x += pieceHeight / 2;
                pos3.x += pieceHeight - bufferForRaysOrigins;
            }

            Physics2D.Raycast(pos, Vector2.down, filter, results, rayDist);
            Debug.DrawLine(pos, pos + Vector3.down * rayDist, Color.green);

            Physics2D.Raycast(pos2, Vector2.down, filter, results2, rayDist);
            Debug.DrawLine(pos2, pos2 + Vector3.down * rayDist, Color.green);

            Physics2D.Raycast(pos3, Vector2.down, filter, results3, rayDist);
            Debug.DrawLine(pos3, pos3 + Vector3.down * rayDist, Color.green);

            results.AddRange(results2);
            results.AddRange(results3);

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
                var tetrominosScript = results[i].collider.transform.GetComponent<TetrominosBehaviour>();
                if (tetrominosScript != null && tetrominosScript.IsSnappedPermanently || 
                    results[i].collider.CompareTag("BlockBorder"))
                    isSnappedBlockBelowUs = true;

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
        if (!IsRotated90Degrees() || !isSnappedBlockBelowUs)
            return;
        if (pernament)
            IsSnappedPermanently = true;
        foreach (var piece in tetrominosPieces)
        {
            if (piece == null) continue;
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
        if(wasBrokenThisGame || IsSnappedPermanently)
            return;

        var randomBrokeness = Random.Range(0, 3);
        brokenesIndex = randomBrokeness;

       switch (randomBrokeness)
         {
             case 0:
                 if (!IsRotated90Degrees())
                     return;
                 SnapTetrominoToPlace(false);
                 ChangeColorForBlocks(FrozenPieceColor);
                 RunTetrominoFX(0, true);
                StopBlocks(true);
             break;
             case 1:
                 FallDownBlocks();
                 break;
             case 2:
                 Explode();
                 ChangeColorForBlocks(ExplodePieceColor);
                 break;

             default:
                 ChangeColorForBlocks(FrozenPieceColor);
                 StopBlocks(true);
             break;
         }
       
        wasBrokenThisGame = true;

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
            case 2:
                RepairExplode();
                break;
            default:
                StopBlocks(false);
                break;

        }
        ChangeColorForBlocks(defaultColor);
        SnapTetrominoToPlace(false);
    }

    public void StopBlocks(bool broken)
    {
        isBroken = broken;
        //(myRigidbody.constraints & RigidbodyConstraints2D.FreezeAll) != RigidbodyConstraints2D.FreezeAll
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void RepairStopBlocks()
    {
        isBroken = false;
        //(myRigidbody.constraints & RigidbodyConstraints2D.FreezeAll) != RigidbodyConstraints2D.FreezeAll
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        RunTetrominoFX(0, false);
    }

    public void FallDownBlocks()
    {
        isBroken = false;
        myRigidbody.gravityScale = 1f;
        //Debug.Log($"FallDownBlocks {gameObject.name} myRigidbody.gravityScale {myRigidbody.gravityScale}");
    }

    public void RepairFallDownBLocks()
    {
        isBroken = false;
    }

    public void Explode()
    {
        isBroken = true;
        myRigidbody.gravityScale = 1f;
        StartCoroutine(ExplodeDelayed(explodeDelay));
        //Debug.Log($"FallDownBlocks {gameObject.name} myRigidbody.gravityScale {myRigidbody.gravityScale}");
    }

    private IEnumerator ExplodeDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        var pos = transform.position;
        pos.z += 2f;
        Instantiate(explodeFx,pos , Quaternion.identity);
        AddExplosionForce(pos);
        Destroy(gameObject);
        

    }

    private void AddExplosionForce(Vector3 pos)
    {
        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask(GameSettingFetcher.instance.GetSettings.TETROMINOS_LAYER_NAME, GameSettingFetcher.instance.GetSettings.PLAYER_LAYER_NAME),
            useLayerMask = true
        };

        var results = new List<RaycastHit2D>();
        Physics2D.CircleCast(pos, explosionRadius, Vector2.down, filter, results, explosionRadius);
        Debug.DrawLine(pos, pos + Vector3.down * explosionRadius, Color.red, 1f);

        foreach (var result in results)
        {
            var dir3 = (result.transform.position - pos).normalized;
            var dir2 = new Vector2(dir3.x,dir3.y);

            if (result.collider.CompareTag(GameSettingFetcher.instance.GetSettings.PLAYER_LAYER_NAME))
            {
                GameManager.Instance.EndGame();
                return;
            }

            var go = result.collider.gameObject.GetComponent<TetrominosBehaviour>();

            if (go != null)
            {
                //Debug.Log($"AddExplosionForce Hit, {go.name}");
                if(!go.IsSnappedPermanently)
                    go.myRigidbody.AddForce(dir2 * explosionForceStrength, ForceMode2D.Impulse);
            }
        }
    }

    public void RepairExplode()
    {
        StopAllCoroutines();
    }

    private void RunTetrominoFX(int _index, bool _activation)
    {
        TetrominoFX[_index].SetActive(_activation);
        TetrominoFX[_index].transform.rotation = Quaternion.identity;
    }
}
