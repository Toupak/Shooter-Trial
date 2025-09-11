using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAnimation : MonoBehaviour
{
    private Quaternion startingRotation;
    private Vector3 startingPosition;

    private float kickbackDistance;
    private float knockUpAngle;

    [Header("StartingPositions")]
    [SerializeField] private Transform StartingTransform;
    [SerializeField] private Transform ADSTransform;


    [Header("ADS")]
    [SerializeField] private float aimOffset;

    private ADS ads;

    [SerializeField] private float kickbackDistanceADS;
    [SerializeField] private float knockUpAngleADS;

    [Header("Recoil")]
    [SerializeField] private float returnSpeed;
    [SerializeField] private float returnSpeedRotation;

    private Quaternion targetRotation;
    private Quaternion currentRotation;

    [Header("Bobbing")]
    [SerializeField] private float kickbackDistanceHipfire;
    [SerializeField] private float knockUpAngleHipfire;

    private Weapon weapon;

    private Vector3 startingHipfirePosition;
    private Quaternion startingHipfireRotation;

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

    [SerializeField] private float gunAnimationSpeedADS;
    [SerializeField] private float gunAnimationDistanceADS;

    [SerializeField] private float jumpOffset;
    [SerializeField] private float dashOffset;

    [SerializeField] private float gunAnimationSmoothTime;

    void Start()
    {
        transform.localPosition = StartingTransform.localPosition;
        transform.localRotation = StartingTransform.localRotation;

        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;

        startingHipfirePosition = transform.localPosition;
        startingHipfireRotation = transform.localRotation;

        kickbackDistance = kickbackDistanceHipfire;
        knockUpAngle = knockUpAngleHipfire;

        weapon = GetComponent<Weapon>();
        ads = PlayerStateMachine.Instance.GetComponent<ADS>();

        Weapon.OnPlayerShoot.AddListener((_, _) => DoRifleShootAnimation());
        ADS.OnPlayerStartAiming.AddListener(AimingAnimation);
        ADS.OnPlayerEndAiming.AddListener(StopAimingAnimation);

        targetRotation = startingRotation;
        currentRotation = startingRotation;
    }

    void Update()
    {
        //Recoil
        currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, returnSpeedRotation * Time.deltaTime);
        transform.localRotation = currentRotation;

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
        if (ads.IsAiming)
        {
            gunAnimationSpeed = gunAnimationSpeedADS;
            gunAnimationDistance = gunAnimationDistanceADS;

            float x = Mathf.Sin(Tools.DegreeToRadian(sinTimer * gunAnimationSpeed)) * gunAnimationDistance;
            float y = Mathf.Cos(Tools.DegreeToRadian(cosTimer * gunAnimationSpeed)) * gunAnimationDistance;

            SetTargetPosition(new Vector3(x, y, 0));
            return;
        }

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
        currentRotation = startingRotation * Quaternion.Euler(knockUpAngle, 0, 0);
    }

    private void AimingAnimation()
    {
        targetPosition = ADSTransform.localPosition;
        targetRotation = ADSTransform.localRotation;

        startingPosition = ADSTransform.localPosition;
        startingRotation = ADSTransform.localRotation;

        kickbackDistance = kickbackDistanceADS;
        knockUpAngle = knockUpAngleADS;
    }

    private void StopAimingAnimation()
    {
        startingPosition = startingHipfirePosition;
        startingRotation = startingHipfireRotation;

        targetPosition = startingPosition;
        targetRotation = startingRotation;

        kickbackDistance = kickbackDistanceHipfire;
        knockUpAngle = knockUpAngleHipfire;
    }
}
