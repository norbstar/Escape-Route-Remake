using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class BorderedTextBoxUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] Image border;

    [Header("Audio")]
    [SerializeField] bool enableAudio;
    [SerializeField] AudioClip audioClip;

    public enum TextMode
    {
        Static,
        Dynamic
    }

    [Header("Staggering")]
    [SerializeField] TextMode mode = TextMode.Static;
    [SerializeField] float staggerTime = 0.1f;
    [SerializeField] float delayTime = 1.0f;

    public delegate void OnComplete(BorderedTextBoxUI instance);
    public OnComplete onComplete;

    private AudioSource audioSource;
    private Coroutine coroutine;

    public string Text { get => content.text; set => SetText(value); }

    void Awake() => audioSource = GetComponent<AudioSource>();

    public void Register(OnComplete onComplete) => this.onComplete += onComplete;

    private void SetText(string text)
    {
        // if (mode == TextMode.Static)
        // {
        //     if (enableAudio)
        //     {
        //         audioSource.PlayOneShot(audioClip);
        //     }

        //     content.text = text;
        //     onComplete?.Invoke(this);
        //     return;
        // }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        if (mode == TextMode.Static)
        {
            StartCoroutine(Co_RevealText(text));
        }
        else
        {
            StartCoroutine(Co_StaggerText(text));
        }
    }

    public void ClearText() => content.text = string.Empty;

    private IEnumerator Co_RevealText(string text)
    {
        if (enableAudio)
        {
            audioSource.PlayOneShot(audioClip);
        }

        content.text = text;

        yield return new WaitForSeconds(delayTime);
        onComplete?.Invoke(this);
    }

    private IEnumerator Co_StaggerText(string text)
    {
        int itr = 0;

        while (itr < text.Length)
        {
            if (enableAudio)
            {
                audioSource.PlayOneShot(audioClip);
            }

            var value = text.Substring(0, itr + 1);
            content.text = value;
            // content.text = value.Insert(value.Length - 1, "<size=84>");
            yield return new WaitForSeconds(staggerTime);
            ++itr;
        }

        yield return new WaitForSeconds(delayTime);
        onComplete?.Invoke(this);
    }

    // Update is called once per frame
    void Update() => content.gameObject.SetActive(content.text.Length > 0);
}
