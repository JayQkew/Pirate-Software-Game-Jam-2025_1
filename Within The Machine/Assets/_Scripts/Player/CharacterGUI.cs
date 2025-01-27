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

        // Calculate target rotation angle based on horizontal input
        float targetAngle = horizontalInput == 0 ? restAngle : horizontalInput > 0 ? -maxTiltAngle : maxTiltAngle;

        // Smoothly interpolate to the target angle
        Vector3 targetRotation = new Vector3(0f, 0f, targetAngle);
        bodyBone.transform.eulerAngles = Vector3.Lerp(bodyBone.transform.eulerAngles, targetRotation, Time.deltaTime * tiltSpeed);
    }
}
