using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private GameObject realObjectPrefab;

    private Outline outline;

    public bool isAimed;
    private bool hasAimed;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (isAimed != hasAimed)
        {
            CastOutline();
            hasAimed = !hasAimed;
        }
    }

    public GameObject PickUpObject()
    {
        return realObjectPrefab;
    }

    private void CastOutline()
    {
        outline.enabled = !outline.enabled;
    }
}
