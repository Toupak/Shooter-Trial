using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    [HideInInspector] public static UnityEvent<GameObject> OnPlayerSwitchWeapon = new UnityEvent<GameObject>();

    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;

    [SerializeField] private float yeetSpeed;

    private GameObject activeWeapon;
    private GameObject activeSlot;

    //Faire en sorte que tu puisses swapper d'arme avec la molette

    void Start()
    {
        slot1.SetActive(true);
        slot2.SetActive(false);

        activeSlot = slot1;
        activeWeapon = activeSlot.transform.GetChild(0).gameObject;

        PlayerInteract.OnPlayerPickUp.AddListener(PickUpWeapon);
    }

    void Update()
    {
        if (PlayerStateMachine.Instance.input.GetSlot1Input() && slot1.activeSelf == false && activeSlot != slot1)
            SwitchToWeaponSlot(slot1);

        if (PlayerStateMachine.Instance.input.GetSlot2Input() && slot2.activeSelf == false && activeSlot != slot2)
            SwitchToWeaponSlot(slot2);
    }

    private void PickUpWeapon(GameObject newWeapon)
    {
        if (slot2.transform.childCount == 0)
        {
            slot1.SetActive(false);
            slot2.SetActive(true);

            GameObject GunToAssign = Instantiate(newWeapon);
            GunToAssign.transform.SetParent(slot2.transform);

            activeSlot = slot2;
            activeWeapon = GunToAssign;

            OnPlayerSwitchWeapon.Invoke(activeWeapon);
            return;
        }

        if (slot1.activeSelf == true)
        {
            TossCurrentWeapon(activeWeapon);

            GameObject GunToAssign = Instantiate(newWeapon);
            GunToAssign.transform.SetParent(slot1.transform);
            activeWeapon = GunToAssign;
        }

        if (slot2.activeSelf == true)
        {
            TossCurrentWeapon(activeWeapon);

            GameObject GunToAssign = Instantiate(newWeapon);
            GunToAssign.transform.SetParent(slot2.transform);
            activeWeapon = GunToAssign;
        }
    }

    private void SwitchToWeaponSlot(GameObject slot)
    {
        if (slot == activeSlot || slot.transform.childCount == 0)
            return;

        activeSlot.SetActive(false);
        activeSlot = slot;

        activeWeapon = slot.transform.GetChild(0).transform.gameObject;
        activeSlot.SetActive(true);

        OnPlayerSwitchWeapon.Invoke(activeWeapon);
    }

    private void TossCurrentWeapon(GameObject currentWeapon)
    {
        if (activeWeapon.GetComponent<Weapon>().pickableWeaponPrefab != null)
        {
            GameObject weaponToToss = Instantiate(activeWeapon.GetComponent<Weapon>().pickableWeaponPrefab, transform.position, Quaternion.identity);

            Vector3 cameraDirection = PlayerStateMachine.Instance.cameraOrientation.forward;
            weaponToToss.GetComponent<Rigidbody>().velocity = cameraDirection * yeetSpeed;
        }

        Destroy(activeWeapon);
    }
}
