using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Magazine : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnPlayerReload = new UnityEvent();
    [HideInInspector] public static UnityEvent OnPlayerEmptyShoot = new UnityEvent();
    [HideInInspector] public static UnityEvent<int, int> OnUpdateAmmoCount = new UnityEvent<int, int>();

    private RifleShoot weapon;
    private WeaponSFX weaponSFX;
    [SerializeField] UI_Magazine ammoDisplay;

    [SerializeField] private int magazineSize;
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
        OnUpdateAmmoCount.Invoke(bulletsLeft, magazineSize);
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
        OnUpdateAmmoCount.Invoke(bulletsLeft, magazineSize);
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        OnPlayerReload.Invoke();

        yield return new WaitForSeconds(reloadTime);

        while (weaponSFX.ReloadSoundIsPlaying == true)
            yield return null;

        bulletsLeft = magazineSize;
        OnUpdateAmmoCount.Invoke(bulletsLeft, magazineSize);

        isReloading = false;
    }
}
