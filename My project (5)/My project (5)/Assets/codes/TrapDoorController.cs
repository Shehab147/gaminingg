using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorController : MonoBehaviour
{
    [Header("Trap Door Settings")]
    public float openAngle = -90f;
    public float openSpeed = 2f;
    public float resetDelay = 5f;

    private HingeJoint2D hingeJoint;
    private JointMotor2D motor;
    private bool isOpen = false;
    private bool isResetting = false;
    private Quaternion initialRotation;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
        initialRotation = transform.rotation;

        motor = hingeJoint.motor;
        motor.motorSpeed = 0f;
        motor.maxMotorTorque = 1000f;
        hingeJoint.motor = motor;
        hingeJoint.useMotor = false;
    }

    // Public method for puzzle to call
    public void OpenTrapDoor()
    {
        if (isOpen || isResetting) return;

        isOpen = true;
        hingeJoint.useMotor = true;
        motor.motorSpeed = openSpeed;
        hingeJoint.motor = motor;

        Debug.Log("Trap door opening via puzzle!");

        // Start reset after delay
        StartCoroutine(ResetTrapDoor());
    }

    System.Collections.IEnumerator ResetTrapDoor()
    {
        yield return new WaitForSeconds(resetDelay);
        isResetting = true;

        motor.motorSpeed = -openSpeed;
        hingeJoint.motor = motor;

        yield return new WaitForSeconds(1f);

        hingeJoint.useMotor = false;
        transform.rotation = initialRotation;

        isOpen = false;
        isResetting = false;
        Debug.Log("Trap door reset!");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isOpen)
        {
            Debug.Log("Player standing on trap door");
        }
    }
}