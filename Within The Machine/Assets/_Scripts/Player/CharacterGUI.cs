using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGUI : MonoBehaviour
{
    [SerializeField] private MovementScript movementScript;
    [Space(10)]
    
    [SerializeField] private GameObject RightIKTarget;
    [SerializeField] private GameObject LeftIKTarget;    

    [SerializeField] private Transform holdingTarget;
    [SerializeField] private float movementSpeed = 5f; // Speed of the smooth movement
    [Space(10)]
    
    [Header("Spring Behaviour")]
    [SerializeField] private GameObject bodyBone;
    [SerializeField] private float maxTiltAngle;
    [SerializeField] private float restAngle;
    [SerializeField] private float tiltSpeed;

    private float currentAngle; // Keeps track of the current tilt angle
    private float tiltVelocity = 0f;     
    [SerializeField, Tooltip("How BOUNCY the spring is")] private float springFrequency = 2f; 
    [SerializeField, Tooltip("How much DAMPING the spring has (0 = no damping, 1 = critical damping)")] private float springDamping = 0.5f; 

    [Header("Eye Behaviour")]
    [SerializeField] private float lookSpeed;
    [SerializeField] private GameObject iris;
    [SerializeField] private Transform lookLeft;
    [SerializeField] private Transform lookRight;
    [SerializeField] private Transform lookMiddle;

    void Update()
    {
        MoveIKTargets();
        TiltGUI();
        EyeBehaviour();
    }

    private void MoveIKTargets()
    {
        // Calculate target positions relative to the character
        Vector2 rightTargetPosition = holdingTarget.position;
        Vector2 leftTargetPosition = holdingTarget.position;

        // Smoothly move the IK targets toward their respective positions
        RightIKTarget.transform.position = Vector2.Lerp(RightIKTarget.transform.position, rightTargetPosition, Time.deltaTime * movementSpeed);
        LeftIKTarget.transform.position = Vector2.Lerp(LeftIKTarget.transform.position, leftTargetPosition, Time.deltaTime * movementSpeed);
    }

    private void TiltGUI()
    {
        // Read input
        Vector2 inputVector = movementScript.controls.Player.Movement.ReadValue<Vector2>();
        float horizontalInput = inputVector.x;

        float targetAngle = restAngle;
        if (horizontalInput != 0)
        {
            targetAngle = restAngle + (horizontalInput > 0 ? -maxTiltAngle : maxTiltAngle);
        }

        // Spring-damper system using Hooke's Law
        // F = -k * (x - xTarget) - c * v
        
        //Spring Term       -k * (x - xTarget)
        // k = w^2
        // w = 2 * PI * f

        float angularFrequency = 2 * Mathf.PI * springFrequency;
        float springStiffness = angularFrequency * angularFrequency;
        float springTerm = -springStiffness * (currentAngle - targetAngle);
        
        //Damping Term      -c * v
        // c = 2 * dampingRatio * w
        
        float dampingCoefficient = 2 * springDamping * angularFrequency;
        float dampingTerm = -dampingCoefficient * tiltVelocity;
        
        // Calculate spring force
        float springForce = springTerm + dampingTerm;

        // Update velocity and angle
        tiltVelocity += springForce * Time.deltaTime;
        currentAngle += tiltVelocity * Time.deltaTime;

        bodyBone.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    private void EyeBehaviour()
    {
        Vector2 inputVector = movementScript.controls.Player.Movement.ReadValue<Vector2>();
        float horizontalInput = inputVector.x;

        if (horizontalInput > 0){
            iris.transform.position = Vector3.Lerp(iris.transform.position, lookRight.position, Time.deltaTime * lookSpeed);
        }
        else if (horizontalInput < 0){
            iris.transform.position = Vector3.Lerp(iris.transform.position, lookLeft.position, Time.deltaTime * lookSpeed);
        }
        else
        {
            iris.transform.position = Vector3.Lerp(iris.transform.position, lookMiddle.position, Time.deltaTime * lookSpeed);
        }
    }
}
