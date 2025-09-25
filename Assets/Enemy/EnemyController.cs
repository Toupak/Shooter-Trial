using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Se d�placer vers le joueur / range d'aggro
    // Attaque le joueur / range d'attaque

    //-> Joue sur les animations

    // Stop d�placements/Attaques

    // Barre de stagger qui se d�clenche en fonction de EnemyHealth

    [SerializeField] private float aggroRange;
    [SerializeField] private float attackRange;

    private NavMeshAgent navAgent;

    private Vector3 playerPosition;
    

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(PlayerStateMachine.Instance.transform.position, transform.position);

        if (distanceToPlayer < aggroRange)
            MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        navAgent.SetDestination(PlayerStateMachine.Instance.transform.position);
    }
}
