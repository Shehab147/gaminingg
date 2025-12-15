using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public int nextLevelIndex ; //based on buldsettings ,scene number 

    public bool isActive = true;

    

    private bool isActive = true;
    private AudioSource audioSource;
[Header("Lives")]
public int maxLives = 3;
public LivesTextUI livesUI;

private int currentLives;

    void Start()
    {
        currentLives = maxLives;

if (livesUI != null)
    livesUI.UpdateLives(currentLives);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

        if (other.CompareTag("Player"))
        {
            isActive = false;
            SceneManager.LoadScene(2);
        }
    }

    void EnterPortal(GameObject player)
    {
        isActive = false;

        // Play portal effects
        if (portalAnimator != null)
            portalAnimator.SetTrigger("Activate");

        if (portalEffect != null)
            portalEffect.Play();

        if (portalSound != null && audioSource != null)
            audioSource.PlayOneShot(portalSound);

        // Disable player control
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        // Make player disappear or teleport animation
        player.GetComponent<SpriteRenderer>().enabled = false;

        // Load next level after delay
        StartCoroutine(LoadNextLevel());
    }

    System.Collections.IEnumerator LoadNextLevel()
    {
        Debug.Log("Portal activated! Loading next level...");
        
        // Wait for sound to play (ensure minimum delay for audio)
        float waitTime = transitionDelay;
        if (portalSound != null && audioSource != null)
        {
            // Ensure we wait at least for the sound clip length or transition delay
            waitTime = Mathf.Max(transitionDelay, portalSound.length);
        }
        
        yield return new WaitForSeconds(waitTime);

        // Load next level
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else if (nextLevelIndex >= 0)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            Debug.LogError("No level specified for portal!");
        }
    }

