using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEdgeSlide : MonoBehaviour
{
    public float slideSpeedMultiplier = 1f;
    public Vector2 slideDirection = Vector2.right;

    private void OnCollisionStay2D(Collision2D collision)
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

                // Apply horizontal slide while keeping player grounded
                playerRb.velocity = new Vector2(slideDirection.x * slideSpeed, playerRb.velocity.y);
                
                // Disable jumping but allow walking
                if (player != null)
                {
                    player.canJump = false;
                }
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
                player.canJump = true;
            }
        }
    }
}