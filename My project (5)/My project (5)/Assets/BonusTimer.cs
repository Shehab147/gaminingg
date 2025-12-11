using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTimer : MonoBehaviour
{

    public float bonusTime = 10f;      // how much time to add
    public AudioClip timeSound;        // optional sound

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add time to the timer (like coins add to score)
            LevelTimer timer = FindObjectOfType<LevelTimer>();
            if (timer != null)
            {
                timer.AddTime(bonusTime);
            }

            // Play sound (like coin sound in Lab 7)
            if (timeSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusicSFX(timeSound);
            }

            // Destroy the pickup
            Destroy(gameObject);
        }
    }
}


