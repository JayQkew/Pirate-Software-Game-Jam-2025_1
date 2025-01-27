using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterGUI : MonoBehaviour
{
    [SerializeField] private GameObject RightIKTarget;
    [SerializeField] private bool rightHolding;
    [SerializeField] private GameObject LeftIKTarget;    
    [SerializeField] private bool leftHolding;

    [SerializeField] private Transform holdingTarget;
    [SerializeField] private Vector2 rightTargetPositionOffset = new Vector2(2f, 1f);
    [SerializeField] private Vector2 leftTargetPositionOffset = new Vector2(-2f, 1f);
    [SerializeField] private float movementSpeed = 5f; // Speed of the smooth movement

    void Update()
    {
        MoveIKTargets();
    }

    private void MoveIKTargets()
    {
        // Calculate target positions relative to the character
        Vector2 rightTargetPosition = !rightHolding ? (Vector2)transform.position + rightTargetPositionOffset : holdingTarget.position;
        Vector2 leftTargetPosition = !leftHolding ? (Vector2)transform.position + leftTargetPositionOffset : holdingTarget.position;

        // Smoothly move the IK targets toward their respective positions
        RightIKTarget.transform.position = Vector2.Lerp(RightIKTarget.transform.position, rightTargetPosition, Time.deltaTime * movementSpeed);
        LeftIKTarget.transform.position = Vector2.Lerp(LeftIKTarget.transform.position, leftTargetPosition, Time.deltaTime * movementSpeed);
    }
}
