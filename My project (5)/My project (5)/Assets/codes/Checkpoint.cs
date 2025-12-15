using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject activatedEffect;
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isActivated) return;

        isActivated = true;

        // Update the player's checkpoint (this sets lastCheckpoint in PlayerController)
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.SetCheckpoint(transform.position);
            Debug.Log("Checkpoint set to: " + transform.position);
        }

        if (activatedEffect != null)
            Instantiate(activatedEffect, transform.position, transform.rotation);
    }
}

