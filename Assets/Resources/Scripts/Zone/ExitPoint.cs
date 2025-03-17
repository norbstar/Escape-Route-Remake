using UnityEngine;

namespace Zone
{
    [RequireComponent(typeof(Collider2D))]
    public class ExitPoint : MonoBehaviour
    {
        [SerializeField] GameObject zonePrefab;
        [SerializeField] PointID entryPointID;
        // [SerializeField] PointFXType pointFXType;

        public PointID EntryPointID => entryPointID;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                var zoneTransitionManager = FindFirstObjectByType<ZoneTransitionManager>();

                if (zoneTransitionManager != null)
                {
                    zoneTransitionManager.Transition(this, zonePrefab, entryPointID);
                }
            }
        }
    }
}
