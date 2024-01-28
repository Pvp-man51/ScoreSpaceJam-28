using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText, usernameText, scoreText, timeText;

    public void SetEntry(string rank, string user, string score, string time)
    {
        rankText.text = rank;
        usernameText.text = user;
        scoreText.text = score;
        timeText.text = time;
    }
}
