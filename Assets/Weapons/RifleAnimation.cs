using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAnimation : MonoBehaviour
{
    [Header("Recoil")]
    [SerializeField] private float kickbackDistance;
    [SerializeField] private float knockupDistance;

    [SerializeField] private float returnSpeed;
    [SerializeField] private float returnSpeedRotation;

    private float targetxrotation;
    private float currentxrotation;

    [Space]
    [Header("Bobbing")]
    private RifleShoot weapon;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private Vector3 velocity;

    private float sinTimer;
    private float cosTimer;

    private float gunAnimationSpeed;
    private float gunAnimationDistance;

    [SerializeField] private float gunAnimationSpeedIdle;
    [SerializeField] private float gunAnimationDistanceIdle;

    [SerializeField] private float gunAnimationSpeedRun;
    [SerializeField] private float gunAnimationDistanceRun;

    [SerializeField] private float jumpOffset;
    [SerializeField] private float dashOffset;

    [SerializeField] private float gunAnimationSmoothTime;

    void Start()
    {
        //Bobbing
        startingPosition = transform.localPosition;
        weapon = GetComponent<RifleShoot>();

        RifleShoot.OnPlayerShoot.AddListener((_, _) => DoRifleShootAnimation());

        targetxrotation = transform.localRotation.x;
        currentxrotation = transform.localRotation.x;
    }

    void Update()
    {
        //Recoil
        //targetPosition.z = Mathf.MoveTowards(targetPosition.z, targetzposition, returnSpeed * Time.deltaTime);
        currentxrotation = Mathf.MoveTowards(currentxrotation, targetxrotation, returnSpeedRotation * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentxrotation, 0, 0);

        //Bobbing
        UpdateTimersCosSin();
        ComputeTargetPositionFromState();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, gunAnimationSmoothTime);
    }

    private void ComputeTargetPositionFromState()
    {
        //Shooting
        if (weapon.IsShooting)
        {
            SetTargetPosition(new Vector3(0, 0, 0));
            return;
        }

        //Dashing
        if (PlayerStateMachine.Instance.currentBehaviour.GetBehaviourType() == BehaviourType.Dash)
            SetTargetPosition(new Vector3(0, 0, dashOffset));

        //Jumping Up 
        if (PlayerStateMachine.Instance.currentBehaviour.GetBehaviourType() == BehaviourType.Jump && PlayerStateMachine.Instance.velocityDirection.y > 0)
            SetTargetPosition(new Vector3(0, -jumpOffset, 0));

        //Jumping Down
        if (PlayerStateMachine.Instance.currentBehaviour.GetBehaviourType() == BehaviourType.Jump && PlayerStateMachine.Instance.velocityDirection.y < 0)
            SetTargetPosition(new Vector3(0, jumpOffset, 0));

        //Running
        if (PlayerStateMachine.Instance.currentBehaviour.GetBehaviourType() == BehaviourType.Run && PlayerStateMachine.Instance.rb.velocity.magnitude > 0.1)
        {
            gunAnimationSpeed = gunAnimationSpeedRun;
            gunAnimationDistance = gunAnimationDistanceRun;

            float x = Mathf.Sin(Tools.DegreeToRadian(sinTimer * gunAnimationSpeed)) * gunAnimationDistance;
            float y = Mathf.Cos(Tools.DegreeToRadian(cosTimer * gunAnimationSpeed)) * gunAnimationDistance;

            SetTargetPosition(new Vector3(x, y, 0));
        }

        //Idle
        if (PlayerStateMachine.Instance.currentBehaviour.GetBehaviourType() == BehaviourType.Run && PlayerStateMachine.Instance.rb.velocity.magnitude < 0.1f)
        {
            gunAnimationSpeed = gunAnimationSpeedIdle;
            gunAnimationDistance = gunAnimationDistanceIdle;

            float x = Mathf.Sin(Tools.DegreeToRadian(sinTimer * gunAnimationSpeed)) * gunAnimationDistance;
            float y = Mathf.Cos(Tools.DegreeToRadian(cosTimer * gunAnimationSpeed)) * gunAnimationDistance;

            SetTargetPosition(new Vector3(x, y, 0));
        }
    }

    private void SetTargetPosition(Vector3 position)
    {
        targetPosition = startingPosition + position;
    }

    private void UpdateTimersCosSin()
    {
        sinTimer += Time.deltaTime;
        cosTimer += Time.deltaTime * 2;

        if (sinTimer >= 360)
            sinTimer -= 360;

        if (cosTimer >= 360)
            cosTimer -= 360;
    }

    private void DoRifleShootAnimation()
    {
        transform.localPosition = startingPosition - Vector3.forward * kickbackDistance;
        currentxrotation = targetxrotation - knockupDistance;
    }
}
