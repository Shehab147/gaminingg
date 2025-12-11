using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEdgeSlide : MonoBehaviour
{
    public float slideSpeedMultiplier = 1f;
    public Vector2 slideDirection = Vector2.right;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (playerRb != null && player != null)
            {
                // Calculate slide speed from falling velocity
                float fallSpeed = Mathf.Abs(playerRb.velocity.y);
                float slideSpeed = fallSpeed * slideSpeedMultiplier;

                // Apply slide
                playerRb.velocity = new Vector2(slideDirection.x * slideSpeed, 0f);
                player.enabled = false;

                StartCoroutine(ReenablePlayer(player));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.enabled = true;
            }
        }
    }

    IEnumerator ReenablePlayer(PlayerController player)
    {
        // Safety measure: re-enable player after 3 seconds if still on platform
        yield return new WaitForSeconds(3f);
        if (player != null)
        {
            player.enabled = true;
        }
    }
}