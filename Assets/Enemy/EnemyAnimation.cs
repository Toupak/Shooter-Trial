using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private NavMeshAgent navAgent;

    private float lastAnimTimeStamp;
    private float animDuration;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (navAgent.velocity.magnitude == 0 && Time.time - lastAnimTimeStamp >= animDuration)
            PlayIdleAnimation();

        if (navAgent.velocity.magnitude > 0.1f)
            animator.SetBool("isMoving", true);

        if (navAgent.velocity.magnitude == 0)
            animator.SetBool("isMoving", false);
    }

    private void PlayIdleAnimation()
    {
        lastAnimTimeStamp = Time.time;

        float whichAnimRandomizer = Random.Range(1, 10);
        int whichAnim = 0;

        if (whichAnimRandomizer < 3)
        {
            if (whichAnim == 1)
                whichAnim = 0;

            whichAnim = 1;
            animDuration = 3.35f;

        }
        else if (whichAnimRandomizer < 7)
        {
            if (whichAnim == 2)
                whichAnim = 0;

            whichAnim = 2;
            animDuration = 3.25f;
        }
        else
        {
            if (whichAnim == 3)
                whichAnim = 0;

            whichAnim = 3;
            animDuration = 6.133f;
        }

        animator.SetInteger("whichIdle", whichAnim);
    }
}