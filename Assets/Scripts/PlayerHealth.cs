using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 5;
    public int currentLives = 3;

    [SerializeField] private Image[] lifeIcons;
    [SerializeField] private Sprite fullLifeSprite;
    [SerializeField] private Sprite emptyLifeSprite;

    public bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 2f;

    private Renderer playerRenderer;

    private void Start()
    {
        playerRenderer = GetComponentInChildren<Renderer>();
        UpdateLifeUI();
    }

    public void AddLife(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
        UpdateLifeUI();
    }

    public void LoseLife(int amount)
    {
        if (isInvincible)
            return;

        currentLives = Mathf.Max(currentLives - amount, 0);
        UpdateLifeUI();

        if (currentLives > 0)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void UpdateLifeUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < currentLives)
            {
                lifeIcons[i].sprite = fullLifeSprite;
                lifeIcons[i].enabled = true;
            }
            else
            {
                lifeIcons[i].sprite = emptyLifeSprite;
                lifeIcons[i].enabled = true;
            }
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        
        isInvincible = true;

        float blinkTime = 0.2f;
        float timer = 0f;

        while (timer < invincibilityDuration)
        {
            if (playerRenderer != null)
                playerRenderer.enabled = !playerRenderer.enabled;

            yield return new WaitForSeconds(blinkTime);
            timer += blinkTime;
        }

       

        isInvincible = false;
    }
}