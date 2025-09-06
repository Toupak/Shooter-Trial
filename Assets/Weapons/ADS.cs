using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ADS : MonoBehaviour
{
    //Animation - placer le gun au bon endroit
    //ADS When click droit isPressed
    //Modif de la vitesse de running - modif du spread de l'arme - peut resserrer le crosshair si besoin ?

    //Booléen isADS
    //Event OnStartADS - OnEndADS qui communique avec le reste -- vient modifier spread Intensity - hipfireSpread // ADSSpread
    //Autres scripts qui ne vont pas se faire

    //Recoil Anim spécifique pour ADS
    //faire disparaitre le rond blanc ?

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
