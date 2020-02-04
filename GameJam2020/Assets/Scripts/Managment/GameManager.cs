using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Time = UnityEngine.Time;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentGameState = GameState.Lobby;
    public GameObject endGameScreen;
    public Text scoresText;
    public float CurrentScore;
    public float scoreMultiplayer = 10f;
    public float ScoreForRepair = 100f;
    public float scoreForTImeTick = 1f;
    public float scoreForTImeTickCurrent;
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

        scoreForTImeTickCurrent += Time.deltaTime;

        if (scoreForTImeTickCurrent > scoreForTImeTick)
        {
            scoreForTImeTickCurrent = 0;
            var addScore = (int) Time.deltaTime * scoreMultiplayer;
            if (addScore < 1)
                addScore = 1f;

            UpdateScores(addScore);
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CurrentScore = 0f;
        CurrentGameState = GameState.Started;
        endGameScreen.SetActive(false);
    }

    public void EndGame()
    {
        endGameScreen.SetActive(true);
        CurrentGameState = GameState.Finnished;
        Debug.Log("End Game");
    }


    public void UpdateScores(float addScore)
    {
        CurrentScore += addScore;
        scoresText.text = CurrentScore.ToString();
    }

}

