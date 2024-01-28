using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [Header("Timer")]
    [SerializeField] private float rTimer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetTimer()
    {
        rTimer = 0f;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Death)
            return;

        rTimer += Time.deltaTime;
    }

    public int GetRoundMinutes()
    {
        return (int)Mathf.Floor(rTimer / 60);
    }

    public float GetRoundTimer()
    {
        return rTimer;
    }
}
