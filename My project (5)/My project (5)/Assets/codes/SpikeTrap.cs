using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spike Settings")]
    public float damage = 100f; // Instant death
    public bool isActive = true;
    public float resetTime = 2f; // Time before trap becomes active again

    [Header("Visual Effects")]
    public GameObject bloodEffect;
    public AudioClip spikeSound;

    private Animator animator;
    private AudioSource audioSource;
    private bool canDamage = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive && canDamage)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !player.IsDead)
            {
                // Play spike animation if available
                if (animator != null)
                {
                    animator.SetTrigger("Activate");
                }

                // Play sound
                if (spikeSound != null)
                {
                    audioSource.PlayOneShot(spikeSound);
                }

                // Show blood effect
                if (bloodEffect != null)
                {
                    Instantiate(bloodEffect, other.transform.position, Quaternion.identity);
                }

                // Damage player
                player.TakeDamage(damage);

                // Temporary disable damage
                StartCoroutine(DisableTemporarily());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive && canDamage)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null && !player.IsDead)
            {
                // Play spike animation if available
                if (animator != null)
                {
                    animator.SetTrigger("Activate");
                }

                // Play sound
                if (spikeSound != null)
                {
                    audioSource.PlayOneShot(spikeSound);
                }

                // Show blood effect
                if (bloodEffect != null)
                {
                    Instantiate(bloodEffect, collision.transform.position, Quaternion.identity);
                }

                // Damage player
                player.TakeDamage(damage);

                // Temporary disable damage
                StartCoroutine(DisableTemporarily());
            }
        }
    }

    IEnumerator DisableTemporarily()
    {
        canDamage = false;
        yield return new WaitForSeconds(resetTime);
        canDamage = true;
    }

    // Method to activate/deactivate trap from other scripts
    public void SetActive(bool active)
    {
        isActive = active;
    }
}