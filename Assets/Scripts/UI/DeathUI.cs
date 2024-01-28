using DG.Tweening;
using UnityEngine;
using TMPro;

public class DeathUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    [Header("Transitions")]
    [SerializeField] private Transform TitleBox;
    [SerializeField] private float TitleBoxTime;
    [Space(5)]
    [SerializeField] private Transform Score;
    [Space(5)]
    [SerializeField] private Transform Buttons;

    private void Start()
    {
        StartUIFade();
        SetText();
    }

    private void StartUIFade()
    {
        // TitleBox
        float tOldPosY = TitleBox.localPosition.y;
        TitleBox.localPosition = new Vector2(0, Screen.height * 2);
        TitleBox.DOLocalMoveY(tOldPosY, TitleBoxTime).SetEase(Ease.OutExpo).SetDelay(0.5f);

        // Score
        Vector2 sOldPos = Score.localPosition;
        Score.localPosition = new Vector2 (-Screen.width * 2,sOldPos.y);
        Score.DOLocalMoveX(sOldPos.x, TitleBoxTime).SetEase(Ease.OutExpo).SetDelay(0.6f);

        // Buttons
        float bOldPosY = Buttons.localPosition.y;
        Buttons.localPosition = new Vector2(0, -Screen.height * 2);
        Buttons.DOLocalMoveY(bOldPosY, TitleBoxTime).SetEase(Ease.OutExpo).SetDelay(0.7f);
    }

    private void SetText(bool pb = false)
    {
        if (pb)
            scoreText.text = "<color=#ffbb00>" + GameManager.Instance.GetScore().ToString();
        else
            scoreText.text = GameManager.Instance.GetScore().ToString();

        float rTimer = Timer.Instance.GetRoundTimer();
        string mins = Mathf.Floor(rTimer / 60).ToString("00");
        string secs = Mathf.Floor(rTimer % 60).ToString("00");
        
        timeText.text = string.Format("{0}:{1}", mins, secs);
    }
}
