using System.Collections;

using UnityEngine;

namespace Lighting
{
    public class Light_Pulse : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] float minPulsePercentage = 0.25f;
        [Range(0f, 1f)]
        [SerializeField] float maxPulsePercentage = 1f;
        [SerializeField] float pulseSpeed = 1.0f;

        [Header("Stats")]
        [SerializeField] float minIntensity;
        [SerializeField] float maxIntensity;
        [SerializeField] float value;

        private new UnityEngine.Rendering.Universal.Light2D light;

        void Awake() => light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        private float intensity;

        // Start is called before the first frame update
        void Start()
        {
            intensity = light.intensity;
            minIntensity = intensity * minPulsePercentage;
            maxIntensity = intensity * maxPulsePercentage;
            StartCoroutine(Co_Pulse());
        }

        private IEnumerator Co_Pulse()
        {
            while (true)
            {
                var fraction = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
                value = light.intensity = minIntensity + ((maxIntensity - minIntensity) * fraction);
                yield return null;
            }
        }
    }
}