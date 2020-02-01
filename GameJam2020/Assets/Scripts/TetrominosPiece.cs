using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosPiece : MonoBehaviour
{
    private Vector3 snappedPosition;

    public Vector3 GetClosestVector()
    {
        return new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }

    public void SnapToPlaceIfPossible()
    {
        var closestPos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);

        snappedPosition = closestPos;
        transform.position = closestPos;
    }
}
