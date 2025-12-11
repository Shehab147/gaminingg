using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDisappearingPlatform : MonoBehaviour
{
    public float disappearDelay = 0.5f;
    public float respawnTime = 2f; // Added respawn time

    private bool hasPlayer = false;
    private Collider2D platformCollider;
    private SpriteRenderer platformRenderer;
    private bool isActive = true;

    void Start()
    {
        // Cache components for better performance
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasPlayer && isActive)
        {
            // Simple check - if player is above platform
            if (collision.transform.position.y > transform.position.y)
            {
                hasPlayer = true;
                StartCoroutine(DisappearAndRespawn());
            }
        }
    }

    IEnumerator DisappearAndRespawn()
    {
        // Wait for disappear delay
        yield return new WaitForSeconds(disappearDelay);

        // Disable the platform
        isActive = false;
        if (platformCollider != null)
            platformCollider.enabled = false;
        if (platformRenderer != null)
            platformRenderer.enabled = false;

        Debug.Log("Platform disappeared!");

        // Wait for respawn time
        yield return new WaitForSeconds(respawnTime);

        // Re-enable the platform
        isActive = true;
        if (platformCollider != null)
            platformCollider.enabled = true;
        if (platformRenderer != null)
            platformRenderer.enabled = true;

        hasPlayer = false;

        Debug.Log("Platform respawned!");
    }

    // Optional: Visual feedback before disappearing
    void Update()
    {
        if (hasPlayer && isActive)
        {
            // Optional: Add visual warning like platform shaking or color change
            // Example: platformRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 10f, 1f));
        }
    }
}