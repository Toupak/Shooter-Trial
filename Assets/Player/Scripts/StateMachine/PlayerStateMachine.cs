using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance;

    public PlayerRunBehaviour runBehaviour = new PlayerRunBehaviour();
    public PlayerJumpBehaviour jumpBehaviour = new PlayerJumpBehaviour();
    public PlayerDashBehaviour dashBehaviour = new PlayerDashBehaviour();

    public IPlayerBehaviour currentBehaviour; 

    public PlayerData data;

    public Transform orientation;
    public Transform cameraOrientation;
    
    [HideInInspector] public PlayerInput input;

    [HideInInspector] public Vector3 inputDirection;
    [HideInInspector] public Vector3 velocityDirection;

    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public bool isGrounded;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();

        currentBehaviour = runBehaviour;
        currentBehaviour.StartBehaviour(this, BehaviourType.Run); 
    }

    private void Update()
    {
        inputDirection = input.ComputeMoveDirection();

        currentBehaviour.UpdateBehaviour(this);
    }

    private void FixedUpdate()
    {
        currentBehaviour.FixedUpdateBehaviour(this);
    }

    public void ChangeBehaviour(IPlayerBehaviour newBehaviour)
    {
        if (newBehaviour == null || newBehaviour == currentBehaviour)
            return;

        currentBehaviour.StopBehaviour(this, newBehaviour.GetBehaviourType());

        BehaviourType previous = currentBehaviour.GetBehaviourType();

        currentBehaviour = newBehaviour;

        currentBehaviour.StartBehaviour(this, previous);
    }

    public void ApplyMovement()
    {
        rb.velocity = velocityDirection;
    }
}
