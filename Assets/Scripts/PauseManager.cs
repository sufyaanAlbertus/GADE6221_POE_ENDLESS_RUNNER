using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject BlurPanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Image pauseButtonImage;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
   

    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;

    private bool isPaused = false;

    void Start()
    {
        pauseButton.onClick.AddListener(TogglePause);
       
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        restartButton.onClick.AddListener(RestartGame);

        pauseMenuPanel.SetActive(false); // Hide pause menu at start
        BlurPanel.SetActive(false);
        UpdateButtonText();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        pauseMenuPanel.SetActive(isPaused);
        BlurPanel.SetActive(isPaused);
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        pauseButtonImage.sprite = isPaused ? playSprite : pauseSprite;
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Make sure time resumes
        SceneManager.LoadScene("GameMode"); // Replace with your actual main menu scene name
    }

    private void RestartGame()
    {
        // Reset MasterInfo values
        MasterInfo.Instance.ResetAll(); 

  
        // Resume time and reload the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
