using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentGameState = GameState.Lobby;
    public GameObject GameOverPanel;

    public enum GameState
    {
        Lobby,
        Started,
        Finnished
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        GameOverPanel.SetActive(false);
        CurrentGameState = GameState.Started;
    }

    public void EndGame()
    {
        CurrentGameState = GameState.Finnished;
        GameOverPanel.SetActive(true);
        Debug.Log("End Game");
    }
}

