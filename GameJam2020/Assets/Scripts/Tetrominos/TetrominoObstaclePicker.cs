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
        None
    }
    
    public static class TetrominoObstaclePicker
    {
        private static float[] chances = new[]
        {
            GameSettingFetcher.instance.GetSettings.TETROMINO_FALL_WEIGHT_CHANCE,
            GameSettingFetcher.instance.GetSettings.TETROMINO_FREEZE_WEIGHT_CHANCE,
            GameSettingFetcher.instance.GetSettings.TETROMINO_REGULAR_WEIGHT_CHANCE,
        };

        public static TetrominoObstacleType PickObstacleType()
        {
            float draw = Random.Range(0, chances.Sum());
            float x = 0;
            
            for (int i = 0; i < chances.Length; i++)
            {
                x += chances[i];
                if (x < draw)
                {
                    return (TetrominoObstacleType) i;
                }
            }

            return TetrominoObstacleType.None;
        }


    }
}