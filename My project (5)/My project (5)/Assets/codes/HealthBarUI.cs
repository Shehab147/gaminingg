using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (player != null)
        {
            healthSlider.value = player.CurrentHealth / 100f;
            healthText.text = $"Health: {player.CurrentHealth}%";
            livesText.text = $"Lives: {player.CurrentLives}";
        }
    }
}