using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    string loginURL = "http://officedash.wuaze.com/Login.php";

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public GameObject ErrorPanel;  // Panel for error messages
    public TextMeshProUGUI messageText;  // Text to show errors or success


    public MainMenuControl mainMenuControl;

    public static bool isLoggedIn = false;

    public void OnLoginButton()
    {
        
        StartCoroutine(LoginToUser(usernameInput.text, passwordInput.text));
    }

    public void OnBackButton()
    {
        mainMenuControl.LoginMenu.SetActive(false);
    }


    IEnumerator LoginToUser(string Username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("UsernamePost", Username);
        form.AddField("passwordPost", password);

        WWW www = new WWW(loginURL, form);
        yield return www;

        Debug.Log(www.text);

        if (www.text.StartsWith("Login success|"))
        {
            string[] splitResponse = www.text.Split('|');
            if (splitResponse.Length > 1 && int.TryParse(splitResponse[1], out int userId))
            {
                GlobalUser.userId = userId;  // store globally
                ScoreManager.Instance.userId = userId;
                ScoreManager.Instance.FetchTotalCoinsFromDB();

                isLoggedIn = true;
                mainMenuControl.LoginMenu.SetActive(false);
                ErrorPanel.SetActive(true);
                messageText.text = "Login successful!";
            }
        else
            {
                // Failed to parse user ID - treat as failure
                isLoggedIn = false;
                ErrorPanel.SetActive(true);
                messageText.text = "Login succeeded but failed to get user ID.";
            }
        }
        else
        {
            isLoggedIn = false;
            if (ErrorPanel != null)
                ErrorPanel.SetActive(true);
            messageText.text = www.text;
        }
    }
}