using System.Collections;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] private Material DeathTranMat;
    [SerializeField] private float TransitionTime = 1f;

    public Canvas Canvas;

    private const string _tPropertyname = "_Progress";

    private void Start()
    {
        DeathTranMat.SetFloat(_tPropertyname, 1f); // Reset Death Transition
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
}
