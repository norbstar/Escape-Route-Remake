using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class SceneWipeUIManager : SingletonMonoBehaviour<SceneWipeUIManager>
{
    [SerializeField] Image overlay;

    public IEnumerator FadeOutCoroutine(float fadeSpeed = 5.0f)
    {
        while (overlay.color.a != 0f)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.MoveTowards(overlay.color.a, 0f, fadeSpeed * Time.deltaTime));
            yield return null;
        }

        overlay.gameObject.SetActive(false);
    }
    
    // public void FadeOut(float fadeSpeed = 5.0f) => StartCoroutine(FadeOutCoroutine(fadeSpeed));

    public IEnumerator FadeInCoroutine(float fadeSpeed = 5.0f)
    {
        overlay.gameObject.SetActive(true);

        while (overlay.color.a != 1f)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.MoveTowards(overlay.color.a, 1f, fadeSpeed * Time.deltaTime));
            yield return null;
        }
    }

    // public void FadeIn(float fadeSpeed = 5.0f) => StartCoroutine(FadeInCoroutine(fadeSpeed));
}