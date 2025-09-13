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
            isHovering = true;
            hoveredObject = hitInfo.collider.gameObject;

            if (hoveredObject.GetComponent<PickableObject>() != null)
                hoveredObject.GetComponent<PickableObject>().SetAimState(isHovering);
        }
        else if (!hasHit && isHovering == true)
        {
            isHovering = false;

            if (hoveredObject.GetComponent<PickableObject>() != null)
                hoveredObject.GetComponent<PickableObject>().SetAimState(isHovering);    

            hoveredObject = null;
        }

        if (hoveredObject != null && hoveredObject.GetComponent<PickableObject>() != null && PlayerStateMachine.Instance.input.GetInteractInput())
        {
            OnPlayerPickUp.Invoke(hoveredObject.GetComponent<PickableObject>().GetGameObject());

            isHovering = false;

            if (hoveredObject.GetComponent<PickableObject>() != null)
            {
                hoveredObject.GetComponent<PickableObject>().SetAimState(isHovering);
                hoveredObject.GetComponent<PickableObject>().DestroyOnPickUp();
            }

            hoveredObject = null;
        }
    }
}
