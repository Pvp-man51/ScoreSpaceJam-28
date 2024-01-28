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

        string mins = Mathf.Floor(rTimer / 60).ToString("00");
        string secs = Mathf.Floor(rTimer % 60).ToString("00");

        GameUIManager.Instance.UpdateTimer(string.Format("{0}:{1}", mins, secs));
    }

    public int GetRoundMinutes()
    {
        return (int)Mathf.Floor(rTimer / 60);
    }

    public string GetFormatedTime()
    {
        string mins = Mathf.Floor(rTimer / 60).ToString("00");
        string secs = Mathf.Floor(rTimer % 60).ToString("00");
        return string.Format("{0}:{1}", mins, secs);
    }

    public float GetRoundTimer()
    {
        return rTimer;
    }
}
