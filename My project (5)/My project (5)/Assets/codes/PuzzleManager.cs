using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public string puzzleName = "Binary Pattern Puzzle";
    public string correctAnswer1 = "1101";
    public string correctAnswer2 = "0111";
    public int maxMistakes = 3;
    public float puzzleDuration = 30f;

    [Header("UI References")]
    public GameObject puzzleUI;
    public Canvas puzzleCanvas;
    public TextMeshProUGUI puzzleText;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI mistakesText;
    public TextMeshProUGUI timerText;
    public TMP_InputField answerInput1;
    public TMP_InputField answerInput2;
    public Button submitButton;

    [Header("References")]
    public TrapDoorController trapDoor;
    public BookPuzzleTrigger bookTrigger;

    [Header("Audio")]
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip puzzleStartSound;

    private int currentMistakes = 0;
    private float currentTime;
    private bool puzzleActive = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Get Canvas component if not assigned
        if (puzzleCanvas == null && puzzleUI != null)
        {
            puzzleCanvas = puzzleUI.GetComponent<Canvas>();
        }

        // Hide UI initially
        if (puzzleUI != null)
            puzzleUI.SetActive(false);
    }

    void Update()
    {
        if (puzzleActive)
        {
            UpdateTimer();

            // Check for ESC key to close puzzle
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePuzzle();
            }

            // Check for Enter key to submit
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SubmitAnswer();
            }
        }
    }

    public void StartPuzzle()
    {
        puzzleActive = true;
        currentMistakes = 0;
        currentTime = puzzleDuration;

        // Show UI
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(true);

            // Ensure canvas is on top
            if (puzzleCanvas != null)
            {
                puzzleCanvas.sortingOrder = 1000; // Very high number to ensure it's on top
            }
        }

        // Set puzzle text
        if (puzzleText != null)
            puzzleText.text = $"{puzzleName}\n\nERROR: File integrity at 42%\nCorruption Type: Missing Binary Pattern\n\nRestore missing pattern:\n\n[ 1110  ?  1011  ? ]\n\nEnter values for the missing slots:";

        // Clear input fields
        if (answerInput1 != null)
            answerInput1.text = "";
        if (answerInput2 != null)
            answerInput2.text = "";

        UpdateMistakesDisplay();
        UpdateTimerDisplay();

        // Play sound
        if (puzzleStartSound != null)
            audioSource.PlayOneShot(puzzleStartSound);

        // Disable player movement
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.enabled = false;
        }

        // Focus on first input field
        if (answerInput1 != null)
        {
            answerInput1.Select();
            answerInput1.ActivateInputField();
        }

        Debug.Log($"{puzzleName} started!");
    }

    public void SubmitAnswer()
    {
        string playerAnswer1 = answerInput1.text.Trim();
        string playerAnswer2 = answerInput2.text.Trim();

        // Validate answers
        if (playerAnswer1 == correctAnswer1 && playerAnswer2 == correctAnswer2)
        {
            CorrectAnswer();
        }
        else
        {
            IncorrectAnswer();
        }
    }

    void CorrectAnswer()
    {
        Debug.Log($"{puzzleName} solved correctly!");

        // Play correct sound
        if (correctSound != null)
            audioSource.PlayOneShot(correctSound);

        // Show success message
        if (errorText != null)
            errorText.text = $"<color=green>{puzzleName} completed!\nFile integrity restored!</color>";

        // Notify book trigger that puzzle is solved
        if (bookTrigger != null)
        {
            bookTrigger.OnPuzzleSolved();
        }

        // Close puzzle after delay
        StartCoroutine(ClosePuzzleWithDelay(2f));
    }

    void IncorrectAnswer()
    {
        currentMistakes++;
        Debug.Log($"{puzzleName} - Incorrect answer! Mistakes: {currentMistakes}/{maxMistakes}");

        // Play incorrect sound
        if (incorrectSound != null)
            audioSource.PlayOneShot(incorrectSound);

        // Show error message
        if (errorText != null)
            errorText.text = $"<color=red>Pattern mismatch!\nTry again. ({currentMistakes}/{maxMistakes} mistakes)</color>";

        UpdateMistakesDisplay();

        // Echo warning
        EchoWarning();

        // Check if player died
        if (currentMistakes >= maxMistakes)
        {
            PlayerDeath();
        }
        else
        {
            // Clear input for retry
            if (answerInput1 != null)
            {
                answerInput1.text = "";
                answerInput2.text = "";
                answerInput1.Select();
                answerInput1.ActivateInputField();
            }
        }
    }

    void EchoWarning()
    {
        Debug.Log($"Echo: '{puzzleName} - Corruption spreading! The pattern must match: 1110, 1101, 1011, 0111'");
    }

    void PlayerDeath()
    {
        Debug.Log($"{puzzleName} - Player died from too many mistakes!");

        // Close puzzle
        ClosePuzzle();

        // Trigger player death
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.DieFromPuzzle();
        }
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (currentTime <= 0)
        {
            TimeUp();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = $"Time: {Mathf.CeilToInt(currentTime)}s";
    }

    void UpdateMistakesDisplay()
    {
        if (mistakesText != null)
            mistakesText.text = $"Mistakes: {currentMistakes}/{maxMistakes}";
    }

    void TimeUp()
    {
        Debug.Log($"{puzzleName} - Time expired!");
        PlayerDeath();
    }

    IEnumerator ClosePuzzleWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePuzzle();
    }

    void ClosePuzzle()
    {
        puzzleActive = false;

        // Hide UI
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(false);

            // Reset canvas sorting order
            if (puzzleCanvas != null)
            {
                puzzleCanvas.sortingOrder = 0;
            }
        }

        // Re-enable player movement
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && !player.IsDead)
        {
            player.enabled = true;
        }

        Debug.Log($"{puzzleName} closed");
    }
}