using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("MovementSounds")]
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;

    [SerializeField] private List<AudioClip> footsteps;
    [SerializeField] private float footstepCD;
    private float lastFootstepTimeStamp;

    void Start()
    {
        PlayerDashBehaviour.OnPlayerDash.AddListener(() => SFXManager.Instance.PlaySFX(dashSound));

        PlayerJumpBehaviour.OnPlayerJump.AddListener(() => SFXManager.Instance.PlaySFX(jumpSound));
        PlayerJumpBehaviour.OnPlayerLand.AddListener(() => SFXManager.Instance.PlaySFX(landSound));
    }

    void Update()
    {
        if (PlayerStateMachine.Instance.currentBehaviour.GetBehaviourType() == BehaviourType.Run && PlayerStateMachine.Instance.rb.velocity.magnitude > 0.1 && Time.time - lastFootstepTimeStamp > footstepCD)
            Footsteps();
    }

    public void Footsteps()
    {
        SFXManager.Instance.PlayRandomSFX(footsteps.ToArray());
        lastFootstepTimeStamp = Time.time;
    }
}
