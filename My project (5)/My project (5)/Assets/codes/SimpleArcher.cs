using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleArcher : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 8f;
    public float attackCooldown = 2f;
    public float projectileSpeed = 10f;
    public int damage = 20;

    [Header("Projectile")]
    public GameObject arrowPrefab;
    public Transform firePoint;

    private Transform player;
    private bool canAttack = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            // Face the player
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = player.position.x < transform.position.x;
            }

            Attack();
        }
    }

    void Attack()
    {
        canAttack = false;

        // Shoot arrow
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

            Vector2 direction = (player.position - firePoint.position).normalized;

            // Rotate arrow
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Set velocity
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
            {
                arrowRb.velocity = direction * projectileSpeed;
            }

            // Set damage
            Projectile arrowProjectile = arrow.GetComponent<Projectile>();
            if (arrowProjectile != null)
            {
                arrowProjectile.damage = damage;
            }
        }

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}