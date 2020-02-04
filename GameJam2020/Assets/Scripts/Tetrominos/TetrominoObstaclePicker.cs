using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetrominos
{
    public enum TetrominoObstacleType
    {
        Ice,
        Rock,
        None,
        Bomb
    }
    
    public static class TetrominoObstaclePicker
    {
        private static float[] chances = new[]
        {
            GameSettingFetcher.instance.GetSettings.TETROMINO_FREEZE_WEIGHT_CHANCE,
            GameSettingFetcher.instance.GetSettings.TETROMINO_FALL_WEIGHT_CHANCE,
            GameSettingFetcher.instance.GetSettings.TETROMINO_REGULAR_WEIGHT_CHANCE,
            GameSettingFetcher.instance.GetSettings.TETROMINO_EXPLODE_CHANCE
        };

        public static TetrominoObstacleType PickObstacleType()
        {
            float draw = Random.Range(0, chances.Sum());

            if (draw == chances.Sum())
                return TetrominoObstacleType.None;
            
            float x = 0;
            
            for (int i = 0; i < chances.Length; i++)
            {
                x += chances[i];
                if (draw < x)
                {
                    return (TetrominoObstacleType) i;
                }
            }

            return TetrominoObstacleType.None;
        }


    }
}