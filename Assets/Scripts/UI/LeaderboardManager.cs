using LootLocker.Requests;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        LootLockerSDKManager.StartGuestSession(playerID, (response) =>
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
        //failedToLoadUI.SetActive(false)

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
                    CreateEntryDisplay(scores[i].rank.ToString(), scores[i].player.public_uid, 
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
            }
            else
            {
                print("Failed: " + response.errorData.message);
            }
        });
    }

    public void SetPlayerID()
    {
        playerID = playerInputField.text;
        PlayerPrefs.SetString("playerID", playerID);
        MenuManager.Instance.OpenMenu("Main");
        ConnectToLootLocker();
    }
}
