using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    public float startTime = 120f;
    public TMP_Text timerText;
    public PlayerController player;

    float timeRemaining;
    bool timerRunning = true;

    // ticking timer
    public AudioSource tickSource;
    public float halfPitch = 1.35f;
    bool halfSpeedUpDone = false;

    void Start()
    {
        timeRemaining = startTime;

        if (tickSource != null)
        {
            tickSource.loop = true;
            tickSource.pitch = 1f;
            if (!tickSource.isPlaying) tickSource.Play();
        }
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        // half time -> speed up ticking ONCE
        if (!halfSpeedUpDone && timeRemaining <= startTime / 2f)
        {
            halfSpeedUpDone = true;
            if (tickSource != null)
                tickSource.pitch = halfPitch;
        }

        // time over
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;
            TimeOver();
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void TimeOver()
    {
        SceneManager.LoadScene(5); // or use SceneManager.LoadScene(gameOverSceneName);
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
    }
}
