using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class HealthBarUI : ProgressUI
    {
        [Header("Config")]
        [SerializeField] bool autoRefill = true;

        private bool isTransitioning, yieldTransition;
        private float lastValue;

        public override void Awake()
        {
            base.Awake();
            lastValue = slider.value;
        }

        public bool AutoRefill { get => autoRefill; set => autoRefill = value; }

        protected override IEnumerator Co_TransitionTo(float target)
        {
            var start = slider.value;
            float fraction = Mathf.Abs(target - start);

            isTransitioning = true;

            if (fraction != 0f)
            {
                var elapsedTime = 0f;

                while (isActiveAndEnabled && autoRefill && slider.value != target)
                {
                    if (yieldTransition)
                    {
                        yieldTransition = false;
                        lastValue = slider.value;
                        break;
                    }

                    elapsedTime += Time.deltaTime;
                    var time = elapsedTime / fraction * transitionSpeed;
                    lastValue = slider.value = Mathf.Lerp(start, target, time);
                    yield return null;
                }
            }

            isTransitioning = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (slider.value != lastValue)
            {
                yieldTransition = true;
            }

            if (!isTransitioning && autoRefill && slider.value != 1f)
            {
                TransitionTo(1f);
            }
        }
    }
}