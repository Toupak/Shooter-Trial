using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public static UnityEvent<float, float> OnPlayerShoot = new UnityEvent<float, float>();
    [HideInInspector] public static UnityEvent<float> OnPlayerStartShooting = new UnityEvent<float>();
    [HideInInspector] public static UnityEvent<float, float> OnPlayerStopShooting = new UnityEvent<float, float>();

    private Magazine magazine;

    [Header("WeaponStats")]
    [SerializeField] private float rpm;

    private float lastShootTimeStamp;
    private float hasStartedshootingTimeStamp;
    private bool isShooting;
    public bool IsShooting => isShooting;

    public bool isAuto;

    [Header("WeaponProperties")]
    private float spreadxIntensity;
    private float spreadyIntensity;

    [SerializeField] private float spreadxIntensityHipfire;
    [SerializeField] private float spreadyIntensityHipfire;

    [SerializeField] private float spreadxIntensityADS;
    [SerializeField] private float spreadyIntensityADS;

    [SerializeField] private float returnRecoilDuration;
    [SerializeField] private float fullRecoilDuration;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float snapiness;

    [SerializeField] private LayerMask targetLayer;

    private float Recoilx;
    private float Recoily;

    [Header("WeaponConstruct")]
    [SerializeField] private List<AudioClip> shootingSounds;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private float bulletVelocity;

    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private float muzzleFlashSizeCoefficient;

    private void Start()
    {
        magazine = GetComponent<Magazine>();

        spreadxIntensity = spreadxIntensityHipfire;
        spreadyIntensity = spreadyIntensityHipfire;

        ADS.OnPlayerStartAiming.AddListener(StartAimingStats);
        ADS.OnPlayerEndAiming.AddListener(StopAimingStats);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            RegisterStartShooting();

        if (Mouse.current.leftButton.wasReleasedThisFrame)
            RegisterStopShooting();

        if (PlayerStateMachine.Instance.input.GetShootInput() && CanShoot() && isAuto)
        {
            Shoot();
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && CanShoot() && !isAuto)
            Shoot();
    }

    private void Shoot()
    {
        lastShootTimeStamp = Time.time;

        Vector3 cameraPosition = PlayerStateMachine.Instance.cameraOrientation.position;
        Vector3 cameraDirection = PlayerStateMachine.Instance.cameraOrientation.forward;
        Vector3 finalPosition = cameraPosition + cameraDirection * 1000;
        Vector3 finalDirection = (finalPosition - bulletSpawnPosition.position).normalized;

        ShootRaycast(cameraPosition, cameraDirection);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = finalDirection * bulletVelocity;

        CalculateRecoil();
        OnPlayerShoot.Invoke(Recoilx, Recoily);

        ShootingEffects();

        Destroy(bullet, 1f);
    }

    private void ShootRaycast(Vector3 cameraPosition, Vector3 shootDirection)
    {
        bool hasHit = Physics.Raycast(cameraPosition, shootDirection, out RaycastHit hitInfo, 200, targetLayer);
        
        if (hasHit)
        {
            if (hitInfo.transform.CompareTag("Target"))
            {
                hitInfo.collider.GetComponent<EnemyHealth>().TakeDamage();
                //Squeeze est d�clench� sur le crosshair depuis un Event OnEnemyTakeDamage
                Debug.Log("Target was hit");
            }
        }
    }

    private void CalculateRecoil()
    {
        if (Time.time - hasStartedshootingTimeStamp > fullRecoilDuration)
            Recoilx = (Random.Range(0, spreadyIntensity) * -1)/4;
        else
            Recoilx = Random.Range(0, spreadyIntensity) * -1;

            Recoily = Random.Range(-spreadxIntensity, spreadxIntensity);
    }

    private void RegisterStartShooting()
    {
        isShooting = true;
        OnPlayerStartShooting.Invoke(snapiness);
        hasStartedshootingTimeStamp = Time.time;
    }

    private void RegisterStopShooting()
    {
        isShooting = false;
        OnPlayerStopShooting.Invoke(returnSpeed, returnRecoilDuration);
    }

    private bool CanShoot()
    {
        return Time.time - lastShootTimeStamp >= 60 / rpm && magazine.BulletsLeft > 0 && magazine.IsReloading == false;
    }

    private void ShootingEffects()
    {
        SFXManager.Instance.PlayRandomSFX(shootingSounds.ToArray());
        //Check le PDF de proSoundCollection

        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, bulletSpawnPosition.position + bulletSpawnPosition.forward * 0.001f, bulletSpawnPosition.rotation);
        muzzleFlash.transform.SetParent(bulletSpawnPosition);
        muzzleFlash.transform.localScale = Vector3.one * muzzleFlashSizeCoefficient;
        Destroy(muzzleFlash, 0.25f);
    }

    private void StartAimingStats()
    {
        spreadxIntensity = spreadxIntensityADS;
        spreadyIntensity = spreadyIntensityADS;
    }

    private void StopAimingStats()
    {
        spreadxIntensity = spreadxIntensityHipfire;
        spreadyIntensity = spreadyIntensityHipfire;
    }
}
