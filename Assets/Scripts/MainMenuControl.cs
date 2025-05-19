using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject hudPanel;

    private void Start()
    {
        // Ensure only main menu shows at start
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        hudPanel.SetActive(false);

        if (backButton != null)
            backButton.interactable = false; // Disable Back initially
    }

    public void OnPlayPressed()
    {
        GameManager.Instance.StartGame();
        hudPanel.SetActive(true);

        if (mainMenu != null) mainMenu.SetActive(false);
        if (optionsMenu != null) optionsMenu.SetActive(false);
    }

    public void OnOptionsPressed()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (optionsMenu != null) optionsMenu.SetActive(true);

        if (backButton != null)
            backButton.interactable = true; // Enable Back button when options are shown
    }

    public void OnBackPressed()
    {
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (mainMenu != null) mainMenu.SetActive(true);

        if (backButton != null)
            backButton.interactable = false; // Disable again after going back
    }

    public void OnQuitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
