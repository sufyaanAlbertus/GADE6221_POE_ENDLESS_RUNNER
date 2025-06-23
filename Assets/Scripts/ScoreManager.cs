using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public int currentCoinsInGame = 0;
    public int totalCoins = 0;
    public int userId = 0;  // Set this after login

    public TMP_Text scoreText;
    public TMP_Text coinText;

    private string saveScoreURL = "http://officedash.wuaze.com/InsertScore.php";
    private string updateCoinsURL = "http://officedash.wuaze.com/UpdateCoins.php";
    private string getCoinsURL = "http://officedash.wuaze.com/GetCoins.php";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FetchTotalCoinsFromDB()
    {
        if (userId == 0)
        {
            Debug.LogWarning("User ID not set! Cannot fetch coins.");
            return;
        }

        StartCoroutine(FetchCoinsCoroutine());
    }

    IEnumerator FetchCoinsCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);

        WWW www = new WWW(getCoinsURL, form);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Coins fetched: " + www.text);
            int.TryParse(www.text, out totalCoins);
        }
        else
        {
            Debug.LogError("Error fetching coins: " + www.error);
        }
    }

    public void ResetRunData()
    {
        currentScore = 0;
        currentCoinsInGame = 0;
        UpdateUI();
    }

    public void UpdateFromMasterInfo()
    {
        currentScore = MasterInfo.Instance.CurrentScore;
        currentCoinsInGame = MasterInfo.coinCount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;

        if (coinText != null)
            coinText.text = "Coins: " + currentCoinsInGame;
    }

    public void SaveScoreAndCoins()
    {
        if (userId == 0)
        {
            Debug.LogWarning("User ID not set! Cannot save score.");
            return;
        }

        UpdateFromMasterInfo();

        StartCoroutine(SaveScoreToDatabase());
        StartCoroutine(UpdateTotalCoins());
    }

    IEnumerator SaveScoreToDatabase()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);
        form.AddField("score", currentScore);
        form.AddField("coincount", currentCoinsInGame);

        WWW www = new WWW(saveScoreURL, form);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Score saved! Response: " + www.text);
        }
        else
        {
            Debug.LogError("Error saving score: " + www.error);
        }
    }

    IEnumerator UpdateTotalCoins()
    {
        int newTotalCoins = totalCoins + currentCoinsInGame;

        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);
        form.AddField("total_coins", newTotalCoins);

        WWW www = new WWW(updateCoinsURL, form);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Coins updated! Response: " + www.text);
            totalCoins = newTotalCoins;
        }
        else
        {
            Debug.LogError("Error updating coins: " + www.error);
        }
    }
}