using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ExtraLifePickup : MonoBehaviour
{
    [SerializeField] private AudioSource pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play sound
            if (pickupSound != null)
                pickupSound.Play();

            // Add 1 life to player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddLife(1);
                Debug.Log("Extra life added!");
            }

            // Disable pickup
            gameObject.SetActive(false);
        }
    }
}
