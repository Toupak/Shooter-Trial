using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Magazine : MonoBehaviour
{
    private Magazine magazine;

    [SerializeField] private TextMeshProUGUI ammoDisplay;

    private IEnumerator Start()
    {
        yield return (0.1f);

        magazine = GetComponent<Magazine>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        ammoDisplay.text = $"{magazine.BulletsLeft}/{magazine.MagazineSize}";
    }
}
