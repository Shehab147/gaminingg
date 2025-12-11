using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject activatedEffect;
    private bool isActivated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        isActivated = true;

        // Set this as the current checkpoint
        LevelManager.Instance.SetCheckpoint(transform);

        // Show activated effect
        if (activatedEffect != null)
        {
            Instantiate(activatedEffect, transform.position, transform.rotation);
        }

        Debug.Log("Checkpoint activated!");
    }
}