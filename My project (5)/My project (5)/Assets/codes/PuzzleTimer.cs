using UnityEngine;
using TMPro;

public class PuzzleTimer : MonoBehaviour
{
    public float timeLeft = 10f;
    public TextMeshProUGUI timerText;
    public BookPuzzleTrigger puzzleTrigger;

    private bool timerRunning = false;
    private AudioSource timerAudio;

    void Awake()
    {
        timerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!timerRunning) return;

        timeLeft -= Time.unscaledDeltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            timerRunning = false;
            StopSound();
            puzzleTrigger.RestartLevel();
        }

        timerText.text = "time " + Mathf.Ceil(timeLeft) + "s";
    }

    public void StartTimer()
    {
        timeLeft = 10f;
        timerRunning = true;
        PlaySound();
    }

    public void StopTimer()
    {
        timerRunning = false;
        StopSound();
    }

    void PlaySound()
    {
        if (timerAudio != null && !timerAudio.isPlaying)
            timerAudio.Play();
    }

    void StopSound()
    {
        if (timerAudio != null && timerAudio.isPlaying)
            timerAudio.Stop();
    }
}
