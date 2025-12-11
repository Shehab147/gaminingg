using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    private Animator animator;
    private Collider2D gateCollider;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        gateCollider = GetComponent<Collider2D>();
    }

    public void OpenGate()
    {
        if (isOpen) return; // Prevent reopening if already open

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        // Disable collider so player can pass through permanently
        if (gateCollider != null)
        {
            gateCollider.enabled = false;
        }

        isOpen = true;
        Debug.Log("Gate opened permanently!");
    }

    // Animation event (called from animation timeline)
    public void OnGateOpened()
    {
        Debug.Log("Gate fully opened and will stay open!");
    }

}