using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : BaseUI
{
    [SerializeField] float fadeDuration;
    Color dark = new Color(0, 0, 0, 1);
    Color transparent = new Color(0, 0, 0, 0);

    public void FadeOut() // Screen slowly turns dark
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        float timer = 0f;
        while(timer < fadeDuration)
        {

            GetUI<Image>("FadeImage").color = Color.Lerp(transparent, dark, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        GetUI<Image>("FadeImage").color = dark;
    }

    public void FadeIn() // Screen slowly lights up
    {
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            GetUI<Image>("FadeImage").color = Color.Lerp(dark, transparent, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        GetUI<Image>("FadeImage").color = transparent;
    }
}
