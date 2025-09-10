using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletHole;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            CreateBulletImpactEffect(collision);
        }
    }

    private void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];

        GameObject hole = Instantiate(bulletHole, contactPoint.point, Quaternion.LookRotation(-contactPoint.normal));

        hole.transform.SetParent(collision.gameObject.transform);
    }
}
