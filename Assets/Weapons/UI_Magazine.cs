using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Magazine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        Magazine.OnUpdateAmmoCount.AddListener(UpdateUI);

        WeaponHolder.OnPlayerSwitchWeapon.AddListener((weapon) =>
        {
            if (weapon.GetComponent<Magazine>() != null)
            {
                Magazine magazine = weapon.GetComponent<Magazine>();
                UpdateUI(magazine.BulletsLeft, magazine.MagazineSize);
            }
        });
    }

    public void UpdateUI(int ammo, int magazineSize)
    {
        ammoDisplay.text = $"{ammo}/{magazineSize}";
    }
}
