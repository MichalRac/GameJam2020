using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosSpawner : MonoBehaviour
{
    [SerializeField] private List<TetrominosBehaviour> tetrominosPrefabs;

    [SerializeField] private float spawnInterval;

    [SerializeField] Vector3 spawnPoint;

    private void Awake()
    {
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            Instantiate(GetRandomTetromino(), spawnPoint, Quaternion.identity, transform);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private TetrominosBehaviour GetRandomTetromino()
    {
        return tetrominosPrefabs[Random.Range(0,tetrominosPrefabs.Count)];
    }
}
