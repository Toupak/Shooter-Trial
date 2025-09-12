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

    private Weapon weapon;
    private WeaponSFX weaponSFX;
    private Coroutine reload;

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
        weapon = GetComponent<Weapon>();
        weaponSFX = GetComponent<WeaponSFX>();

        Weapon.OnPlayerShoot.AddListener((_,_) => UseAmmo());

        Melee.OnPlayerMelee.AddListener(CancelReload);
        WeaponHolder.OnPlayerSwitchWeapon.AddListener((_) => CancelReload());

        bulletsLeft = magazineSize;
        OnUpdateAmmoCount.Invoke(bulletsLeft, magazineSize);
    }

    void Update()
    {
        if (magazineSize != bulletsLeft && PlayerStateMachine.Instance.input.GetReloadInput() && isReloading == false && weapon.IsShooting == false)
            reload = StartCoroutine(Reload());

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

    private void CancelReload()
    {
        if (reload != null)
        {
            StopAllCoroutines();
            reload = null;
            isReloading = false;
        }
    }
}
