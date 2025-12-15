using UnityEngine;

public class BookPuzzleTrigger1 : MonoBehaviour
{
    public Animator bookAnimator;
    public PuzzleManager_2 puzzleManager;
    public TrapDoorController trapDoor;
    public GameObject interactPrompt;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!puzzleManager.IsPuzzleActive())
            {
                OpenBookAndStartPuzzle();
            }
        }

        if (interactPrompt != null)
            interactPrompt.SetActive(playerInRange && !puzzleManager.IsPuzzleActive());
    }

    void OpenBookAndStartPuzzle()
    {
        if (bookAnimator != null)
            bookAnimator.SetTrigger("Open");

        puzzleManager.StartPuzzle();
    }

    public void OnPuzzleSolved()
    {
        if (bookAnimator != null)
            bookAnimator.SetTrigger("Close");

        Invoke(nameof(OpenTrapDoor), 1f);
    }

    void OpenTrapDoor()
    {
        if (trapDoor != null)
            trapDoor.OpenTrapDoor();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}