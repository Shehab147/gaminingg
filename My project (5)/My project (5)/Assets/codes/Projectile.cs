using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int damage = 20;
    public float lifetime = 3f;
    public GameObject hitEffect;

    [Header("Collision Settings")]
    public LayerMask collisionLayers = ~0; // Collide with everything by default
    public bool destroyOnHit = true;

    private void Start()
    {
        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we should collide with this object
        if (((1 << other.gameObject.layer) & collisionLayers) == 0)
            return;

        // ===== PLAYER HIT =====
        if (other.CompareTag("Player"))
        {
            // 🔊 Play hit sound from player
            AudioSource hitSound = other.GetComponent<AudioSource>();
            if (hitSound != null)
            {
                hitSound.Play();
            }

            // Deal damage
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log($"Arrow hit player for {damage} damage!");
            }
        }

        // ===== HIT EFFECT =====
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // ===== DESTROY PROJECTILE =====
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collisions with non-trigger colliders
        if (((1 << collision.gameObject.layer) & collisionLayers) == 0)
            return;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }
}
