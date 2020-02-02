using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.PlayerLoop;

public  enum TetrominoState
{
    FallingClassic,
    FallingPhysics,
    PernamentlySnapped,
    Frozen,
}

public class TetrominoRoot : MonoBehaviour
{
    [SerializeField]
    private List<TetrominosPiece> tetrominoPieces = new List<TetrominosPiece>();
    private GameSettingSO gameSettingSO;
    private TetrominoState _tetrominoState;
    private Rigidbody2D rb2D;
    private float currentTime;
    private int layerMask;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        gameSettingSO = GameSettingFetcher.instance.GetSettings;

        var listToRemove = tetrominoPieces.Where(tetromino => !tetromino.gameObject.activeSelf).ToList();
        foreach (var tetromino in listToRemove)
        {
            tetrominoPieces.Remove(tetromino);
            Destroy(tetromino.gameObject);
        }
    }

    private void Start()
    {
        currentTime = 0f;
        SetTetrominoState(TetrominoState.FallingClassic);
        layerMask = LayerMask.GetMask("Tetrominos", "Default", "Player");
    }

    private void FixedUpdate()
    {
        if (_tetrominoState != TetrominoState.FallingClassic)
        {
            return;
        }

        if (CheckCanSwitchToPhysics())
        {
            SetFallingPhysicsState();
            return;
        }
        
        if (CanMove())
        {
            currentTime = 0f;
            Move();
        }
    }

    private bool CheckCanSwitchToPhysics()
    {
        var results = new List<RaycastHit2D>();
        var rayDist = 0.995f;
        var bufferForRaysOrigins = 0.1f;
        var raycastSpacing = (gameSettingSO.PIECE_SIZE - bufferForRaysOrigins) / 2;

        foreach (var piece in tetrominoPieces)
        {
            var pos = piece.GetCenterOfSprite();
            var diff = new Vector3(raycastSpacing, 0f, 0f);

            results.AddRange(Physics2D.RaycastAll(pos, Vector2.down, rayDist, layerMask).ToList());
            results.AddRange(Physics2D.RaycastAll(pos + diff, Vector2.down, rayDist, layerMask).ToList());
            results.AddRange(Physics2D.RaycastAll(pos - diff, Vector2.down, rayDist, layerMask).ToList());

            Debug.DrawLine(pos, pos + Vector3.down * rayDist, Color.magenta);
            Debug.DrawLine(pos + diff, pos + diff + Vector3.down * rayDist, Color.magenta);
            Debug.DrawLine(pos - diff, pos - diff + Vector3.down * rayDist, Color.magenta);
        }

        bool touchingWall = true;
        foreach (var result in results)
        {
            if (!gameObject.Equals(result.transform.gameObject))
            {
                touchingWall = false;
                break;
            }
        }

        return !touchingWall;
    }

    private bool CanMove()
    {
        if (currentTime > gameSettingSO.TETROMINO_FALL_FREQUENCY)
        {
            return true;
        }

        currentTime += Time.fixedDeltaTime;
        return false;
    }

    private void Move()
    {
        transform.Translate(new Vector3(0, -1, 0), Space.World);
    }

    public void SnapTetrominoToGrid(bool pernamently)
    {
        foreach (var tetrominoPiece in tetrominoPieces)
        {
            tetrominoPiece.SnapToPlaceIfPossible(pernamently);
        }
    }

    #region StateHelper
    
    public void SetTetrominoState(TetrominoState state)
    {
        switch (state)
        {
            case TetrominoState.FallingClassic:
                SetFalingClassicState();
                break;
            case TetrominoState.FallingPhysics:
                SetFallingPhysicsState();
                break;
            case TetrominoState.PernamentlySnapped:
                SetPernamentlySnappedState();
                break;
            case TetrominoState.Frozen:
                SetFrozenState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void SetFalingClassicState()
    {
        _tetrominoState = TetrominoState.FallingClassic;
        rb2D.gravityScale = 0f;
    }
    
    private void SetFallingPhysicsState()
    {
        _tetrominoState = TetrominoState.FallingPhysics;
        rb2D.gravityScale = 1f;

    }

    private void  SetPernamentlySnappedState()
    {
        SnapTetrominoToGrid(true);
        _tetrominoState = TetrominoState.PernamentlySnapped;
        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;

    }

    private void SetFrozenState()
    {
        _tetrominoState = TetrominoState.Frozen;

    }

    #endregion
}
