using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour
{
    [HideInInspector] public static UnityEvent<GameObject> OnPlayerPickUp = new UnityEvent<GameObject>();

    private GameObject hoveredObject;

    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactLayer;

    private bool isHovering;

    void Start()
    {

    }

    void Update()
    {
        Vector3 cameraPosition = PlayerStateMachine.Instance.cameraOrientation.position;
        Vector3 cameraDirection = PlayerStateMachine.Instance.cameraOrientation.forward;

        bool hasHit = Physics.Raycast(cameraPosition, cameraDirection, out RaycastHit hitInfo, interactDistance, interactLayer);

        if (hasHit && isHovering == false)
        {
            StartHovering(hitInfo);
        }
        else if (!hasHit && isHovering == true)
        {
            StopHovering();
        }

        if (hoveredObject != null && hoveredObject.GetComponent<PickableObject>() != null && PlayerStateMachine.Instance.input.GetInteractInput())
        {
            StopHoveringOnPickup();
        }
    }

    private void StartHovering(RaycastHit hitInfo)
    {
        isHovering = true;
        hoveredObject = hitInfo.collider.gameObject;

        PickableObject pickableObjectComponent = hoveredObject.GetComponent<PickableObject>();

        if (pickableObjectComponent != null)
            pickableObjectComponent.SetAimState(isHovering);
    }

    private void StopHovering()
    {
        isHovering = false;
        PickableObject pickableObjectComponent = hoveredObject.GetComponent<PickableObject>();

        if (pickableObjectComponent != null)
            pickableObjectComponent.SetAimState(isHovering);

        hoveredObject = null;
    }

    private void StopHoveringOnPickup()
    {
        isHovering = false;
        PickableObject pickableObjectComponent = hoveredObject.GetComponent<PickableObject>();

        if (pickableObjectComponent != null)
        {
            OnPlayerPickUp.Invoke(pickableObjectComponent.GetGameObjectInfo());

            pickableObjectComponent.SetAimState(isHovering);
            pickableObjectComponent.DestroyOnPickUp();
        }

        hoveredObject = null;
    }
}
