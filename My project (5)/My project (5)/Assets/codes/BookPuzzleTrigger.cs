using UnityEngine;
using UnityEngine.SceneManagement;

public class BookPuzzleTrigger : MonoBehaviour
{
    public GameObject puzzlePanel;

    void Start()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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


    // 🔘 YES button
    public void RestartLevel()
    {
        Time.timeScale = 1f; // unpause FIRST
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
