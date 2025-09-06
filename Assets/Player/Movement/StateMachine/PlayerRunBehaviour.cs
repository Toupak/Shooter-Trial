using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunBehaviour : IPlayerBehaviour
{
    public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
    {
        
    }

    public void UpdateBehaviour(PlayerStateMachine player)
    {
        if (player.jumpBehaviour.CanJump(player) && player.input.GetJumpInput())
            player.jumpBehaviour.StartJump(player);

        if (player.dashBehaviour.CanDash(player) && player.input.GetDashInput())
            player.ChangeBehaviour(player.dashBehaviour);
    }

    public void FixedUpdateBehaviour(PlayerStateMachine player)
    {
        player.jumpBehaviour.CheckCollision(player);

        MovePlayer(player);

        player.jumpBehaviour.ApplyGravity(player);

        player.ApplyMovement();

        if (player.isGrounded == false)
            player.ChangeBehaviour(player.jumpBehaviour);
    }

    private void MovePlayer(PlayerStateMachine player)
    {
        Vector3 moveDirection = (player.orientation.forward * player.inputDirection.z + player.orientation.right * player.inputDirection.x).normalized;
        moveDirection *= player.data.speed;

        if (player.inputDirection.magnitude <= 0.1f)
        {
            player.velocityDirection.x = Mathf.MoveTowards(player.velocityDirection.x, 0f, player.data.groundDeceleration * Time.fixedDeltaTime);
            player.velocityDirection.z = Mathf.MoveTowards(player.velocityDirection.z, 0f, player.data.groundDeceleration * Time.fixedDeltaTime);
        }
        else
        {
            player.velocityDirection.x = Mathf.MoveTowards(player.velocityDirection.x, moveDirection.x, player.data.groundAcceleration * Time.fixedDeltaTime);
            player.velocityDirection.z = Mathf.MoveTowards(player.velocityDirection.z, moveDirection.z, player.data.groundAcceleration * Time.fixedDeltaTime);
        }

        
    }

    public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
    {

    }

    public BehaviourType GetBehaviourType()
    {
        return BehaviourType.Run;
    }
}
