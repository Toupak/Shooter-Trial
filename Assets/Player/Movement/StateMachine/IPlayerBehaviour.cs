using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviourType
{
    Run,
    Jump,
    Dash
}

public interface IPlayerBehaviour
{
    public void StartBehaviour(PlayerStateMachine player, BehaviourType previous);
    public void UpdateBehaviour(PlayerStateMachine player);
    public void FixedUpdateBehaviour(PlayerStateMachine player);
    public void StopBehaviour(PlayerStateMachine player, BehaviourType next);
    public BehaviourType GetBehaviourType();
}
