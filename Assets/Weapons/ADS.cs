using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ADS : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnPlayerStartAiming = new UnityEvent();
    [HideInInspector] public static UnityEvent OnPlayerEndAiming = new UnityEvent();

    private bool isAiming;
    public bool IsAiming => isAiming;

    [SerializeField] private GameObject crosshair;

    void Start()
    {

    }

    void Update()
    {
        if (PlayerStateMachine.Instance.input.GetAimInput())
            StartAiming();

        if (Mouse.current.rightButton.wasReleasedThisFrame)
            StopAiming();
    }

    private void StartAiming()
    {
        isAiming = true;
        OnPlayerStartAiming.Invoke();

        crosshair.SetActive(false);
    }

    private void StopAiming()
    {
        isAiming = false;
        OnPlayerEndAiming.Invoke();

        crosshair.SetActive(true);
    }
}
