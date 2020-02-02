using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentGameState = GameState.Lobby;
    public GameObject endGameScreen;

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

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CurrentGameState = GameState.Started;
        endGameScreen.SetActive(false);
    }

    public void EndGame()
    {
        endGameScreen.SetActive(true);
        CurrentGameState = GameState.Finnished;
        Debug.Log("End Game");
    }
}

