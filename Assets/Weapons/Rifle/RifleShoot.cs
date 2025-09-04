using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RifleShoot : MonoBehaviour
{
    [HideInInspector] public static UnityEvent<float, float> OnPlayerShoot = new UnityEvent<float, float>();
    [HideInInspector] public static UnityEvent<float> OnPlayerStartShooting = new UnityEvent<float>();
    [HideInInspector] public static UnityEvent<float, float> OnPlayerStopShooting = new UnityEvent<float, float>();

    [Header("WeaponStats")]
    [SerializeField] private float rpm;

    [Header("WeaponProperties")]
    [SerializeField] private float spreadxIntensity;
    [SerializeField] private float spreadyIntensity;
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

    private Squeeze_and_Stretch squeeze;
    private float lastShootTimeStamp;
    private float hasStartedshootingTimeStamp;

    private void Start()
    {
        squeeze = GetComponent<Squeeze_and_Stretch>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            RegisterStartShooting();

        if (Mouse.current.leftButton.wasReleasedThisFrame)
            RegisterStopShooting();

        if (PlayerStateMachine.Instance.input.GetShootInput() && CanShoot())
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
                squeeze.Trigger();
                hitInfo.collider.GetComponent<EnemyHealth>().TakeDamage();
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
        OnPlayerStartShooting.Invoke(snapiness);
        hasStartedshootingTimeStamp = Time.time;
    }

    private void RegisterStopShooting()
    {
        OnPlayerStopShooting.Invoke(returnSpeed, returnRecoilDuration);
    }

    private bool CanShoot()
    {
        return Time.time - lastShootTimeStamp >= 60 / rpm;
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

    //arme qui bouge / en movement
}
