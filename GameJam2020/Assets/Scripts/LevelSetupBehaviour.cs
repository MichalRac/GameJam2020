using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bg;


    private void Awake()
    {
        ResizeBgToScreen();
    }

    private void ResizeBgToScreen()
    {
        var sr = bg.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var targetScale = new Vector3((float)worldScreenWidth / width, 1, (float)worldScreenHeight / height);
        bg.transform.localScale = targetScale;
    }
}
