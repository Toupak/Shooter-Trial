using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private GameObject currentWeapon;

    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;

    //Enregistrer les games object -> SetChild
    //Faire en sorte que tu puisses swapper d'arme avec la molette et/ou 1 & 2 -> Nouveaux input qui font des events
    //Communique aux autre scripts le changement d'arme

    void Start()
    {
        slot1.SetActive(true);
        slot2.SetActive(false);

        PlayerInteract.OnPlayerPickUp.AddListener(PickUpWeapon);
    }

    void Update()
    {
        
    }

    private void PickUpWeapon(GameObject newWeapon)
    {
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
}
