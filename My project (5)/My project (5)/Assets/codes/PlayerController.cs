using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;        
using TMPro;//added for heart bonus

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Health and lives system
    private float currentHealth = 100f;
    public static int currentLives = 3;
    private bool isDead = false;
    private Vector3 lastCheckpoint;

    // Store original scale to prevent shrinking
    private Vector3 originalScale;

    // Properties for other scripts to access
    public float CurrentHealth => currentHealth;
    public int CurrentLives => currentLives;
    public bool IsDead => isDead;

    // Animation state tracking
    private bool isHurt = false;
    private bool isDying = false;

    //UI heart bonus
    public TMP_Text livesText;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original scale to prevent shrinking
        originalScale = transform.localScale;
        lastCheckpoint = transform.position; // Set initial checkpoint to starting position

        Debug.Log($"Player original scale: {originalScale}");
    }

    void Update()
    {
        if (isDead || isDying)
        {
            // Update UI even when dead so it shows correct count
            if (livesText != null)
            {
                livesText.text = "  " + currentLives;
            }
            return;
        }
                if (livesText != null)
        {
            livesText.text = "  " + currentLives;
        }


        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip character based on movement direction
        UpdateFacingDirection(moveInput);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Only update movement animations if not hurt
        if (!isHurt)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", rb.velocity.y);
        }
    }

    void UpdateFacingDirection(float moveInput)
    {
        if (moveInput > 0.1f) // Moving right
        {
            // Use original scale but flip X to positive
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (moveInput < -0.1f) // Moving left
        {
            // Use original scale but flip X to negative
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        // If moveInput is 0, maintain current facing direction
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isDying) return;

        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}");

        // Start hurt animation
        StartCoroutine(HurtAnimation());

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HurtAnimation()
    {
        isHurt = true;
        animator.SetTrigger("Hurt");

        // Wait for hurt animation to play (adjust time as needed)
        yield return new WaitForSeconds(0.5f);

        // Reset hurt state
        animator.ResetTrigger("Hurt");
        isHurt = false;

        // Reset to idle state
        animator.Play("Idle", -1, 0f);
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, 100f);
        Debug.Log($"Player healed. Health: {currentHealth}");
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpoint = checkpointPosition;
        Debug.Log($"Checkpoint set at: {checkpointPosition}");
    }

    public void Die()
    {
        if (isDead || isDying) return;

        isDying = true;
        isDead = true;
        currentLives--;

        Debug.Log($"Player died! Lives remaining: {currentLives}");

        // Play death animation
        animator.SetTrigger("Die");

        // Disable movement and physics
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;

        // Start respawn process
        StartCoroutine(RespawnAfterDeath());
    }

    private IEnumerator RespawnAfterDeath()
    {
        // Wait for death animation to play (2 seconds)
        yield return new WaitForSeconds(2f);

        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    private void Respawn()
    {
        // Reset position to last checkpoint
        transform.position = lastCheckpoint;

        // Reset health
        currentHealth = 100f;

        // Reset animation states
        animator.ResetTrigger("Die");
        animator.Play("Idle", -1, 0f); // Force idle animation

        // Re-enable components
        rb.isKinematic = false;
        GetComponent<Collider2D>().enabled = true;
        enabled = true;
        isDead = false;
        isDying = false;

        // Reset to original scale facing right
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        Debug.Log($"Player respawned at checkpoint. Health: {currentHealth}, Lives: {currentLives}");
    }

    private void GameOver()
    {
        Debug.Log("Game Over! No lives remaining.");
        // You can add game over screen logic here
        // For now, just reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    // Called by PuzzleManager when player makes too many mistakes
    public void DieFromPuzzle()
    {
        TakeDamage(100f); // Instant death
    }

    // Public method to get facing direction (useful for other scripts)
    public bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    // Public method to get facing direction as vector
    public Vector2 GetFacingDirection()
    {
        return IsFacingRight() ? Vector2.right : Vector2.left;
    }
}