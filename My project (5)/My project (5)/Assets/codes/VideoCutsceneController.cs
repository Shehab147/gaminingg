using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoCutsceneController : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public string videoFileName; // Name of video file in StreamingAssets folder
    
    [Header("Scene Transition")]
    public string nextSceneName = "level 2"; // Scene to load after video
    public int nextSceneIndex = -1; // Alternative: use build index
    public bool skipOnInput = true; // Allow player to skip video
    
    [Header("UI Elements")]
    public GameObject skipPromptUI; // Optional: "Press any key to skip" text
    public CanvasGroup fadeCanvas; // Optional: for fade transitions
    
    private bool videoEnded = false;
    private bool isTransitioning = false;

    void Start()
    {
        // Get VideoPlayer component if not assigned
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Setup video player
        if (videoPlayer != null)
        {
            // Set video source
            if (!string.IsNullOrEmpty(videoFileName))
            {
                videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            }

            // Subscribe to video events
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.prepareCompleted += OnVideoPrepared;
            
            // Prepare and play video
            videoPlayer.Prepare();
        }
        else
        {
            Debug.LogError("No VideoPlayer component found!");
        }

        // Show skip prompt if available
        if (skipPromptUI != null)
        {
            skipPromptUI.SetActive(skipOnInput);
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video prepared, starting playback...");
        vp.Play();
    }

    void Update()
    {
        // Allow skipping video with any key press
        if (skipOnInput && !videoEnded && !isTransitioning && Input.anyKeyDown)
        {
            SkipVideo();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished playing");
        videoEnded = true;
        LoadNextScene();
    }

    public void SkipVideo()
    {
        if (isTransitioning) return;
        
        Debug.Log("Video skipped by player");
        
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
        
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        StartCoroutine(TransitionToNextScene());
    }

    IEnumerator TransitionToNextScene()
    {
        // Optional: Fade out
        if (fadeCanvas != null)
        {
            float fadeDuration = 1f;
            float elapsed = 0f;
            
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
                yield return null;
            }
        }
        else
        {
            // Small delay before transition
            yield return new WaitForSeconds(0.5f);
        }

        // Load next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"Loading scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else if (nextSceneIndex >= 0)
        {
            Debug.Log($"Loading scene index: {nextSceneIndex}");
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("No next scene specified!");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
            videoPlayer.prepareCompleted -= OnVideoPrepared;
        }
    }
}
