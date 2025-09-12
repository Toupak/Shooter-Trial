using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;

    private GameObject activeSlot;

    //Enregistrer les games object -> SetChild
    //Faire en sorte que tu puisses swapper d'arme avec la molette et/ou 1 & 2 -> Nouveaux input qui font des events
    //Communique aux autre scripts le changement d'arme

    void Start()
    {
        slot1.SetActive(true);
        slot2.SetActive(false);

        activeSlot = slot1;

        PlayerInteract.OnPlayerPickUp.AddListener(PickUpWeapon);
    }

    void Update()
    {
        //A transformer dans un check booléen a part pour la lisibilité
        if (PlayerStateMachine.Instance.input.GetSlot1Input() && slot1.activeSelf == false && activeSlot != slot1)
        {
            Debug.Log("1 is pressed");
            SwitchToWeaponSlot(slot1);
        }

        if (PlayerStateMachine.Instance.input.GetSlot2Input() && slot2.activeSelf == false && activeSlot != slot2)
        {
            Debug.Log("2 is pressed");
            SwitchToWeaponSlot(slot2);
        }
    }

    private void PickUpWeapon(GameObject newWeapon)
    {
        if (slot2.transform.childCount == 0)
        {
            GameObject GunToAssign = Instantiate(newWeapon);
            GunToAssign.transform.SetParent(slot2.transform);

            slot1.SetActive(false);
            slot2.SetActive(true);

            activeSlot = slot2;
        }

        if (slot1.activeSelf == true)
        {
            Destroy(slot1.transform.GetChild(0).gameObject);

            GameObject GunToAssign = Instantiate(newWeapon);
            GunToAssign.transform.SetParent(slot1.transform);
        }

        if (slot2.activeSelf == true)
        {
            Destroy(slot2.transform.GetChild(0).gameObject);

            GameObject GunToAssign = Instantiate(newWeapon);
            GunToAssign.transform.SetParent(slot2.transform);
        }
    }

    private void SwitchToWeaponSlot(GameObject slot)
    {
        if (slot == activeSlot)
            return;

        activeSlot.SetActive(false);
        activeSlot = slot;
        activeSlot.SetActive(true);
    }
}
