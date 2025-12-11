using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWalkingEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float pauseTime = 1f;
    public float damage = 30f;

    private Transform currentTarget;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isMoving = true;
    private bool isWaiting = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointB;
    }

    void Update()
    {
        if (isMoving && !isWaiting)
        {
            MoveBetweenPoints();
        }
        UpdateFacingDirection(); // Always update facing direction
    }

    void MoveBetweenPoints()
    {
        // Move towards current target
        Vector2 direction = (currentTarget.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Check if reached current target
        float distance = Vector2.Distance(transform.position, currentTarget.position);
        if (distance < 0.5f)
        {
            StartCoroutine(PauseAndSwitch());
        }
    }

    IEnumerator PauseAndSwitch()
    {
        isWaiting = true;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(pauseTime);

        // Switch target
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        isWaiting = false;
    }

    void UpdateFacingDirection()
    {
        if (spriteRenderer != null)
        {
            // Face based on actual movement velocity
            if (rb.velocity.x > 0.1f) // Moving right
            {
                spriteRenderer.flipX = false;
            }
            else if (rb.velocity.x < -0.1f) // Moving left
            {
                spriteRenderer.flipX = true;
            }
            // If velocity is near zero, maintain current direction
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}