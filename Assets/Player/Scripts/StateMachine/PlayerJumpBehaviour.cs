using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpBehaviour : IPlayerBehaviour
{
    private bool jumpWasReleasedEarly;
    private bool jumpToConsume;
    private int timesJumped;

    public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
    {

    }

    public void UpdateBehaviour(PlayerStateMachine player)
    {
        if (timesJumped < 2 && player.input.GetJumpInput(false))
            StartJump(player);

        if (player.dashBehaviour.CanDash(player) && player.input.GetDashInput(false))
            player.ChangeBehaviour(player.dashBehaviour);
    }

    public void FixedUpdateBehaviour(PlayerStateMachine player)
    {
        bool hasLanded = CheckCollision(player);

        HandleJump(player);

        MovePlayer(player);
        ApplyGravity(player);

        player.ApplyMovement();

        if (hasLanded == true)
            player.ChangeBehaviour(player.runBehaviour);
    }

    public void StartJump(PlayerStateMachine player)
    {
        jumpToConsume = true;
        player.ChangeBehaviour(player.jumpBehaviour);
    }

    private void HandleJump(PlayerStateMachine player)
    {
        if (player.velocityDirection.y > 0f && Keyboard.current.spaceKey.wasReleasedThisFrame)
            jumpWasReleasedEarly = true;

        if (jumpToConsume == true)
            ExecuteJump(player);

        jumpToConsume = false;
    }

    private void ExecuteJump(PlayerStateMachine player)
    {
        jumpWasReleasedEarly = false;
        
        if (player.isGrounded == true)
            timesJumped += 1;
        else
            timesJumped += 2;
        
        player.isGrounded = false;

        player.velocityDirection.y = player.data.jumpForce;
    }

    public bool CheckCollision(PlayerStateMachine player)
    {
        bool hasLanded = false;

        if (player.velocityDirection.y <= 0)
        {
            bool checkIfGrounded = Physics.Raycast(player.transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.2f, player.data.groundLayer);

            if (player.isGrounded != checkIfGrounded)
            {
                if (checkIfGrounded == true)
                {
                    jumpWasReleasedEarly = false;
                    timesJumped = 0;
                    Debug.Log("you touched the ground");
                    hasLanded = true;
                }
                else
                    Debug.Log("you are now flying");

                player.isGrounded = checkIfGrounded;
            }
        }

        return hasLanded;
    }

    public void ApplyGravity(PlayerStateMachine player)
    {
        if (player.isGrounded == true)
        {
            player.velocityDirection.y = -0.5f;
            return;
        }

        float fallSpeed = jumpWasReleasedEarly ? player.data.fallAcceleration * player.data.jumpStoppedEarlyMultiplier : player.data.fallAcceleration;

        player.velocityDirection.y = Mathf.MoveTowards(player.velocityDirection.y, player.data.maxFallSpeed * -1.0f, fallSpeed * Time.fixedDeltaTime);
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
            player.velocityDirection.x = Mathf.MoveTowards(player.velocityDirection.x, moveDirection.x, player.data.airControlMultiplier * Time.fixedDeltaTime);
            player.velocityDirection.z = Mathf.MoveTowards(player.velocityDirection.z, moveDirection.z, player.data.airControlMultiplier * Time.fixedDeltaTime);
        }
    }

    public bool CanJump(PlayerStateMachine player)
    {
        return timesJumped < 2;
    }

    public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
    {

    }

    public BehaviourType GetBehaviourType()
    {
        return BehaviourType.Jump;
    }
}
