using UnityEngine;
using TMPro;

public class PuzzleTimer : MonoBehaviour
{
    public float timeLeft = 10f;          // seconds
    public TextMeshProUGUI timerText;
    public BookPuzzleTrigger puzzleTrigger;

    private bool timerRunning = false;

    void Update()
    {
        if (!timerRunning) return;

        timeLeft -= Time.unscaledDeltaTime; // works even when game is paused

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            timerRunning = false;
            puzzleTrigger.RestartLevel(); // same as NO button
        }

        timerText.text = "time " + Mathf.Ceil(timeLeft) + "s";
    }

    public void StartTimer()
    {
        timeLeft = 10f;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
}
