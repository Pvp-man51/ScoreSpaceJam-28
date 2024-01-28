using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public delegate void MutateEnemies();
    public static event MutateEnemies onMutateEnemies;

    [Header("Game State")]
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    [Header("Score")]
    [SerializeField] private int Score;

    private int bestScore;

    [Header("ComboCounter")]
    [SerializeField] private float maxTimeBetweenHits = 1f;

    public int hitCounter { get; private set; }
    private float lastHitTime;

    private GameUIManager uiManager;

    public LeaderboardManager leaderboardManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateGameState(GameState.Normal);

        Cursor.lockState = CursorLockMode.Confined;

        //leaderboardManager.GetComponent<LeaderboardManager>();

        //uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameUIManager>();
    }

    #region GameState

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Normal:
                break;
            case GameState.Death:
                Death();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);

        print("Current State: " + State.ToString());
    }

    private void Death()
    {
        MenuManager.Instance.OpenMenu("Death");
        uiManager.StartTransition();

        if (Score > bestScore)
        {
            bestScore = Score;
            PlayerPrefs.SetFloat("BestScore", bestScore);
        }
    }

    #endregion

    #region Mutation

    public void StartMutation()
    {
        onMutateEnemies?.Invoke();
    }

    public int GetMutatedEnemyHealth(int currentHealth)
    {
        return currentHealth++;
    }

    public float GetMutatedSpeed(float currentSpeed)
    {
        return currentSpeed += 1;
    }

    public int GetMutatedDamage(int currentDamage)
    {
        print(currentDamage);
        return currentDamage += 1;
    }

    public int GetEnemyAmount()
    {
        int minutes = Timer.Instance.GetRoundMinutes();
        int min = 1 + (int)Mathf.Floor(minutes / 2);
        int max = (int)Mathf.Floor(minutes / 1.5f);
        return Random.Range(min, max);
    }

    #endregion

    #region ComboCounter

    public void AddHit()
    {
        if (Time.time - lastHitTime < maxTimeBetweenHits)
        {
            // Add Hit to count
            hitCounter++;
        }
        else
        {
            // Delay to big => not a combo anymore
            // Reset the counter and start with this hit
            hitCounter = 1;
        }

        lastHitTime = Time.time;
    }

    #endregion

    #region ScoreCounter

    public int GetScore()
    {
        return Score;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    #endregion

    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

public enum GameState
{
    Normal,
    Death
}