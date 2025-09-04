using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    private Coroutine blinkroutine = null;
    private Color OGcolor;

    [SerializeField] private Image image;
    [SerializeField] private Color blinkColor;
    [SerializeField] private float blinkDuration;
    [SerializeField] private float blinkInterval;

    void Awake()
    {
        OGcolor = image.color;
    }

    public void Trigger()
    {
        if (blinkroutine != null)
            StopCoroutine(blinkroutine);

        blinkroutine = StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        Color newColor = blinkColor;

        float timer = 0;
        while (timer < blinkDuration)
        {
            image.color = newColor;
            yield return new WaitForSeconds(blinkInterval);
            image.color = OGcolor;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval * 2;
        }
    }
}
