using System.Collections;
using UnityEngine;

public class SpeedBoostPickUp : MonoBehaviour
{
    [SerializeField] private AudioSource boostSound;
    [SerializeField] private float boostAmount = 2f;
    [SerializeField] private float boostDuration = 4f;

    private static bool boostActive = false; // shared flag so player can't pick up another boost while active

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !boostActive)
        {
            if (boostSound != null)
                boostSound.Play();

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.StartCoroutine(ApplySpeedBoost(playerMovement));
            }

            gameObject.SetActive(false); // deactivate this pickup
        }
    }

    private IEnumerator ApplySpeedBoost(PlayerMovement playerMovement)
    {
        boostActive = true; // block further boosts
        float originalSpeed = playerMovement.moveSpeed;

        // Apply boost (+2)
        playerMovement.moveSpeed += boostAmount;
        Debug.Log("Speed Boost Activated! Current speed: " + playerMovement.moveSpeed);

        yield return new WaitForSeconds(boostDuration);

        // Reset back to original
        playerMovement.moveSpeed = originalSpeed;
        Debug.Log("Speed Boost Ended. Reset to speed: " + playerMovement.moveSpeed);

        boostActive = false; // allow next pickup
    }
}