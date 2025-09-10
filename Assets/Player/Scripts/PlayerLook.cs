using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecoilBuffer
{
    public float recoilxStrength;
    public float recoilyStrength;
    public float timeStamp;

    public RecoilBuffer(float recoilx, float recoily, float time)
    {
        recoilxStrength = recoilx;
        recoilyStrength = recoily;
        timeStamp = time;
    }
}


public class PlayerLook : MonoBehaviour
{
    //PlayerMouse Camera
    public float sensX;
    public float sensY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    //PlayerRecoil Camera
    private List<RecoilBuffer> recoilBuffers = new List<RecoilBuffer>();

    private float cameraMovingSpeedOnShoot;
    
    private float targetxOffset;
    private float targetyOffset;

    private float currentxOffset;
    private float currentyOffset;

    private float onStopShootxOffset;
    private float onStopShootyOffset;

    private float onStartShootingTimeStamp;
    private float lastShootTimeStamp;

    void Start()
    {
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Weapon.OnPlayerShoot.AddListener((spreadx, spready) =>
        {
            currentxOffset = xRotation;
            currentyOffset = yRotation;

            targetxOffset = currentxOffset + spreadx;
            targetyOffset = currentyOffset + spready;
        });

        Weapon.OnPlayerStartShooting.AddListener((snapiness) =>
        {
            onStopShootxOffset = 0;
            onStopShootyOffset = 0;

            cameraMovingSpeedOnShoot = snapiness;
            recoilBuffers.Clear();
        });

        Weapon.OnPlayerStopShooting.AddListener((returnSpeed, returnRecoilDuration) =>
        {
            currentxOffset = xRotation;
            currentyOffset = yRotation;

            GetLastBuffers(returnRecoilDuration);

            targetxOffset = currentxOffset - onStopShootxOffset;
            targetyOffset = currentyOffset - onStopShootyOffset;

            cameraMovingSpeedOnShoot = returnSpeed;
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
        
        currentxOffset = Mathf.MoveTowards(currentxOffset, targetxOffset, cameraMovingSpeedOnShoot * Time.deltaTime);
        currentyOffset = Mathf.MoveTowards(currentyOffset, targetyOffset, cameraMovingSpeedOnShoot * Time.deltaTime);

        xRotation += currentxOffset - previousxOffset;
        yRotation += currentyOffset - previousyOffset;

        RecoilBuffer lastBuffer = new RecoilBuffer(currentxOffset - previousxOffset, currentyOffset - previousyOffset, Time.time);
        recoilBuffers.Add(lastBuffer);
    }

    private void GetLastBuffers(float returnRecoilDuration)
    {
        foreach(RecoilBuffer buffer in recoilBuffers)
        {
            if (Time.time - buffer.timeStamp < returnRecoilDuration)
            {
                onStopShootxOffset += buffer.recoilxStrength;
                onStopShootyOffset += buffer.recoilyStrength;
            }
        }
    }
}
