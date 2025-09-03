using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    [SerializeField] private float cameraSpeedShootingSpread;

    private float targetxOffset;
    private float targetyOffset;

    private float currentxOffset;
    private float currentyOffset;

    private float onStopShootxOffset;
    private float onStopShootyOffset;

    void Start()
    {
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        RifleShoot.OnPlayerShoot.AddListener((x, y) =>
        {
            currentxOffset = xRotation;
            currentyOffset = yRotation;

            targetxOffset = currentxOffset + x;
            targetyOffset = currentyOffset + y;
        });

        RifleShoot.OnPlayerStartShooting.AddListener(() =>
        {
            onStopShootxOffset = 0;
            onStopShootyOffset = 0;
        });

        RifleShoot.OnPlayerStopShooting.AddListener(() =>
        {
            currentxOffset = xRotation;
            currentyOffset = yRotation;

            targetxOffset = currentxOffset - onStopShootxOffset;
            targetyOffset = currentyOffset - onStopShootyOffset;
        });
    }

    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam & orientation
        AddRecoilEffect();
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void AddRecoilEffect()
    {
        float previousxOffset = currentxOffset;
        float previousyOffset = currentyOffset;
        
        currentxOffset = Mathf.MoveTowards(currentxOffset, targetxOffset, cameraSpeedShootingSpread * Time.deltaTime);
        currentyOffset = Mathf.MoveTowards(currentyOffset, targetyOffset, cameraSpeedShootingSpread * Time.deltaTime);

        xRotation += currentxOffset - previousxOffset;
        yRotation += currentyOffset - previousyOffset;

        onStopShootxOffset += currentxOffset - previousxOffset;
        onStopShootyOffset += currentyOffset - previousyOffset;
    }
}
