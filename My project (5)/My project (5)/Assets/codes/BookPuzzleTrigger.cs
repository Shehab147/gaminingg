using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPuzzleTrigger : MonoBehaviour
{
    [Header("Book Settings")]
    public Animator bookAnimator;
    public string openAnimation = "BookOpen";
    public string closeAnimation = "BookClose";
    private bool isBookOpen = false;
    private bool playerInRange = false;

    [Header("Puzzle Settings")]
    public GameObject puzzleUI;
    public PuzzleManager puzzleManager;
    public TrapDoorController trapDoor;

    [Header("UI Prompt")]
    public GameObject interactPrompt;

    void Update()
    {
        // Check if player is in range and presses E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isBookOpen)
            {
                OpenBookAndStartPuzzle();
            }
            else
            {
                CloseBook();
            }
        }

        // Update interact prompt
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(playerInRange && !isBookOpen);
        }
    }

    void OpenBookAndStartPuzzle()
    {
        isBookOpen = true;

        // Play open animation
        if (bookAnimator != null)
        {
            bookAnimator.SetTrigger("Open");
        }

        // Start puzzle after a short delay
        StartCoroutine(StartPuzzleAfterDelay(1f));

        Debug.Log("Book opened, starting puzzle...");
    }

    IEnumerator StartPuzzleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Start the puzzle
        if (puzzleManager != null)
        {
            puzzleManager.StartPuzzle();
        }
    }

    void CloseBook()
    {
        isBookOpen = false;

        // Play close animation
        if (bookAnimator != null)
        {
            bookAnimator.SetTrigger("Close");
        }

        Debug.Log("Book closed");
    }

    // Called by PuzzleManager when puzzle is solved
    public void OnPuzzleSolved()
    {
        Debug.Log("Puzzle solved! Closing book and preparing trap door...");

        // Close book first
        CloseBook();

        // Wait 1 second then open trap door
        StartCoroutine(OpenTrapDoorAfterDelay(1f));
    }

    IEnumerator OpenTrapDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Open trap door after delay
        if (trapDoor != null)
        {
            trapDoor.OpenTrapDoor();
            Debug.Log("Trap door opened with 1-second delay!");
        }
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

            // Auto-close book if player moves away
            if (isBookOpen)
            {
                CloseBook();
            }
        }
    }
}