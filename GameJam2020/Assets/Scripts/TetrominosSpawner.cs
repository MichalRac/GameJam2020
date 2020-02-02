using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosSpawner : MonoBehaviour
{
    [SerializeField] private List<TetrominosBehaviour> tetrominosPrefabs;

    private int spawnedObjCounter;
    [SerializeField] private float crrentPace;
    [SerializeField] private float crrentFrequency;
    [SerializeField] private float minFrequency = 3.5f;
    private GameSettingSO settings;

    private void Start()
    {
        settings = GameSettingFetcher.instance.GetSettings;
        crrentPace = 1f;
        crrentFrequency = settings.TETROMINO_SPAWN_FREQUENCY;
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        
        while(GameManager.Instance.CurrentGameState == GameManager.GameState.Started)
        {
            var spawnPoint = new Vector3(Random.Range(2, settings.LEVEL_WIDTH - 3), settings.LEVEL_HEIGHT, 0);
            var newObject = GetRandomTetromino();

            if (IsThereBlockAlready(spawnPoint))
            {
                yield return 0;
            }

            if (spawnedObjCounter > 0 && settings.SPAWN_INCREASE_TICK % spawnedObjCounter == 0)
            {
                crrentPace += crrentPace * settings.SPAWN_PACE_INCREASE_VALUE;
                crrentFrequency -= crrentFrequency * settings.SPAWN_PACE_INCREASE_VALUE;
            }
            crrentFrequency = Mathf.Clamp(crrentFrequency, minFrequency, settings.TETROMINO_SPAWN_FREQUENCY);
            Instantiate(newObject, spawnPoint, newObject.transform.rotation, transform);

            //Debug.Log($"Spawn crrentPace {crrentPace} crrentFrequency {crrentFrequency} tick {settings.SPAWN_INCREASE_TICK} spawnedObjCounter {spawnedObjCounter} frequency {settings.TETROMINO_SPAWN_FREQUENCY}  value {settings.SPAWN_PACE_INCREASE_VALUE}");

            spawnedObjCounter++;
            yield return new WaitForSeconds(crrentFrequency);
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
        var rayDist = 0.5f;
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
