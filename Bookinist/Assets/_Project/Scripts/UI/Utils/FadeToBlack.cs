using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeToBlack : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private Coroutine currentFade;

    public void Fade(float targetAlpha)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            Color c = fadeImage.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = c;

            yield return null;
        }

        // Ensure exact final value
        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;

        currentFade = null;
    }
}
