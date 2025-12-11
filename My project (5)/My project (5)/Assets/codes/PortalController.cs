using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [Header("Portal Settings")]
    public string nextLevelName; // Enter exact scene name (e.g., "Level2")
    public int nextLevelIndex = -1; // Alternative: Use build index
    public float transitionDelay = 1f;

    [Header("Visual Effects")]
    public ParticleSystem portalEffect;
    public Animator portalAnimator;

    [Header("Audio")]
    public AudioClip portalSound;

    private bool isActive = true;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            EnterPortal(other.gameObject);
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
        yield return new WaitForSeconds(transitionDelay);

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
}