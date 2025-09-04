using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSFX : MonoBehaviour
{
    private Magazine magazine;

    private bool reloadSoundIsPlaying;
    public bool ReloadSoundIsPlaying => reloadSoundIsPlaying;

    [Header("Magazine")]
    [SerializeField] private List<AudioClip> emptyMagazineSounds;
    [SerializeField] private List<AudioClip> startReloadingSounds;
    [SerializeField] private List<AudioClip> finishReloadingSounds;

    void Start()
    {
        magazine = GetComponent<Magazine>();

        Magazine.OnPlayerReload.AddListener(() => StartCoroutine(ReloadSound()));
        Magazine.OnPlayerEmptyShoot.AddListener(() => SFXManager.Instance.PlayRandomSFX(emptyMagazineSounds.ToArray()));
    }

    void Update()
    {
        
    }

    private IEnumerator ReloadSound()
    {
        reloadSoundIsPlaying = true;

        SFXManager.Instance.PlayRandomSFX(startReloadingSounds.ToArray());
        yield return new WaitForSeconds(magazine.ReloadTime);

        AudioSource finishReloading = SFXManager.Instance.PlayRandomSFX(finishReloadingSounds.ToArray());
        yield return new WaitWhile(() => finishReloading != null);
        
        reloadSoundIsPlaying = false;
    }
}
