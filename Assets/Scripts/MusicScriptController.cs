using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class MusicScriptController : MonoBehaviour
{
    public AudioSource musicSource;  // Your background music AudioSource
    public Slider volumeSlider;      // UI Slider (min 0, max 1)
    public Toggle muteToggle;        // Toggle to mute
    public Image toggleImage;        // Image inside the toggle to change sprite
    public Sprite volumeOnSprite;    // Sprite when not muted
    public Sprite volumeOffSprite;   // Sprite when muted

    private const string volumePrefKey = "MusicVolume";
    private const string mutePrefKey = "MusicMuted";

    void Start()
    {
        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat(volumePrefKey, 1f);
        bool isMuted = PlayerPrefs.GetInt(mutePrefKey, 0) == 1;

        musicSource.volume = savedVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (muteToggle != null)
        {
            muteToggle.isOn = isMuted;
            muteToggle.onValueChanged.AddListener(SetMute);
        }

        ApplyMute(isMuted);
        UpdateToggleSprite(isMuted);
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(volumePrefKey, volume);
        PlayerPrefs.Save();

        // If volume > 0 and muted, unmute
        if (muteToggle != null && muteToggle.isOn && volume > 0f)
        {
            muteToggle.isOn = false;
        }
    }

    public void SetMute(bool mute)
    {
        ApplyMute(mute);
        PlayerPrefs.SetInt(mutePrefKey, mute ? 1 : 0);
        PlayerPrefs.Save();

        UpdateToggleSprite(mute);
    }

    private void ApplyMute(bool mute)
    {
        if (mute)
        {
            musicSource.volume = 0f;
        }
        else
        {
            float volume = PlayerPrefs.GetFloat(volumePrefKey, 1f);
            musicSource.volume = volume;
        }
    }

    private void UpdateToggleSprite(bool isMuted)
    {
        if (toggleImage != null)
        {
            toggleImage.sprite = isMuted ? volumeOffSprite : volumeOnSprite;
        }
    }
}