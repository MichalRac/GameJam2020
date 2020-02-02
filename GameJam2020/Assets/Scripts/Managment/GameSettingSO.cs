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
    
    [Space(15)]
    
    public Color32 FIXED_TETROMINO_COLOR;
    
    [Space(15)]
    
    public string TETROMINOS_LAYER_NAME = "Tetrominos";
    public string DEFAULT_LAYER_NAME = "Default";
    public string PLAYER_LAYER_NAME = "Player";

    [Space(15)] 
    public int TETROMINO_OBSTACLE_MAX_ACTIVATE_HEIGHT = 20;
    public int TETROMINO_OBSTACLE_MIN_ACTIVATE_HEIGHT = 10;
   
    [Space(5)] 

    public float TETROMINO_REGULAR_WEIGHT_CHANCE = 15f;
    public float TETROMINO_FREEZE_WEIGHT_CHANCE = 15f;
    public float TETROMINO_FALL_WEIGHT_CHANCE = 15f;
}
