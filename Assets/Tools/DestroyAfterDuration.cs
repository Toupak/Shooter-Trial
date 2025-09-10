using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour
{
    [SerializeField] private float duration;

    void Start()
    {
        Destroy(gameObject, duration);
    }
}