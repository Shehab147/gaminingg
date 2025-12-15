using UnityEngine;
using UnityEngine.SceneManagement;

public class BookPuzzleTrigger : MonoBehaviour
{
    public GameObject puzzlePanel;
    private bool hasTriggered = false;
private AudioSource audioSource;
void Start()
{
    if (puzzlePanel != null)
        puzzlePanel.SetActive(false);

    audioSource = GetComponent<AudioSource>();
}

    [Header("UI Prompt")]
    public GameObject interactPrompt;

    void Start()
    {
        // Ensure puzzle UI is hidden at the start
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(false);
        }
    }

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

        // Hide puzzle UI
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(false);
        }

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

    // Called by PuzzleTimer when time runs out
    public void RestartLevel()
    {
        Debug.Log("Time's up! Restarting level...");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player") && !hasTriggered)
{
    hasTriggered = true;

    // 🔊 Play puzzle sound
    if (audioSource != null)
        audioSource.Play();

    puzzlePanel.SetActive(true);
    Time.timeScale = 0f;

    PuzzleTimer timer = puzzlePanel.GetComponent<PuzzleTimer>();
    if (timer != null)
        timer.StartTimer();
}

    }

    // 🔹 REQUIRED by PuzzleManager & PuzzleManager_2
    // Do NOT remove this
    public void OnPuzzleSolved()
    {
        // Default behavior when puzzle system says "solved"
        ClosePuzzle();
    }

 public void ClosePuzzle()
{
    PuzzleTimer timer = puzzlePanel.GetComponent<PuzzleTimer>();
    if (timer != null)
        timer.StopTimer();

    puzzlePanel.SetActive(false);
    Time.timeScale = 1f;
}


    // 🔘 NO button
    public void RestartLevel()
    {
        // Respawn player (decrease life, etc) - handled by PuzzleManager on restart level function
        Time.timeScale = 1f;
        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.DieFromPuzzle();
        }
        
        hasTriggered = false; // Reset trigger so puzzle can be reopened after respawn
        ClosePuzzle();
    }
}
