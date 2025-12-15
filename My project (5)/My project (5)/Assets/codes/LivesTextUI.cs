using TMPro;
using UnityEngine;

public class LivesTextUI : MonoBehaviour
{
    public TextMeshProUGUI livesText;

    public void UpdateLives(int lives)
    {
        livesText.text = "x " + lives;
    }
}
