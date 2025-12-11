using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlyingEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float pauseTime = 1f;

    [Header("Combat Settings")]
    public float damage = 30f;
    public float attackCooldown = 1f; // Prevent rapid damage

    [Header("Collision Settings")]
    public bool useTrigger = true; // Use trigger collider for damage
    public bool useCollision = true; // Use collision for damage

    private Transform currentTarget;
    private bool isMoving = true;
    private SpriteRenderer spriteRenderer;
    private bool canDamage = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTarget = pointB; // Start by moving to point B

        Debug.Log("Flying Enemy initialized. Damage: " + damage);
    }

    void Update()
    {
        if (isMoving && currentTarget != null)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        // Move towards current target
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        // Update facing direction
        UpdateFacingDirection();

        // Check if reached target
        float distance = Vector2.Distance(transform.position, currentTarget.position);
        if (distance < 0.1f)
        {
            StartCoroutine(SwitchTargetAfterPause());
        }
    }

    IEnumerator SwitchTargetAfterPause()
    {
        isMoving = false;
        yield return new WaitForSeconds(pauseTime);

        // Switch target
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        isMoving = true;

        Debug.Log($"Switched to: {(currentTarget == pointA ? "Point A" : "Point B")}");
    }

    void UpdateFacingDirection()
    {
        if (spriteRenderer != null)
        {
            // Face the direction of movement
            if (currentTarget == pointA)
            {
                spriteRenderer.flipX = true; // Face left
            }
            else
            {
                spriteRenderer.flipX = false; // Face right
            }
        }
    }

    // TRIGGER-BASED DAMAGE
    void OnTriggerEnter2D(Collider2D other)
    {
        if (useTrigger && other.CompareTag("Player") && canDamage)
        {
            DealDamage(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (useTrigger && other.CompareTag("Player") && canDamage)
        {
            DealDamage(other.gameObject);
        }
    }

    // COLLISION-BASED DAMAGE
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (useCollision && collision.gameObject.CompareTag("Player") && canDamage)
        {
            DealDamage(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (useCollision && collision.gameObject.CompareTag("Player") && canDamage)
        {
            DealDamage(collision.gameObject);
        }
    }

    void DealDamage(GameObject playerObject)
    {
        PlayerController player = playerObject.GetComponent<PlayerController>();
        if (player != null && !player.IsDead && canDamage)
        {
            Debug.Log("Flying enemy dealing " + damage + " damage to player!");
            player.TakeDamage(damage);
            StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(attackCooldown);
        canDamage = true;
    }

    void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.3f);
            Gizmos.DrawWireSphere(pointB.position, 0.3f);
        }
    }


}