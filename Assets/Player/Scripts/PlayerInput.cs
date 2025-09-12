using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private float jumpTimeStamp;
    private float dashTimeStamp;

    public Vector3 ComputeMoveDirection()
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

    public Vector2 GetAimingDirectionWithSensibility(PlayerData playerData)
    {
        Vector2 gamepad = Vector2.zero;
        Vector2 mouse = Vector2.zero;
        float sensibilityMultiplier = Application.isEditor ? 5.0f : 1.0f;

        if (Gamepad.current != null)
        {
            gamepad = new Vector2(Gamepad.current.rightStick.x.ReadValue(), Gamepad.current.rightStick.y.ReadValue());

            if (Mathf.Abs(gamepad.x) <= 0.15f)
                gamepad.x = 0.0f;

            if (Mathf.Abs(gamepad.y) <= 0.15f)
                gamepad.y = 0.0f;

            if (gamepad.magnitude > 0.15f)
            {
                gamepad.x *= playerData.joystickSensitivityX * sensibilityMultiplier * Time.deltaTime;
                gamepad.y *= playerData.joystickSensitivityY * sensibilityMultiplier * Time.deltaTime;
                return gamepad;
            }
        }

        mouse.x = Input.GetAxisRaw("Mouse X");
        mouse.y = Input.GetAxisRaw("Mouse Y");

        if (mouse.magnitude > 0.0f)
            mouse *= playerData.mouseSensitivity * sensibilityMultiplier * Time.deltaTime;

        return mouse;
    }

    public bool GetJumpInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.aButton.wasPressedThisFrame || Gamepad.current.leftShoulder.wasPressedThisFrame;
        else
            return false;
    }

    public bool GetDashInput()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.bButton.wasPressedThisFrame;
        else
            return false;
    }

    public bool GetShootInput()
    {
        if (Mouse.current.leftButton.isPressed)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.rightTrigger.isPressed;
        else
            return false;
    }
    public bool GetAimInput()
    {
        if (Mouse.current.rightButton.isPressed)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.leftTrigger.isPressed;
        else
            return false;
    }

    public bool GetReloadInput()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.xButton.wasPressedThisFrame;
        else
            return false;
    }
    public bool GetMeleeInput()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.yButton.isPressed;
        else
            return false;
    }

    public bool GetInteractInput()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.rightShoulder.isPressed;
        else
            return false;
    }

    public bool GetSlot1Input()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.rightStickButton.isPressed;
        else
            return false;
    }

    public bool GetSlot2Input()
    {
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            return true;
        else if (Gamepad.current != null)
            return Gamepad.current.leftStickButton.isPressed;
        else
            return false;
    }
}
