using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager3 : MonoBehaviour
{
    public Transform currentCheckpoint;   // updated by Checkpoint script
    public Transform startPoint;          // optional fallback

    public void RespawnPlayer(Transform player)
    {
        if (currentCheckpoint != null)
            player.position = currentCheckpoint.position;
        else if (startPoint != null)
            player.position = startPoint.position;
    }
}
