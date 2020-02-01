using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingFetcher : MonoBehaviour
{
    public static GameSettingFetcher instance;

    [SerializeField] private GameSettingSO gameSetting;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public GameSettingSO GetSettings => gameSetting;
}
