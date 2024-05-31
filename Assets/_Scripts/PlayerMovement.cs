using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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
    #endregion

    #region Unity Messages

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        //Calculate the target position based on the input values
        //Vector3 targetPosition = new Vector3(transform.position.x + horizontalInputValue, 0, transform.position.z + verticalInputValue);

        //Vector3 targetPosition = transform.position + transform.forward * verticalInputValue + transform.right * horizontalInputValue;

        Vector3 inputDirection = new Vector3(horizontalInputValue, 0, verticalInputValue).normalized;
        //Vector3 targetPosition = transform.position + inputDirection * speed * Time.fixedDeltaTime;

        float currentSpeed = Mathf.Min(velocity.magnitude + acceleration * Time.fixedDeltaTime, maxSpeed);
        Vector3 targetVelocity = inputDirection * currentSpeed;

        velocity = Vector3.MoveTowards(velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        controller.Move(velocity * Time.fixedDeltaTime);

        //Move the character towards the target position using the speed value
        //rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime));

        //rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, speed * Time.fixedDeltaTime));
    }

    #endregion

    #region methods

    #endregion

    #region controls 

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        horizontalInputValue = context.ReadValue<float>() * -1;
    }
    public void OnMoveVertical(InputAction.CallbackContext context) {
        verticalInputValue = context.ReadValue<float>();
    }

    #endregion
}
