using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class ProgressUI : MonoBehaviour
    {
        [SerializeField] Image fill;
        [SerializeField] float initialTransitionSpeed = 1f;

        [Header("Gradient")]
        [SerializeField] bool useGradient;
        [SerializeField] Gradient gradient;

        protected Slider slider;
        protected float transitionSpeed;

        private Coroutine coroutine;

        public virtual void Awake() => slider = GetComponent<Slider>();

        // Start is called before the first frame update
        void Start() => transitionSpeed = initialTransitionSpeed;

        public float Value { get => slider.value; set => slider.value = value; }

        public void TransitionTo(float value)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            StartCoroutine(Co_TransitionTo(value));
        }

        public float TransitionSpeed { get => transitionSpeed; set => transitionSpeed = value; }

        protected virtual IEnumerator Co_TransitionTo(float target)
        {
            var start = slider.value;
            float fraction = Mathf.Abs(target - start);

            if (fraction != 0f)
            {
                var elapsedTime = 0f;

                while (isActiveAndEnabled && slider.value != target)
                {
                    elapsedTime += Time.deltaTime;
                    var time = elapsedTime / fraction * transitionSpeed;
                    slider.value = Mathf.Lerp(start, target, time);
                    yield return null;
                }
            }
        }

        private void UpdateUI()
        {
            if (!useGradient || gradient == null) return;
            fill.color = gradient.Evaluate(slider.value);
        }

        // Update is called once per frame
        protected virtual void Update() => UpdateUI();
    }
}