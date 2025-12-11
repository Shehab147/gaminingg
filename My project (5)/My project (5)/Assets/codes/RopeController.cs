using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [Header("Rope Points")]
    public Transform startPoint;
    public Transform endPoint;
    public float slideSpeed = 5f;

    [Header("Player Settings")]
    public Transform player;
    public float playerMountOffset = 0.5f;

    [Header("Visual Feedback")]
    public GameObject interactPrompt;
    public ParticleSystem slideParticles;

    private bool playerInRange = false;
    private bool isSliding = false;
    private Vector3 slideDirection;
    private float slideProgress = 0f;
    private PlayerController playerController;
    private Rigidbody2D playerRb;

    void Start()
    {
        // Calculate slide direction
        if (startPoint != null && endPoint != null)
        {
            slideDirection = (endPoint.position - startPoint.position).normalized;
        }

        // Get player components
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // Check for interaction input
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isSliding)
        {
            StartSliding();
        }

        // Handle sliding movement
        if (isSliding)
        {
            SlidePlayer();
        }

        // Update interact prompt
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(playerInRange && !isSliding);
        }
    }

    void StartSliding()
    {
        if (player == null || startPoint == null || endPoint == null) return;

        isSliding = true;
        slideProgress = 0f;

        // Disable player control
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Disable player physics
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.isKinematic = true;
        }

        // Position player at start point with offset
        Vector3 mountPosition = startPoint.position + (slideDirection * playerMountOffset);
        player.position = mountPosition;

        // Play slide particles
        if (slideParticles != null)
        {
            slideParticles.Play();
        }

        Debug.Log("Player started sliding on rope!");
    }

    void SlidePlayer()
    {
        if (player == null || startPoint == null || endPoint == null) return;

        // Update slide progress
        slideProgress += slideSpeed * Time.deltaTime;
        slideProgress = Mathf.Clamp01(slideProgress);

        // Calculate new position along the rope
        Vector3 newPosition = Vector3.Lerp(startPoint.position, endPoint.position, slideProgress);
        player.position = newPosition;

        // Rotate player to face slide direction
        float angle = Mathf.Atan2(slideDirection.y, slideDirection.x) * Mathf.Rad2Deg;
        player.rotation = Quaternion.Euler(0, 0, angle);

        // Check if reached end point
        if (slideProgress >= 1f)
        {
            StopSliding();
        }
    }

    void StopSliding()
    {
        isSliding = false;

        // Re-enable player control
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Re-enable player physics
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
        }

        // Reset player rotation
        if (player != null)
        {
            player.rotation = Quaternion.identity;
        }

        // Stop slide particles
        if (slideParticles != null)
        {
            slideParticles.Stop();
        }

        Debug.Log("Player finished sliding!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // Visual debugging in Scene view
    void OnDrawGizmosSelected()
    {
        if (startPoint != null && endPoint != null)
        {
            // Draw rope line
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPoint.position, endPoint.position);

            // Draw points
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPoint.position, 0.3f);
            Gizmos.DrawWireSphere(endPoint.position, 0.3f);

            // Draw slide direction
            Gizmos.color = Color.red;
            Vector3 direction = (endPoint.position - startPoint.position).normalized;
            Gizmos.DrawRay(startPoint.position, direction * 2f);
        }
    }
}