using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public AudioClip heartSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add one life (like Lab 7)
            PlayerController.currentLives++;
            
            Debug.Log("currentLives: " + PlayerController.currentLives);

            // Play sound
            if (heartSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusicSFX(heartSound);
            }

            // Destroy heart
            Destroy(gameObject);
        }
    }
}

