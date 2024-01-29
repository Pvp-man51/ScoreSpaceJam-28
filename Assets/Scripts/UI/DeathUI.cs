using DG.Tweening;
using UnityEngine;
using TMPro;

public class DeathUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text bestScore;

    [Header("Transitions")]
    [SerializeField] private Transform TitleBox;
    [SerializeField] private float TitleBoxTime;
    [Space(5)]
    [SerializeField] private Transform Score;
    [Space(5)]
    [SerializeField] private Transform Buttons;
    [Space(5)]
    [SerializeField] private Transform UploadScoreButton;

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

        // Leaderboard UploadButton
        Vector2 uBOldPos = UploadScoreButton.localPosition;
        UploadScoreButton.localPosition = new Vector2(Screen.width * 2, uBOldPos.y);
        UploadScoreButton.DOLocalMoveX(uBOldPos.x, TitleBoxTime).SetEase(Ease.OutExpo).SetDelay(0.6f);
    }

    private void SetText(bool pb = false)
    {
        if (pb)
            scoreText.text = "<color=#ffbb00>" + GameManager.Instance.GetScore().ToString();
        else
            scoreText.text = GameManager.Instance.GetScore().ToString();

        timeText.text = Timer.Instance.GetFormatedTime();

        bestScore.text = "<color=#ffbb00>" + GameManager.Instance.GetBestScore().ToString();
    }
}
