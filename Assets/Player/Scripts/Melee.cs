using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Melee : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnPlayerMelee = new UnityEvent();

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject sword;
    
    private Animator animator;
    private bool isSwiping;
    //Sound & EnemyHealth

    void Start()
    {
        animator = sword.GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerStateMachine.Instance.input.GetMeleeInput() && isSwiping == false)
            StartCoroutine(Swipe());
    }


    private IEnumerator Swipe()
    {
        isSwiping = true;
        OnPlayerMelee.Invoke();

        gun.SetActive(false);
        sword.SetActive(true);

        animator.Play("Swipe");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1); //WaitForSeconds(1.05f); //Durée animation Swipe depuis l'animator

        sword.SetActive(false);
        gun.SetActive(true);

        isSwiping = false;
    }
}
