using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private GameObject realObjectPrefab;

    private Outline outline;

    public bool isAimed;

    //public enum : Weapon / ? -- Fais une action selon le type : ammo/enum/coffre/porte/...

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    public GameObject PickUpObject()
    {
        //Se fait détruire
        return realObjectPrefab;
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
