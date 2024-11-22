using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class ProgressUI : MonoBehaviour
    {
        [SerializeField] Image fill;
        [SerializeField] Gradient gradient;
        [SerializeField] float transitionSpeed = 1f;

        private Slider slider;
        private Coroutine coroutine;

        void Awake() => slider = GetComponent<Slider>();

        // Start is called before the first frame update
        void Start()
        {

        }

        public float Value
        {
            get => slider.value;

            set
            {
                // slider.value = value;
                
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }

                StartCoroutine(Co_TransitionTo(value));
            }
        }

        private IEnumerator Co_TransitionTo(float value)
        {
            var startValue = slider.value;
            var elapsedTime = 0f;

            while (slider.value != value)
            {
                elapsedTime += Time.deltaTime;
                var time = elapsedTime * transitionSpeed;
                slider.value = Mathf.Lerp(startValue, value, time);
                yield return null;
            }
        }

        private void UpdateUI() => fill.color = gradient.Evaluate(slider.value);

        // Update is called once per frame
        void Update() => UpdateUI();
    }
}