using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "ScriptableObjects/GameSettingSO", order = 1)]
public class GameSettingSO : ScriptableObject
{
    public int PIECE_SIZE = 1;
    public int LEVEL_WIDTH = 30;
    public int LEVEL_HEIGHT = 24;
    public float TETROMINO_SPAWN_FREQUENCY = 4f;
    public float TETROMINO_FALL_FREQUENCY = 4f;
    public Color32 FIXED_TETROMINO_COLOR;
    public string TETROMINOS_LAYER_NAME = "Tetrominos";
    public string DEFAULT_LAYER_NAME = "Default";
    public string PLAYER_LAYER_NAME = "Player";
}
