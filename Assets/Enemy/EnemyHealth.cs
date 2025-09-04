using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Image crosshairOnHit;
    [SerializeField] private float fadeDuration;
    [SerializeField] private List<AudioClip> hurtSounds;
    [SerializeField] private float hurtVolume;

    private Coroutine crosshairOnHitCoroutine;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage()
    {
        SFXManager.Instance.PlayRandomSFX(hurtSounds.ToArray(), hurtVolume);

        if (crosshairOnHitCoroutine != null)
        {
            StopCoroutine(crosshairOnHitCoroutine);
            crosshairOnHitCoroutine = null;
        }

        crosshairOnHitCoroutine = StartCoroutine(SpawnOnHitCrosshair());
    }

    private IEnumerator SpawnOnHitCrosshair()
    {
        crosshairOnHit.color = crosshairOnHit.color.ToAlpha(1);
        yield return Tools.Fade(crosshairOnHit, fadeDuration, false);
    }



    //Son localisé
}
