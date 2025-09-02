using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShoot : MonoBehaviour
{
    [SerializeField] private float firerate;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private float bulletVelocity;

    [SerializeField] private List<AudioClip> shootingSounds;

    private float lastShootTimeStamp;

    void Update()
    {
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

        ShootingSound();

        Destroy(bullet, 1f);
    }

    private bool CanShoot()
    {
        return Time.time - lastShootTimeStamp >= firerate;
    }

    private void ShootingSound()
    {
        SFXManager.Instance.PlayRandomSFX(shootingSounds.ToArray());
    }
}
