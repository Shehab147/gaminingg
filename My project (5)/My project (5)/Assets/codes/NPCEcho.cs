using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEcho : MonoBehaviour
{
    public AudioClip[] guideMessages;  // all voice clips
    public int messageIndex = 0;       // which one to play
    private bool hasSpoken = false;

    void Start()
    {
        PlayGuideMessage();
    }

    void PlayGuideMessage()
    {
        if (hasSpoken) return;
        
        // Make sure index is valid
        if (messageIndex >= 0 && messageIndex < guideMessages.Length)
        {
            AudioClip clip = guideMessages[messageIndex];
            
            if (clip != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusicSFX(clip);
                hasSpoken = true;
            }
        }
    }
}
