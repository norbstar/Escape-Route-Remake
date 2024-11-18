using System.Collections;

using UnityEngine;

namespace Lighting
{
    public class Light_CycleColor : MonoBehaviour
    {
        [SerializeField] Gradient gradient;
        [SerializeField] float transitionSpeed = 1f;

        private new UnityEngine.Rendering.Universal.Light2D light;

        [Header("Stats")]
        [SerializeField] Color value;

        void Awake() => light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() => StartCoroutine(Co_CycleColors());

        public Color GetColor(float value) => gradient.Evaluate(value);

        private IEnumerator Co_CycleColors()
        {
            float startTime = Time.time;

            while (true)
            {
                float fractionComplete = (Time.time - startTime) * transitionSpeed;
                value = light.color = GetColor(fractionComplete);

                if (fractionComplete >= 1.0f)
                {
                    startTime = Time.time;
                }

                yield return null;
            }
        }
    }
}