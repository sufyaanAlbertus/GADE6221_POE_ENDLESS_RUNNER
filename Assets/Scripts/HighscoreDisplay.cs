using System.Collections;
using TMPro;
using UnityEngine;

public class HighscoreDisplay : MonoBehaviour
{
    public string[] Scores;

    public TMP_Text highscoreText;


    void Start()
    {
        // Load highscores when scene starts
        LoadHighscores();
    }

    public void LoadHighscores()
    {
        StartCoroutine(LoadHighscoresCoroutine());
    }

    IEnumerator LoadHighscoresCoroutine()
    {
        WWW UsersData = new WWW("http://officedash.wuaze.com/GetHighscore.php");
        yield return UsersData;

        if (string.IsNullOrEmpty(UsersData.error))
        {
            string hsDataString = UsersData.text;
            Debug.Log("Raw Highscore Data:\n" + hsDataString);

            // Split items by semicolon (;)
            Scores = hsDataString.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);

            // Display in UI
            string display = "TOP SCORES\n";
            int rank = 1;

            foreach (string score in Scores)
            {
                Debug.Log("Score Entry: " + score);

                string username = GetDataValue(score, "Username");
                string scoreValue = GetDataValue(score, "Score");

                display += rank + ". " + username + " - " + scoreValue + "\n";
                rank++;
            }

            if (highscoreText != null)
                highscoreText.text = display;
        }
        else
        {
            Debug.LogError("Error loading highscores: " + UsersData.error);
            if (highscoreText != null)
                highscoreText.text = "Failed to load highscores!";
        }
    }

    // Extract value by key
    string GetDataValue(string data, string key)
    {
        string[] fields = data.Split('|');
        foreach (var field in fields)
        {
            var trimmed = field.Trim();
            if (trimmed.StartsWith(key + ":"))
            {
                return trimmed.Substring((key + ":").Length).Trim();
            }
        }
        return "";
    }
}
