using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running , raceOver };

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameStates gameState = GameStates.countDown;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
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

   

    public  GameStates GetGameState()
    {
        return gameState;
    }

    public void OnRaceStart()
    {
        Debug.Log("Race started");
        gameState = GameStates.running;
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
