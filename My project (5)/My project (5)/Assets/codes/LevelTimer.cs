using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class LevelTimer : MonoBehaviour
{
    public float startTime = 120f;
    public TMPro.TMP_Text timerText;   
    public PlayerController player; 
    
    public string gameOverSceneName = "GameOver";  // scene to load when time ends


    float timeRemaining;
    bool timerRunning = true;

    void Start()
    {
        timeRemaining = startTime;
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;
            TimeOver();  // call time over function
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    void TimeOver()
    {
        SceneManager.LoadScene(5);  // load retry scene
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
    }
}
