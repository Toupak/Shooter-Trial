using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    //Dash

    private Camera cam;

    [SerializeField] private float adsFOV;
    [SerializeField] private float transitionTime;

    private float startingFOV;
    private float startingTransitionTime;
    private float targetFOV;

    void Start()
    {
        cam = GetComponent<Camera>();
        startingFOV = cam.fieldOfView;
        //startingTransitionTime = transitionTime;
        targetFOV = startingFOV;

        ADS.OnPlayerStartAiming.AddListener(() => targetFOV = adsFOV);
        ADS.OnPlayerEndAiming.AddListener(() => targetFOV = startingFOV);
    }

    private void Update()
    {
        if (cam.fieldOfView != targetFOV)
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFOV, transitionTime * Time.deltaTime);
    }
}
