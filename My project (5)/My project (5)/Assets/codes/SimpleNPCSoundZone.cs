using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNPCSoundZone : MonoBehaviour
{
    public AudioClip npcSound;
    public float volume = 1f;
    public bool playOnlyOnce = true;

    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasPlayed || !playOnlyOnce)
            {
                PlaySound();
                hasPlayed = true;
            }
        }
    }

    void PlaySound()
    {
        if (npcSound != null)
        {
            audioSource.PlayOneShot(npcSound);
            Debug.Log("NPC sound played");
        }
    }
}