using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("MovementSounds")]
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip jumpSound;

    void Start()
    {
        PlayerDashBehaviour.OnPlayerDash.AddListener(() => SFXManager.Instance.PlaySFX(dashSound));
        PlayerJumpBehaviour.OnPlayerJump.AddListener(() => SFXManager.Instance.PlaySFX(jumpSound));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
