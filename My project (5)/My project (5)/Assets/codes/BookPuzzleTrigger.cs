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
    Debug.Log("Closing puzzle");
    if (timer != null){
    Debug.Log("Stopping timer");
        timer.StopTimer();
    }
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
