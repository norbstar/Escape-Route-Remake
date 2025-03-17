using System.Collections;

using UnityEngine;

public class ScaleFX : MonoBehaviour
{
    [SerializeField] float startScale = 5f;
    [SerializeField] float endScale = 1f;
    [SerializeField] float scaleSpeed = 1f;
    [SerializeField] float preDelay = 1f;

    [Header("Stats")]
    [SerializeField] float value;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        transform.localScale = Vector3.one * startScale;

        if (startScale != endScale)
        {
            yield return new WaitForSeconds(preDelay);
            StartCoroutine(Co_Scale());
        }
    }

    private IEnumerator Co_Scale()
    {
        float elsapsedTime = 0f;
        
        while (transform.localScale.x != endScale)
        {
            elsapsedTime += Time.deltaTime;
            var scale = Mathf.Lerp(startScale, endScale, elsapsedTime * scaleSpeed);
            
            if (startScale < endScale)
            {
                scale = Mathf.Clamp(scale, startScale, endScale);
            }
            else
            {
                scale = Mathf.Clamp(scale, endScale, startScale);
            }

            transform.localScale = Vector3.one * scale;
            value = scale;
            yield return null;
        }
    }
}
