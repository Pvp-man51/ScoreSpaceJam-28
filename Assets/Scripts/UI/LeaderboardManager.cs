using LootLocker.Requests;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    private const int id = 20006;
    private string playerID;

    [Header("Entry")]
    [SerializeField] private GameObject entryDisplayObj;
    [SerializeField] private Transform entryDisplayParent;
    [SerializeField] private int maxLoadedScores = 50;

    private List<GameObject> entryList = new List<GameObject>();

    [Header("FailedToLoadUI")]
    [SerializeField] private GameObject failedToLoadUI;

    [Header("PlayerIDInput")]
    [SerializeField] private TMP_InputField playerInputField;
    [SerializeField] private TMP_Text errorTextPlayerID;

    [Header("UploadScore")]
    [SerializeField] private TMP_Text errorTextUploadScore;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += IsMenu;
    }

    private void IsMenu(Scene arg0, Scene arg1)
    {
       if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
       {
            AudioManager.Instance.Stop("CombatMusic");

            if (PlayerPrefs.HasKey("playerID"))
            {
                playerID = PlayerPrefs.GetString("playerID");
                MenuManager.Instance.OpenMenu("Main");
            }
            else
            {
                MenuManager.Instance.OpenMenu("PlayerInputUI");
            }
       }

       ConnectToLootLocker();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("playerID"))
        {
            playerID = PlayerPrefs.GetString("playerID");
            MenuManager.Instance.OpenMenu("Main");
        }
        else
        {
            MenuManager.Instance.OpenMenu("PlayerInputUI");
        }

        ConnectToLootLocker();
    }

    private void ConnectToLootLocker()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                print("Connected to LootLocker!!");
            }
            else
            {
                Debug.LogWarning("Failed to connect to LootLocker:\n" + response.errorData.message);
            }
        });
    }

    public void ShowScores()
    {
        failedToLoadUI.SetActive(false);

        foreach (var entry in entryList)
        {
            Destroy(entry.gameObject);
        }

        entryList.Clear();

        LootLockerSDKManager.GetScoreList(id.ToString(), maxLoadedScores, (response) =>
        {
            LootLockerLeaderboardMember[] scores = response.items;

            if (response.success)
            {
                for (int i = 0; i < scores.Length; i++)
                {
                    CreateEntryDisplay(scores[i].rank.ToString(), scores[i].player.name, 
                        scores[i].score.ToString(), scores[i].metadata.ToString());
                }
            }
            else
            {
                failedToLoadUI.SetActive(true);

                Debug.LogWarning("Failed: " + response.errorData.message);
            }
        });
    }

    private void CreateEntryDisplay(string rank, string user, string score, string time)
    {
        GameObject entryDisplay = Instantiate(entryDisplayObj.gameObject, entryDisplayParent);
        entryDisplay.GetComponent<Entry>().SetEntry(rank, user, score, time);
        entryList.Add(entryDisplay);
    }

    public void SumbitScore()
    {
        LootLockerSDKManager.SubmitScore(playerID, GameManager.Instance.GetScore(), id.ToString(), Timer.Instance.GetFormatedTime(), (response) =>
        {
            if (response.success)
            {
                print("Submitted to Leaderboard succesfully");
                errorTextUploadScore.text = "<color=green>Uploaded Succesfully!";
            }
            else
            {
                print("Failed: " + response.errorData.message);
                errorTextUploadScore.text = "<color=red>Upload Failed.\nPlease Retry";
            }
        });
    }

    public void SetPlayerID()
    {
        playerID = playerInputField.text;
        LootLockerSDKManager.SetPlayerName(playerID, (response) =>
        {
            if (response.success)
            {
                MenuManager.Instance.OpenMenu("Main");
                PlayerPrefs.SetString("playerID", playerID);
            }
            else
            {
                errorTextPlayerID.text = "<color=red>Name coulden't be set.\nPlease Retry";
            }
        });
    }

    public void PlayOffline()
    {
        MenuManager.Instance.OpenMenu("Main");
        GameManager.Instance.PlayOffline();
    }
}
