using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PuzzleManager_2 : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleData
    {
        public string puzzleName;
        [TextArea(3, 5)]
        public string puzzleDescription;
        public string correctAnswer1;
        public string correctAnswer2;
        public int maxMistakes = 3;
        public float puzzleDuration = 30f;
    }

    public PuzzleData puzzleSettings;

    [Header("UI References")]
    public GameObject puzzleUI;
    public TextMeshProUGUI puzzleTitleText;
    public TextMeshProUGUI puzzleDescriptionText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI mistakesText;
    public TextMeshProUGUI timerText;
    public TMP_InputField answerInput1;
    public TMP_InputField answerInput2;
    public Button submitButton;

    [Header("External References")]
    public PlayerController playerController;
    public BookPuzzleTrigger1 puzzleTrigger;

    private int currentMistakes = 0;
    private float currentTime;
    private bool puzzleActive = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Only add listener if button exists
        if (submitButton != null)
            submitButton.onClick.AddListener(SubmitAnswer);
        else
            Debug.LogError("Submit button is not assigned in PuzzleManager_2!");

        HidePuzzleUI();
    }

    void Update()
    {
        if (puzzleActive)
        {
            HandleInput();
            UpdateTimer();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ClosePuzzle();
        if (Input.GetKeyDown(KeyCode.Return))
            SubmitAnswer();
    }

    void UpdateTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (timerText != null)
                timerText.text = $"Time: {Mathf.CeilToInt(currentTime)}s";

            if (currentTime <= 0)
                TimeUp();
        }
    }

    public void StartPuzzle()
    {
        if (puzzleActive) return;

        puzzleActive = true;
        currentMistakes = 0;
        currentTime = puzzleSettings.puzzleDuration;

        ShowPuzzleUI();
        UpdateUI();

        if (playerController != null)
            playerController.enabled = false;

        if (answerInput1 != null)
        {
            answerInput1.Select();
            answerInput1.ActivateInputField();
        }
    }

    void UpdateUI()
    {
        if (puzzleTitleText != null)
            puzzleTitleText.text = puzzleSettings.puzzleName;

        if (puzzleDescriptionText != null)
            puzzleDescriptionText.text = puzzleSettings.puzzleDescription;

        if (mistakesText != null)
            mistakesText.text = $"Mistakes: {currentMistakes}/{puzzleSettings.maxMistakes}";

        if (timerText != null)
            timerText.text = $"Time: {Mathf.CeilToInt(currentTime)}s";

        if (feedbackText != null)
            feedbackText.text = "";
    }

    public void SubmitAnswer()
    {
        if (!puzzleActive) return;

        // Check if input fields exist
        if (answerInput1 == null || answerInput2 == null)
        {
            Debug.LogError("Input fields are not assigned!");
            return;
        }

        string answer1 = answerInput1.text.Trim().ToLower();
        string answer2 = answerInput2.text.Trim().ToLower();

        string correct1 = puzzleSettings.correctAnswer1.ToLower();
        string correct2 = puzzleSettings.correctAnswer2.ToLower();

        if (answer1 == correct1 && answer2 == correct2)
        {
            OnCorrectAnswer();
        }
        else
        {
            OnIncorrectAnswer();
        }
    }

    void OnCorrectAnswer()
    {
        if (feedbackText != null)
            feedbackText.text = "<color=green>Puzzle solved!</color>";

        if (puzzleTrigger != null)
            puzzleTrigger.OnPuzzleSolved();

        Invoke(nameof(ClosePuzzle), 1.5f);
    }

    void OnIncorrectAnswer()
    {
        currentMistakes++;

        if (feedbackText != null)
            feedbackText.text = "<color=red>Incorrect!</color>";

        if (mistakesText != null)
            mistakesText.text = $"Mistakes: {currentMistakes}/{puzzleSettings.maxMistakes}";

        if (answerInput1 != null && answerInput2 != null)
        {
            answerInput1.text = "";
            answerInput2.text = "";
            answerInput1.Select();
        }

        if (currentMistakes >= puzzleSettings.maxMistakes)
            OnPuzzleFailed();
    }

    void OnPuzzleFailed()
    {
        if (feedbackText != null)
            feedbackText.text = "<color=red>Puzzle failed!</color>";
        Invoke(nameof(ClosePuzzle), 1.5f);
    }

    void TimeUp()
    {
        if (feedbackText != null)
            feedbackText.text = "<color=orange>Time's up!</color>";
        OnPuzzleFailed();
    }

    public void ClosePuzzle()
    {
        if (!puzzleActive) return;

        puzzleActive = false;
        HidePuzzleUI();

        if (playerController != null && !playerController.IsDead)
            playerController.enabled = true;
    }

    void ShowPuzzleUI()
    {
        if (puzzleUI != null)
            puzzleUI.SetActive(true);
        else
            Debug.LogError("Puzzle UI is not assigned!");
    }

    void HidePuzzleUI()
    {
        if (puzzleUI != null)
            puzzleUI.SetActive(false);
    }

    public bool IsPuzzleActive() => puzzleActive;
}