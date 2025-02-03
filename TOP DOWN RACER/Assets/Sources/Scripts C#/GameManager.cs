using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameStates gameState = GameStates.countDown;

    float raceStartedTime = 0;
    float raceCompletedTime = 0;

    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {

    }


    void LevelStart()
    {
        gameState = GameStates.countDown;

        Debug.Log("Level started");
    }



    public GameStates GetGameState()
    {
        return gameState;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;

            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
            return 0;
        else if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else return Time.time - raceStartedTime;
    }

    public void OnRaceStart()
    {
        Debug.Log("Race started");

        raceStartedTime = Time.time;

        ChangeGameState(GameStates.running);
    }

    public void OnRaceCompleated()
    {
        Debug.Log("Race compleated");

        raceCompletedTime = Time.time;

        ChangeGameState(GameStates.raceOver);
    }
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }
}
