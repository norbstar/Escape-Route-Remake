using System.Collections;

using UnityEngine;

namespace Tests
{
    public class ComparativeYieldTest : MonoBehaviour
    {
        private int nullCounter = 0;
        private int waitForFixedUpdateCounter = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(Co_YieldNull());
            StartCoroutine(Co_YieldWaitForFixedUpdate());
        }

        private IEnumerator Co_YieldNull()
        {
            while (isActiveAndEnabled)
            {
                ++nullCounter;
                Debug.Log($"Yield Null [{nullCounter}]");
                yield return null;
            }
        }

        private IEnumerator Co_YieldWaitForFixedUpdate()
        {
            while (isActiveAndEnabled)
            {
                ++waitForFixedUpdateCounter;
                Debug.Log($"Yield WaitForFixedUpdate [{waitForFixedUpdateCounter}]");
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
