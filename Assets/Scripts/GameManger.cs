using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public delegate void MutateEnemies();
    public static event MutateEnemies onMutateEnemies;

    [Header("Score")]
    [SerializeField] private int Score;

    private int bestScore;

    [Header("Camera")]
    public Camera Cam { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cam = Camera.main;
    }

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


    public void QuitGame()
    {
        Application.Quit();
    }
}