using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class TestEnemyMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
            navAgent.SetDestination(PlayerStateMachine.Instance.transform.position);
    }

}
