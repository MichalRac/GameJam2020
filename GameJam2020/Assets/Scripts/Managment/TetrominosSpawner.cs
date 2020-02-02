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
        while(GameManager.Instance.CurrentGameState == GameManager.GameState.Started)
        {
            var spawnPoint = new Vector3(Random.Range(3, settings.LEVEL_WIDTH - 4), settings.LEVEL_HEIGHT, 0);
            var newObject = GetRandomTetromino();

            if (IsThereBlockAlready(spawnPoint))
            {
                yield return 0;
            }

            Instantiate(newObject, spawnPoint, newObject.transform.rotation, transform);
            yield return new WaitForSeconds(settings.TETROMINO_SPAWN_FREQUENCY);
        }
    }

    private TetrominosBehaviour GetRandomTetromino()
    {
        return tetrominosPrefabs[Random.Range(0,tetrominosPrefabs.Count)];
    }

    private bool IsThereBlockAlready(Vector3 pos)
    {
        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask(GameSettingFetcher.instance.GetSettings.TETROMINOS_LAYER_NAME),
            useLayerMask = true
        };
        var rayDist = 1f;
        var results = new List<RaycastHit2D>();
        Physics2D.CircleCast(pos, rayDist, Vector2.down, filter, results, rayDist);
        Debug.DrawLine(pos, pos + Vector3.down * rayDist, Color.blue, 1f);
        foreach (var result in results)
        {
            var go = result.collider.gameObject.GetComponent<TetrominosBehaviour>();

            if (go != null)
            {
                Debug.Log($"Spawner Hit, end game {go.name}");
                GameManager.Instance.EndGame();
            }
        }

        return false;
    }
}
