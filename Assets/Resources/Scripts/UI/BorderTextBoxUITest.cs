using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(BorderedTextBoxUI))]
public class BorderTextBoxUITest : MonoBehaviour
{
    [SerializeField] List<string> content;

    private BorderedTextBoxUI borderedTextBoxUI;
    private int index;

    void Awake() => borderedTextBoxUI = GetComponent<BorderedTextBoxUI>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        if (index < content.Count)
        {
            borderedTextBoxUI.Text = content[index];
        }
    }

    void OnEnable() => borderedTextBoxUI.onComplete += OnComplete;

    void OnDisable() => borderedTextBoxUI.onComplete -= OnComplete;

    private IEnumerator Co_PostDelayedText()
    {
        yield return new WaitForSeconds(1.0f);
        borderedTextBoxUI.Text = content[index];
    }

    private void OnComplete(BorderedTextBoxUI instance)
    {
        if (++index < content.Count)
        {
            StartCoroutine(Co_PostDelayedText());
            return;
        }
        
        // borderedTextBoxUI.ClearText();
    }
}
