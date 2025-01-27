using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGUI : MonoBehaviour
{
    [SerializeField] private MovementScript movementScript;
    [Space(10)]
    
    [SerializeField] private GameObject RightIKTarget;
    [SerializeField] private bool rightHolding;
    [SerializeField] private GameObject LeftIKTarget;    
    [SerializeField] private bool leftHolding;

    [SerializeField] private Transform holdingTarget;
    [SerializeField] private Transform rightRestingPosition;
    [SerializeField] private Transform leftRestingPosition;
    [SerializeField] private float movementSpeed = 5f; // Speed of the smooth movement
    
    [Space(10)]
    
    [SerializeField] private GameObject bodyBone;
    [SerializeField] private float maxTiltAngle;
    [SerializeField] private float restAngle;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float overshootFactor = 1.2f; // How much to overshoot
    [SerializeField] private float dampingSpeed = 2f; // Speed of damping correction

    private float currentTiltVelocity; // Stores velocity for smooth damping
    private float currentTiltAngle; // Keeps track of the current tilt angle
    private float tiltVelocity = 0f;     
    [SerializeField] private float springFrequency = 2f; // How "bouncy" the spring is
    [SerializeField] private float springDamping = 0.5f; // How much damping the spring has (0 = no damping, 1 = critical damping)


    void Update()
    {
        MoveIKTargets();
        TiltGUI();
    }

    private void MoveIKTargets()
    {
        // Calculate target positions relative to the character
        Vector2 rightTargetPosition = !rightHolding ? rightRestingPosition.position : holdingTarget.position;
        Vector2 leftTargetPosition = !leftHolding ? leftRestingPosition.position : holdingTarget.position;

        // Smoothly move the IK targets toward their respective positions
        RightIKTarget.transform.position = Vector2.Lerp(RightIKTarget.transform.position, rightTargetPosition, Time.deltaTime * movementSpeed);
        LeftIKTarget.transform.position = Vector2.Lerp(LeftIKTarget.transform.position, leftTargetPosition, Time.deltaTime * movementSpeed);
    }

    private void TiltGUI()
    {
        // Read input
        Vector2 inputVector = movementScript.controls.Player.Movement.ReadValue<Vector2>();
        float horizontalInput = inputVector.x;

        // Determine target angle
        float targetAngle = restAngle; // Default to 90 degrees
        if (horizontalInput != 0)
        {
            // Tilt left or right based on input
            targetAngle = restAngle + (horizontalInput > 0 ? -maxTiltAngle : maxTiltAngle);
        }

        // Spring-damper system
        float angularFrequency = springFrequency * 2 * Mathf.PI; // Convert frequency to angular frequency
        float dampingRatio = springDamping;
        float dt = Time.deltaTime;

        // Calculate spring force and damping
        float force = -angularFrequency * angularFrequency * (currentTiltAngle - targetAngle) - 2 * dampingRatio * angularFrequency * tiltVelocity;

        // Update velocity and angle
        tiltVelocity += force * dt;
        currentTiltAngle += tiltVelocity * dt;

        // Apply the tilt to the bodyBone
        bodyBone.transform.rotation = Quaternion.Euler(0f, 0f, currentTiltAngle);
    }
}
