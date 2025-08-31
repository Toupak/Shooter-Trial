using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public LayerMask groundLayer;

    [Header("Aim Variables")]
    public float mouseSensitivity;
    [Space]
    public float joystickSensitivityX;
    public float joystickSensitivityY;

    [Header("Run Variables")]
    public float speed;
    public float groundAcceleration;
    public float groundDeceleration;

    [Header("Jump Variables")]
    public float airControlMultiplier;
    public float maxFallSpeed;
    public float jumpStoppedEarlyMultiplier;
    public float fallAcceleration;
    public float jumpForce;

    [Header("Dash Variables")]
    public float dashForce;
    public float dashDistance;
    public float dashDeceleration;
}
