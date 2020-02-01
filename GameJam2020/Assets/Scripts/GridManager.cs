using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridManager
{
    public static Dictionary<Vector2Int, TetrominosPiece> tetromnoGrid = new Dictionary<Vector2Int, TetrominosPiece>();

    public static void ClearOnStartUp()
    {
        tetromnoGrid.Clear();
    }

    public static void AddTetrominoAtPosition(Vector2Int pos, TetrominosPiece piece)
    {
        tetromnoGrid.Add(pos, piece);
        CheckLinesAtLine(pos.y);
    }

    private static void CheckLinesAtLine(int line)
    {
        var blocksInRow = GameSettingFetcher.instance.GetSettings.LEVEL_WIDTH;
        bool anyDeactivated = false;
        List<Tuple<Vector2Int, GameObject>> consideredTetrominos = new List<Tuple<Vector2Int, GameObject>>();

        for (int i = 0; i < blocksInRow; i++)
        {
            if(tetromnoGrid.TryGetValue(new Vector2Int(i, line), out var tetrominosPiece))
            {
                if(!tetrominosPiece.isPernamentlySnapped)
                {
                    anyDeactivated = true;
                    consideredTetrominos.Clear();
                    break;
                }
                consideredTetrominos.Add(new Tuple<Vector2Int, GameObject>(new Vector2Int(i, line), tetrominosPiece.gameObject));
            }
            else
            {
                consideredTetrominos.Clear();
                anyDeactivated = true;
                break;
            }
        }

        if(!anyDeactivated)
        {
            foreach(var pair in consideredTetrominos)
            {
                tetromnoGrid.Remove(pair.Item1);
                UnityEngine.Object.Destroy(pair.Item2);
            }
        }
    }
}
