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
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            // Check if it's the player
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    Debug.Log($"Arrow hit player for {damage} damage!");
                }
            }

            // Play hit effect
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // Destroy projectile
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collisions with non-trigger colliders
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
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
}