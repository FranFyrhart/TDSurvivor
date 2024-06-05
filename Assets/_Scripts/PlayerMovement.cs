using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Sirenix.OdinInspector.Editor.Internal.FastDeepCopier;

public class PlayerMovement : CombatHandler
{
    [SerializeField] private Rigidbody rb;
    [Header("Movement settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

    private CharacterController controller;
    private Vector3 velocity;

    #region variables
    private float horizontalInputValue;
    private float verticalInputValue;
    private float finalHorizontalInputValue;
    private float finalVerticalInputValue;
    #endregion

    #region Unity Messages

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Vector3 inputDirection = new Vector3(horizontalInputValue, 0, verticalInputValue).normalized;

        float currentSpeed = Mathf.Min(velocity.magnitude + acceleration * Time.fixedDeltaTime, maxSpeed);
        Vector3 targetVelocity = inputDirection * currentSpeed;

        velocity = Vector3.MoveTowards(velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        controller.Move(velocity * Time.fixedDeltaTime);
    }

    #endregion

    #region methods

    #endregion

    #region controls 

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        horizontalInputValue = context.ReadValue<float>() * -1;
    }
    public void OnMoveVertical(InputAction.CallbackContext context) 
    {
        verticalInputValue = context.ReadValue<float>();
    }

    #endregion
}
