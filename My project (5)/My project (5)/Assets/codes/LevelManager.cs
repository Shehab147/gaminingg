using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Transform respawnPoint;
    private PlayerController player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // If no respawn point set, use starting position
        if (respawnPoint == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                respawnPoint = playerObj.transform;
                player.SetCheckpoint(respawnPoint.position);
            }
        }
        else
        {
            player.SetCheckpoint(respawnPoint.position);
        }
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        respawnPoint = newCheckpoint;
        player.SetCheckpoint(newCheckpoint.position);
        Debug.Log($"New checkpoint set at: {newCheckpoint.position}");
    }

    public void RespawnPlayer()
    {
        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            player.Heal(100f); // Full heal on respawn

            Debug.Log("Player respawned at checkpoint!");
        }
    }

    public PlayerController GetPlayer()
    {
        return player;
    }
}