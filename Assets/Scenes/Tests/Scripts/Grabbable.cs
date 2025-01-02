using System.Collections;
using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(Collider2D))]
    public class Grabbable : MonoBehaviour
    {
        [SerializeField] float disableColliderDurationSecs = 0.25f;

        private new Collider2D collider;

        void Awake() => collider = GetComponent<Collider2D>();

        private IEnumerator Co_DisableColliderTemporarily()
        {
            collider.enabled = false;
            yield return new WaitForSeconds(disableColliderDurationSecs);
            collider.enabled = true;
        }

        public void DisableColliderTemporarily() => StartCoroutine(Co_DisableColliderTemporarily());
    }
}
