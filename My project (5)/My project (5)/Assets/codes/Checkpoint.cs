using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject activatedEffect;
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            // update the LevelManager's currentCheckpoint
            FindObjectOfType<LevelManager3>().currentCheckpoint = transform;  // or a child transform

            if (activatedEffect != null)
                Instantiate(activatedEffect, transform.position, transform.rotation);
        }
    }
}
