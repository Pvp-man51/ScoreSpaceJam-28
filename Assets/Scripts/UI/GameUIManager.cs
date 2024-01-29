using System.Collections;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("Death")]
    [SerializeField] private Material DeathTranMat;
    [SerializeField] private float TransitionTime = 1f;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboCounter;
    [SerializeField] private TMP_Text timerText;

    private const string _tPropertyname = "_Progress";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AudioManager.Instance.Stop("MenuMusic");
        AudioManager.Instance.Play("CombatMusic");
        DeathTranMat.SetFloat(_tPropertyname, 1f); // Reset Death Transition
        transform.GetChild(0).GetComponent<Menu>().open = true;
    }

    private void Update()
    {
        UpdateScoreCounter();
    }

    #region Transition

    public void StartTransition()
    {
        StartCoroutine(StartDeathTransition());
    }

    public IEnumerator StartDeathTransition()
    {
        float currentTime = 0;
        while (currentTime < TransitionTime)
        {
            currentTime += Time.deltaTime;

            DeathTranMat.SetFloat(_tPropertyname, Mathf.Clamp01(1 - (currentTime / TransitionTime)));

            yield return null;
        }
    }

    #endregion

    #region Score

    private void UpdateScoreCounter()
    {
        scoreText.text = GameManager.Instance.GetScore().ToString();
        
        if (GameManager.Instance.hitCounter > 0) 
        {
            comboCounter.text = "x" + GameManager.Instance.hitCounter.ToString();
        }
        else
        {
            comboCounter.text = string.Empty;
        }
    }

    public void UpdateTimer(string time)
    {
        timerText.text = time;
    }

    #endregion
}
