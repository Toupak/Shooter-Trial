using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour
{
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

            if (hoveredObject.GetComponent<Outline>() != null)
                hoveredObject.GetComponent<PickableObject>().isAimed = true;

        }
        else if (!hasHit && isHovering == true)
        {
            isHovering = false;

            if (hoveredObject.GetComponent<Outline>() != null)
                hoveredObject.GetComponent<PickableObject>().isAimed = false;    

            hoveredObject = null;
        }
    }
}
