using System.Collections;

using UnityEngine;

namespace Lighting
{
    public class Light_Flicker : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] float minFlickerPercentage = 0.25f;
        [SerializeField] float minFlickerSpeed = 0.035f;
        [Range(0f, 1f)]
        [SerializeField] float maxFlickerPercentage = 1f;
        [SerializeField] float maxFlickerSpeed = 0.1f;

        [Header("Stats")]
        [SerializeField] float value;

        private new UnityEngine.Rendering.Universal.Light2D light;
        private float intensity;

        void Awake() => light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        
        // Start is called before the first frame update
        void Start()
        {
            intensity = light.intensity;
            StartCoroutine(Co_Flicker());
        }

        private IEnumerator Co_Flicker()
        {
            while (true)
            {
                value = light.intensity = Random.Range(intensity * minFlickerPercentage, intensity * maxFlickerPercentage);
                yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
            }
        }
    }
}