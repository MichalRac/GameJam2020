using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosSpawner : MonoBehaviour
{
    [SerializeField] private List<TetrominosBehaviour> tetrominosPrefabs;

    private void Start()
    {
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        var settings = GameSettingFetcher.instance.GetSettings;
        while(true)
        {
            var spawnPoint = new Vector3(Random.Range(2, settings.LEVEL_WIDTH - 3), settings.LEVEL_HEIGHT, 0);
            var newObject = GetRandomTetromino();
            Instantiate(newObject, spawnPoint, newObject.transform.rotation, transform);
            yield return new WaitForSeconds(settings.TETROMINO_SPAWN_FREQUENCY);
        }
    }

    private TetrominosBehaviour GetRandomTetromino()
    {
        return tetrominosPrefabs[Random.Range(0,tetrominosPrefabs.Count)];
    }
}
