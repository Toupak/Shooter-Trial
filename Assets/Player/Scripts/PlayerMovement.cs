using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float groundAcceleration;
    public float groundDeceleration;
    public float airControlMultiplier;
    public float maxFallSpeed;
    public float jumpStoppedEarlyMultiplier;
    public float fallAcceleration;

    public LayerMask groundLayer;
    bool isGrounded;
    bool jumpWasReleasedEarly;

    public float jumpForce;

    public Transform orientation;

    private Rigidbody rb;

    Vector3 inputDirection;
    Vector3 velocityDirection;

    private float jumpTimeStamp;
    private int timesJumped;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        inputDirection = ComputeMoveDirection();

        if (timesJumped < 2 && GetJumpInput(false))
            Jump();

        if (velocityDirection.y > 0f && Keyboard.current.spaceKey.wasReleasedThisFrame)
            jumpWasReleasedEarly = true;
    }

    void FixedUpdate()
    {
        if (velocityDirection.y <= 0)
        {
            bool checkIfGrounded = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.2f, groundLayer);

            if (isGrounded != checkIfGrounded)
            {
                if (checkIfGrounded == true)
                {
                    jumpWasReleasedEarly = false;
                    timesJumped = 0;
                    Debug.Log("you touched the ground");
                }
                else
                    Debug.Log("you are now flying");

                isGrounded = checkIfGrounded;
            }
        }

        MovePlayer(inputDirection);

        if (isGrounded == false)
            ApplyGravity();
        else
            velocityDirection.y = -0.5f;

        rb.velocity = velocityDirection;
    }

    private void MovePlayer(Vector3 direction)
    {
        Vector3 moveDirection = (orientation.forward * direction.z + orientation.right * direction.x).normalized;
        moveDirection *= speed;

        if (inputDirection.magnitude <= 0.1f)
        {
            velocityDirection.x = Mathf.MoveTowards(velocityDirection.x, 0f, groundDeceleration * Time.fixedDeltaTime);
            velocityDirection.z = Mathf.MoveTowards(velocityDirection.z, 0f, groundDeceleration * Time.fixedDeltaTime);
        }
        else if (isGrounded == false)
        {
            velocityDirection.x = Mathf.MoveTowards(velocityDirection.x, moveDirection.x, airControlMultiplier * Time.fixedDeltaTime);
            velocityDirection.z = Mathf.MoveTowards(velocityDirection.z, moveDirection.z, airControlMultiplier * Time.fixedDeltaTime);
        }
        else
        {
            velocityDirection.x = Mathf.MoveTowards(velocityDirection.x, moveDirection.x, groundAcceleration * Time.fixedDeltaTime);
            velocityDirection.z = Mathf.MoveTowards(velocityDirection.z, moveDirection.z, groundAcceleration * Time.fixedDeltaTime);
        }
    }

    private void ApplyGravity()
    {
        float fallSpeed = jumpWasReleasedEarly ? fallAcceleration * jumpStoppedEarlyMultiplier : fallAcceleration;

        velocityDirection.y = Mathf.MoveTowards(velocityDirection.y, -maxFallSpeed, fallSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        isGrounded = false;
        jumpWasReleasedEarly = false;
        timesJumped += 1;
        velocityDirection.y = jumpForce;
    }


    public static Vector3 ComputeMoveDirection()
    {
        Vector3 joystickInput = Vector3.zero;

        if (Gamepad.current != null)
        {
            joystickInput = Gamepad.current.leftStick.value;

            if (joystickInput.magnitude < 0.1f)
                joystickInput = Vector3.zero;
        }

        Vector3 inputDirection = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            inputDirection.z = 1.0f;

        if (Keyboard.current.sKey.isPressed)
            inputDirection.z = -1.0f;

        if (Keyboard.current.dKey.isPressed)
            inputDirection.x = 1.0f;

        if (Keyboard.current.aKey.isPressed)
            inputDirection.x = -1.0f;


        if (joystickInput != Vector3.zero)
            return joystickInput;
        else
            return inputDirection.normalized;
    }

    public bool GetJumpInput(bool useBuffer = true)
    {
        if (useBuffer && Time.time <= jumpTimeStamp)
        {
            jumpTimeStamp = -1.0f;
            return true;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.aButton.wasPressedThisFrame || Gamepad.current.leftShoulder.wasPressedThisFrame;
        else
            return false;
    }
}
