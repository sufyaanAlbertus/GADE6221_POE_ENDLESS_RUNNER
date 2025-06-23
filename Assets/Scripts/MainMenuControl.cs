using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] public GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject BlurPanel;
    [SerializeField] public GameObject CoinStore;
    [SerializeField] public GameObject ProfileMenu;
    [SerializeField] public GameObject LoginMenu;
    [SerializeField] public GameObject CreateMenu;
    [SerializeField] public GameObject StorePanel;
    [SerializeField] public GameObject HighScorePanel;
    [SerializeField] public GameObject errorMessage;
    [SerializeField] public GameObject errorMessageCreate;


    [SerializeField] public TextMeshProUGUI messageText;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        hudPanel.SetActive(false);
        BlurPanel.SetActive(true);
        CoinStore.SetActive(true);
        ProfileMenu.SetActive(false);
        StorePanel.SetActive(false);
        HighScorePanel.SetActive(false);
        errorMessage.SetActive(false);
        LoginMenu.SetActive(false);
        errorMessageCreate.SetActive(false);
        CreateMenu.SetActive(false);


        // Re-set userId to keep it after scene reload/reset
        ScoreManager.Instance.userId = GlobalUser.userId;

        // Fetch latest total coins again
        if (ScoreManager.Instance.userId != 0)
        {
            ScoreManager.Instance.FetchTotalCoinsFromDB();
        }


        if (backButton != null)
            backButton.interactable = false;
    }

    public void OnPlayPressed()
    {
        if (!Login.isLoggedIn)
        {
            errorMessage.SetActive(true);
            messageText.text = "You must log in first! Click Profile to Login";
            return;
        }


        // Re-set userId to keep it after scene reload/reset
        ScoreManager.Instance.userId = GlobalUser.userId;

        // Fetch latest total coins again
        if (ScoreManager.Instance.userId != 0)
        {
            ScoreManager.Instance.FetchTotalCoinsFromDB();
        }

        GameManager.Instance.StartGame();
        hudPanel.SetActive(true);
        BlurPanel.SetActive(false);
        CoinStore.SetActive(false);

        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        ProfileMenu.SetActive(false);
        StorePanel.SetActive(false);
        HighScorePanel.SetActive(false);
    }

    public void OnOptionsPressed()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        CoinStore.SetActive(false);
        ProfileMenu.SetActive(false);
        StorePanel.SetActive(false);
        HighScorePanel.SetActive(false);

        if (backButton != null)
            backButton.interactable = true;
    }

    public void OnCreateUserPressed()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        CoinStore.SetActive(false);
        ProfileMenu.SetActive(false);
        StorePanel.SetActive(false);
        HighScorePanel.SetActive(false);
        LoginMenu.SetActive(false);
        CreateMenu.SetActive(true);

        if (backButton != null)
            backButton.interactable = true;
    }

    public void OnBackPressed()
    {
        optionsMenu.SetActive(false);
        ProfileMenu.SetActive(false);
        StorePanel.SetActive(false);
        HighScorePanel.SetActive(false);
        LoginMenu.SetActive(false);
        mainMenu.SetActive(true);
        CoinStore.SetActive(true);
        CreateMenu.SetActive(false);

        if (backButton != null)
            backButton.interactable = false;
    }

    public void OnProfilePressed()
    {
        if (Login.isLoggedIn)
        {
            ProfileMenu.SetActive(true);
        }
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        StorePanel.SetActive(false);
        HighScorePanel.SetActive(false);

        if (!Login.isLoggedIn)
        {
            LoginMenu.SetActive(true);
        }


        if (backButton != null)
            backButton.interactable = true;
    }

    public void OnStorePressed()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        ProfileMenu.SetActive(false);
        HighScorePanel.SetActive(false);

        StorePanel.SetActive(true);

        if (backButton != null)
            backButton.interactable = true;
    }

    public void OnHighScorePressed()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        ProfileMenu.SetActive(false);
        StorePanel.SetActive(false);

        HighScorePanel.SetActive(true);

        if (backButton != null)
            backButton.interactable = true;
    }

    public void OnClosePressed()
    {
        errorMessage.SetActive(false);
        errorMessageCreate.SetActive(false);
    }



    public void OnQuitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
