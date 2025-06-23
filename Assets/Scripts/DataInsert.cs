using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataInsert : MonoBehaviour
{

    string createUserURL = "http://officedash.wuaze.com/InsertUser.php";

    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI messageText;  // to display result or errors
    public MainMenuControl mainMenuControl;


    // Create User Button click
    public void OnCreateUserButton()
    {
        string username = usernameInput.text.Trim();
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        // Check if fields are empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            mainMenuControl.errorMessageCreate.SetActive(true);
            messageText.text = "All fields must be filled!";
            return;
        }

        // Check length validation
        if (password.Length > 10)
        {
            mainMenuControl.errorMessageCreate.SetActive(true);
            messageText.text = "Password must be less than 10 characters!";
            return;
        }

        if (username.Length > 40)
        {
            mainMenuControl.errorMessageCreate.SetActive(true);
            messageText.text = "Username must be less than 40 characters!";
            return;
        }

        if (email.Length > 40)
        {
            mainMenuControl.errorMessageCreate.SetActive(true);
            messageText.text = "Email must be less than 40 characters!";
            return;
        }

        // ? If we reach here ? all checks passed ? start create user
        StartCoroutine(CreateUser(username, email, password));
    }


    IEnumerator CreateUser(string Username, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("UsernamePost", Username);
        form.AddField("emailPost", email);
        form.AddField("passwordPost", password);

        WWW www = new WWW(createUserURL, form);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            messageText.text = www.text;
            messageText.text = "Created User, Click Profile And Login!!";
            mainMenuControl.errorMessageCreate.SetActive(true);
            mainMenuControl.CoinStore.SetActive(true);
            mainMenuControl.CreateMenu.SetActive(false);
            mainMenuControl.mainMenu.SetActive(true);

            Debug.Log("Create User Result: " + www.text);
        }
        else
        {
            messageText.text = "Error: " + www.error;
            Debug.LogError("Create User Error: " + www.error);
        }
    }
}
