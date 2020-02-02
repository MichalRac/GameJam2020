using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "ScriptableObjects/GameSettingSO", order = 1)]
public class GameSettingSO : ScriptableObject
{
    public int PIECE_SIZE = 8;
    public int LEVEL_WIDTH = 30;
    public int LEVEL_HEIGHT = 24;
    public float SPAWN_PACE_INCREASE_VALUE = 0.25f; //every x blocks spawned increase pace by value
    public int SPAWN_INCREASE_TICK = 10; //every x blocks spawned increase pace by value
    public float BLOCKS_PACE_INCREASE_VALUE = 0.25f;
    public int BLOCKS_PACE_MOVE_RATE = 5;
    public float TETROMINO_SPAWN_FREQUENCY = 4f;
    public Color32 FIXED_TETROMINO_COLOR;
    public string TETROMINOS_LAYER_NAME = "Tetrominos";
    public string DEFAULT_LAYER_NAME = "Default";
    public string PLAYER_LAYER_NAME = "Player";
}
