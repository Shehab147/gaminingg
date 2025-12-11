using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyController : MonoBehaviour
{
    [Header("Trophy Settings")]
    public Transform player;
    public Transform tableTarget;
    public float followSpeed = 3f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;

    [Header("Gate Reference")]
    public Animator gateAnimator;

    // Made public so other scripts can access them
    public bool isFollowing = false;
    public bool isOnTable = false;

    private Vector3 initialPosition;
    private float originalY;

    void Start()
    {
        initialPosition = transform.position;
        if (tableTarget != null)
            originalY = tableTarget.position.y;
    }

    void Update()
    {
        if (isFollowing && !isOnTable)
        {
            // Follow player with slight offset
            Vector3 targetPosition = player.position + new Vector3(0, 1.5f, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Check for E key press when player is near table
            if (Input.GetKeyDown(KeyCode.E) && IsPlayerNearTable())
            {
                PlaceOnTable();
            }
        }
        else if (isOnTable)
        {
            // Floating animation
            FloatAboveTable();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing && !isOnTable)
        {
            StartFollowing();
        }
    }

    void StartFollowing()
    {
        isFollowing = true;
        Debug.Log("Trophy started following player!");
    }

    bool IsPlayerNearTable()
    {
        if (player == null || tableTarget == null) return false;
        float distanceToTable = Vector2.Distance(player.position, tableTarget.position);
        return distanceToTable < 2f;
    }

    // Made public so PlayerInteraction can call it
    public void PlaceOnTable()
    {
        isFollowing = false;
        isOnTable = true;

        if (tableTarget != null)
            transform.position = tableTarget.position;

        // Open the gate (one-time only)
        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("Open");
            Debug.Log("Gate opened permanently!");
        }

        Debug.Log("Trophy placed on table!");
    }

    void FloatAboveTable()
    {
        if (tableTarget == null) return;

        // Calculate floating Y position using sine wave
        float newY = originalY + floatHeight + Mathf.Sin(Time.time * floatSpeed) * 0.1f;
        transform.position = new Vector3(tableTarget.position.x, newY, tableTarget.position.z);
    }

    // Reset trophy (optional - for debugging)
    public void ResetTrophy()
    {
        isFollowing = false;
        isOnTable = false;
        transform.position = initialPosition;
    }
}