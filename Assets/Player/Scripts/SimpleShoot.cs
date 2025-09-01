using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;

    [SerializeField] private float bulletVelocity;

    void Update()
    {
        if (PlayerStateMachine.Instance.input.GetShootInput())
            Shoot();
    }

    private void Shoot()
    {
        Vector3 cameraPosition = PlayerStateMachine.Instance.cameraOrientation.position;
        Vector3 cameraDirection = PlayerStateMachine.Instance.cameraOrientation.forward;
        Vector3 finalPosition = cameraPosition + cameraDirection * 200;
        Vector3 finalDirection = (finalPosition - bulletSpawnPosition.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = finalDirection * bulletVelocity;
        Destroy(bullet, 1f);
    }
}
