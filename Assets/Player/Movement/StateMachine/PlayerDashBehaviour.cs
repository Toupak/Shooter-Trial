using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDashBehaviour : IPlayerBehaviour
{
    [HideInInspector] public static UnityEvent OnPlayerDash = new UnityEvent();

    private Vector3 startingPosition;
    private float dashTimeStamp;
    private float dashDuration;

    private bool isDecelerating;

    public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
    {
        startingPosition = player.transform.position;
        dashDuration = player.data.dashDistance / player.data.dashForce;

        player.velocityDirection = player.cameraOrientation.forward * player.data.dashForce;
        player.ApplyMovement();

        dashTimeStamp = Time.time;
        OnPlayerDash.Invoke();
    }

    public void UpdateBehaviour(PlayerStateMachine player)
    {
        if (Vector3.Distance(startingPosition, player.transform.position) >= player.data.dashDistance)
            isDecelerating = true;

        if (Time.time - dashTimeStamp > dashDuration)
            isDecelerating = true;
    }

    public void FixedUpdateBehaviour(PlayerStateMachine player)
    {
        player.jumpBehaviour.CheckCollision(player);

        if (isDecelerating)
            player.velocityDirection = Vector3.MoveTowards(player.velocityDirection, Vector3.zero, player.data.dashDeceleration * Time.fixedDeltaTime);

        if (isDecelerating && player.velocityDirection.magnitude < player.data.dashVelocityStopThreshold)
            StopDash(player);

        player.ApplyMovement();
    }

    public void StopDash(PlayerStateMachine player)
    {
        if (player.isGrounded)
            player.ChangeBehaviour(player.runBehaviour);
        else
            player.ChangeBehaviour(player.jumpBehaviour);
    }

    public bool CanDash(PlayerStateMachine player)
    {
        return true;
    }

    private void PlayDashSound()
    {

    }

    public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
    {
        isDecelerating = false;
    }

    public BehaviourType GetBehaviourType()
    {
        return BehaviourType.Dash;
    }

    // appuie sur shift -> Ajoute une vélocité dans la direction qu'on regarde 
}
