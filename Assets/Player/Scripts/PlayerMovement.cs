using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float gravity;

    public LayerMask groundLayer;
    bool isGrounded;

    public float jumpForce;
    public float airMultiplier;

    public Transform orientation;

    private Rigidbody rb;

    Vector3 inputDirection;
    Vector3 velocityDirection;

    private float jumpTimeStamp;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        inputDirection = ComputeMoveDirection();

        if (isGrounded && GetJumpInput(false))
            Jump();
    }

    void FixedUpdate()
    {
        if (velocityDirection.y <= 0)
            isGrounded = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.2f, groundLayer);

        MovePlayer(inputDirection);

        if (isGrounded == false)
            ApplyGravity();
        else
            velocityDirection.y = 0;

        rb.velocity = velocityDirection;
    }

    private void MovePlayer(Vector3 direction)
    {
        Vector3 moveDirection = orientation.forward * direction.z + orientation.right * direction.x;
        moveDirection *= speed;
        velocityDirection = new Vector3(moveDirection.x, velocityDirection.y, moveDirection.z);
    }

    private void ApplyGravity()
    {
        velocityDirection += Vector3.down * gravity;
    }

    private void Jump()
    {
        //reset y velocity
        velocityDirection = new Vector3(velocityDirection.x, 0f, velocityDirection.z);
        isGrounded = false;

        velocityDirection += Vector3.up * jumpForce;
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
