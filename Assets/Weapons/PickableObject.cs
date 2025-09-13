using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private GameObject realObjectPrefab;

    private Outline outline;

    public bool isAimed;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    public GameObject GetGameObjectInfo()
    {
        return realObjectPrefab;
    }

    public void DestroyOnPickUp()
    {
        Destroy(transform.gameObject);
    }

    private void CastOutline()
    {
        outline.enabled = isAimed;
    }

    public void SetAimState(bool state)
    {
        isAimed = state;
        CastOutline();
    }
}
