using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float detectionRange = 8f;

    [Header("Visual Settings")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private Transform player;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private bool isChasing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();

        if (rb != null)
        {
            // Freeze Y position completely to prevent any jumping/falling
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!isChasing)
            {
                StartChasing();
            }
            ChasePlayer();
            
        }
        else if (isChasing)
        {
            StopChasing();
        }

        UpdateFacingDirection();
    }

    void StartChasing()
    {
        isChasing = true;
        if (animator != null)
        {
            animator.SetBool("IsChasing", true);
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (rb != null)
        {
            // Only move horizontally - Y is frozen by constraints
            rb.velocity = new Vector2(direction.x * moveSpeed, 0f);
        }
        else
        {
            // Move only on X axis
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void StopChasing()
    {
        isChasing = false;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        if (animator != null)
        {
            animator.SetBool("IsChasing", false);
        }
    }

    void UpdateFacingDirection()
    {
        if (spriteRenderer != null && player != null)
        {
            spriteRenderer.flipX = player.position.x < transform.position.x;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        if (playerController != null && !playerController.IsDead)
        {
            playerController.Die();
        }
    }
}