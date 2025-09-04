using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Magazine : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnPlayerReload = new UnityEvent();
    [HideInInspector] public static UnityEvent OnPlayerEmptyShoot = new UnityEvent();

    private RifleShoot weapon;
    private WeaponSFX weaponSFX;
    [SerializeField] UI_Magazine ammoDisplay;


    [SerializeField] private int magazineSize;
    public int MagazineSize => magazineSize;
    [SerializeField] private float reloadTime;
    public float ReloadTime => reloadTime;

    private int bulletsLeft;
    public int BulletsLeft => bulletsLeft;

    private bool isReloading;
    public bool IsReloading => isReloading;

    void Start()
    {
        weapon = GetComponent<RifleShoot>();
        weaponSFX = GetComponent<WeaponSFX>();

        RifleShoot.OnPlayerShoot.AddListener((_,_) => UseAmmo());

        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (magazineSize != bulletsLeft && PlayerStateMachine.Instance.input.GetReloadInput() && isReloading == false && weapon.IsShooting == false)
            StartCoroutine(Reload());

        if (bulletsLeft <= 0 && isReloading == false && Mouse.current.leftButton.wasPressedThisFrame)
            OnPlayerEmptyShoot.Invoke();
    }

    public void UseAmmo()
    {
        bulletsLeft -= 1;
        ammoDisplay.UpdateUI();
    }

    private IEnumerator Reload()
    {
        OnPlayerReload.Invoke();
        
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        while (weaponSFX.ReloadSoundIsPlaying == true)
            yield return null;

        bulletsLeft = magazineSize;
        isReloading = false;
    }
}
