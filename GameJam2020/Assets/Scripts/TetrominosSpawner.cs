using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosSpawner : MonoBehaviour
{
    [SerializeField] private List<TetrominosBehaviour> tetrominosPrefabs;

    private void Awake()
    {
    }

    private IEnumerator Spawn()
    {
        yield return null;
    }
}
