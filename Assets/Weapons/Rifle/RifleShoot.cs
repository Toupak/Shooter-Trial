using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RifleShoot : MonoBehaviour
{
    [HideInInspector] public static UnityEvent<float, float> OnPlayerShoot = new UnityEvent<float, float>();
    [HideInInspector] public static UnityEvent OnPlayerStartShooting = new UnityEvent();
    [HideInInspector] public static UnityEvent OnPlayerStopShooting = new UnityEvent();

    [SerializeField] private float firerate;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float spreadIntensity;

    [SerializeField] private List<AudioClip> shootingSounds;

    private float lastShootTimeStamp;

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

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = finalDirection * bulletVelocity;

        OnPlayerShoot.Invoke(CalculateSpread(), CalculateSpread());

        ShootingSound();

        Destroy(bullet, 1f);
    }

    private float CalculateSpread()
    {
        return Random.Range(-spreadIntensity, spreadIntensity);
    }

    private void RegisterStartShooting()
    {
        OnPlayerStartShooting.Invoke();
    }

    private void RegisterStopShooting()
    {
        OnPlayerStopShooting.Invoke();
    }

    //2x plus fort vers le haut - pas vers le bas - 2x moins fort en horizontal
    //limite de temps ou ça arrête de monter
    //vitesse différente aller/retour
    //Ajouter "snapiness"
    //Ajouter "ReturnSpeed"
    //Ce script qui envoie la vitesse de caméra et pas l'autre

    private bool CanShoot()
    {
        return Time.time - lastShootTimeStamp >= firerate;
    }

    private void ShootingSound()
    {
        SFXManager.Instance.PlayRandomSFX(shootingSounds.ToArray());
    }
}
