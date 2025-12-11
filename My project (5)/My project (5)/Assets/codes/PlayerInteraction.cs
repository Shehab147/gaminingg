using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;
    public float interactionRange = 2f;

    [Header("UI Feedback")]
    public GameObject interactPrompt;

    private TrophyController currentTrophy;

    void Update()
    {
        CheckForInteractions();
        HandleInteractPrompt();
    }

    void CheckForInteractions()
    {
        // Check if player is near a table with a trophy following
        if (currentTrophy != null && currentTrophy.isFollowing) // Now accessible
        {
            if (currentTrophy.tableTarget != null)
            {
                float distanceToTable = Vector2.Distance(transform.position, currentTrophy.tableTarget.position);
                if (distanceToTable < interactionRange)
                {
                    if (Input.GetKeyDown(interactKey))
                    {
                        currentTrophy.PlaceOnTable(); // Now accessible
                    }
                }
            }
        }
    }

    void HandleInteractPrompt()
    {
        if (interactPrompt != null)
        {
            bool showPrompt = currentTrophy != null &&
                             currentTrophy.isFollowing && // Now accessible
                             currentTrophy.tableTarget != null &&
                             Vector2.Distance(transform.position, currentTrophy.tableTarget.position) < interactionRange;

            interactPrompt.SetActive(showPrompt);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trophy"))
        {
            TrophyController trophy = other.GetComponent<TrophyController>();
            if (trophy != null)
            {
                currentTrophy = trophy;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Trophy"))
        {
            currentTrophy = null;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}
