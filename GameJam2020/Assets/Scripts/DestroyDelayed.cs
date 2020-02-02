using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelayed : MonoBehaviour
{
    public float timeToDestroy;
    private float currentTime;

    void Start()
    {
        currentTime = 0f;
    }


    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > timeToDestroy)
            Destroy(gameObject);
    }
}
