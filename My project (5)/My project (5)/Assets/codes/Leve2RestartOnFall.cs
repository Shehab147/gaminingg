using UnityEngine;
using UnityEngine.SceneManagement;

public class Leve2RestartOnFall : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Make sure game is not paused
            Time.timeScale = 1f;

            // Restart current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
