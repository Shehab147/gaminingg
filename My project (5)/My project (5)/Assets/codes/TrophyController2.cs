using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyController2 : MonoBehaviour
{
    [Header("Trophy Settings")]
    public Transform player;
    public Transform tableTarget;
    public float followSpeed = 3f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;

    [Header("Gate Reference")]
    public Animator gateAnimator;

    [Header("Moving Target Settings")]
    public Transform targetObject;
    public float moveDistance = 5f;
    public float moveDuration = 2f; // Duration of movement in seconds
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Smooth easing

    public bool isFollowing = false;
    public bool isOnTable = false;

    private Vector3 initialPosition;
    private float originalY;
    private Vector3 targetObjectStartPosition;
    private Coroutine moveCoroutine;

    void Start()
    {
        initialPosition = transform.position;
        if (tableTarget != null)
            originalY = tableTarget.position.y;

        if (targetObject != null)
        {
            targetObjectStartPosition = targetObject.position;
        }
    }

    void Update()
    {
        if (isFollowing && !isOnTable)
        {
            Vector3 targetPosition = player.position + new Vector3(0, 1.5f, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.E) && IsPlayerNearTable())
            {
                PlaceOnTable();
            }
        }
        else if (isOnTable)
        {
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

    public void PlaceOnTable()
    {
        isFollowing = false;
        isOnTable = true;

        if (tableTarget != null)
            transform.position = tableTarget.position;

        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("Open");
            Debug.Log("Gate opened permanently!");
        }

        // Start smooth movement of target object
        if (targetObject != null)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveTargetObjectSmoothly());
        }

        Debug.Log("Trophy placed on table!");
    }

    IEnumerator MoveTargetObjectSmoothly()
    {
        Vector3 startPos = targetObject.position;
        Vector3 endPos = targetObjectStartPosition + Vector3.right * moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveDuration;
            float curveProgress = moveCurve.Evaluate(progress);

            targetObject.position = Vector3.Lerp(startPos, endPos, curveProgress);
            yield return null;
        }

        targetObject.position = endPos;
        Debug.Log("Target object smoothly moved to destination!");
    }

    void FloatAboveTable()
    {
        if (tableTarget == null) return;

        float newY = originalY + floatHeight + Mathf.Sin(Time.time * floatSpeed) * 0.1f;
        transform.position = new Vector3(tableTarget.position.x, newY, tableTarget.position.z);
    }

    public void ResetTrophy()
    {
        isFollowing = false;
        isOnTable = false;
        transform.position = initialPosition;

        if (targetObject != null)
        {
            targetObject.position = targetObjectStartPosition;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
    }
}