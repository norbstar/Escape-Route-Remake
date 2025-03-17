using System.Collections;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFXUI : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float minFlickerPercentage = 0.25f;
    [SerializeField] float minFlickerSpeed = 0.035f;
    [Range(0f, 1f)]
    [SerializeField] float maxFlickerPercentage = 1f;
    [SerializeField] float maxFlickerSpeed = 0.1f;
    
    [Header("Stats")]
    [SerializeField] float value;

    private CanvasGroup canvasGroup;
    private float intensity;

    void Awake() => canvasGroup = GetComponent<CanvasGroup>();
    
    // Start is called before the first frame update
    void Start()
    {
        intensity = canvasGroup.alpha;
        StartCoroutine(Co_Flicker());
    }

    private IEnumerator Co_Flicker()
    {
        while (true)
        {
            value = canvasGroup.alpha = Random.Range(intensity * minFlickerPercentage, intensity * maxFlickerPercentage);
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
    }
}
