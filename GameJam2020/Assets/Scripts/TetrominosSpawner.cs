using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosSpawner : MonoBehaviour
{
    [SerializeField] private List<TetrominosBehaviour> tetrominosPrefabs;

    private void Awake()
    {
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        var settings = GameSettingFetcher.instance.GetSettings;
        while(true)
        {
            var spawnPoint = new Vector3(Random.Range(1, settings.LEVEL_WIDTH - 1), settings.LEVEL_HEIGHT, 0);
            Instantiate(GetRandomTetromino(), spawnPoint, Quaternion.identity, transform);
            yield return new WaitForSeconds(settings.TETROMINO_SPAWN_FREQUENCY);
        }
    }

    private TetrominosBehaviour GetRandomTetromino()
    {
        return tetrominosPrefabs[Random.Range(0,tetrominosPrefabs.Count)];
    }
}
