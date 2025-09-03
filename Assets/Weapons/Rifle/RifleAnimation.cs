using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAnimation : MonoBehaviour
{
    [SerializeField] private float kickbackDistance;
    [SerializeField] private float knockupDistance;

    [SerializeField] private float returnSpeed;
    [SerializeField] private float returnSpeedRotation;

    private float targetzposition;
    private float currentzposition;

    private float targetxrotation;
    private float currentxrotation;

    void Start()
    {
        RifleShoot.OnPlayerShoot.AddListener((_, _) => DoRifleAnimation());

        targetzposition = transform.localPosition.z;
        currentzposition = transform.localPosition.z;

        targetxrotation = transform.localRotation.x;
        currentxrotation = transform.localRotation.x;
    }

    void Update()
    {
        currentzposition = Mathf.MoveTowards(currentzposition, targetzposition, returnSpeed * Time.deltaTime);
        currentxrotation = Mathf.MoveTowards(currentxrotation, targetxrotation, returnSpeedRotation * Time.deltaTime);

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, currentzposition);
        transform.localRotation = Quaternion.Euler(currentxrotation, 0, 0);
    }

    private void DoRifleAnimation()
    {
        currentzposition = targetzposition - kickbackDistance;
        currentxrotation = targetxrotation - knockupDistance;
    }

    //rotation en X
}
