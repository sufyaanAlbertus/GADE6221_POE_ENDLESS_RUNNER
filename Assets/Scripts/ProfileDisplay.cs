using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileDisplay : MonoBehaviour
{
    public string[] UserProfile;

    public TMP_Text usernameDisplay;
    public TMP_Text emailDisplay;
    public TMP_Text coinsDisplay;

    string getProfileURL = "http://officedash.wuaze.com/GetProfile.php";

    IEnumerator Start()
    {
        yield return FetchProfileData();
    }

    public void RefreshProfile()
    {
        if (Login.isLoggedIn && GlobalUser.userId != 0)
        {
            StartCoroutine(FetchProfileData());
        }
        else
        {
            Debug.LogWarning("Cannot refresh profile — user not logged in.");
        }
    }

    IEnumerator FetchProfileData()
    {
        if (!Login.isLoggedIn || GlobalUser.userId == 0)
        {
            Debug.LogWarning("Player not logged in — skipping profile fetch.");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("user_id", GlobalUser.userId);

        WWW ProfileData = new WWW(getProfileURL, form);
        yield return ProfileData;

        if (string.IsNullOrEmpty(ProfileData.error))
        {
            string ProfileDataString = ProfileData.text;
            Debug.Log("Raw Profile Data:\n" + ProfileDataString);

            UserProfile = ProfileDataString.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string data in UserProfile)
            {
                Debug.Log("Profile Item : " + data);
            }

            if (UserProfile.Length >= 1)
            {
                string username = GetDataValue(UserProfile[0], "Username");
                string email = GetDataValue(UserProfile[0], "email");
                string coins = GetDataValue(UserProfile[0], "coins");

                if (usernameDisplay != null) usernameDisplay.text = username;
                if (emailDisplay != null) emailDisplay.text = email;
                if (coinsDisplay != null) coinsDisplay.text = coins;
            }
        }
        else
        {
            Debug.LogError("Error loading GetProfile.php: " + ProfileData.error);
        }
    }

    // Extract value by index keyword
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