using UnityEngine;

namespace Zone
{
    [RequireComponent(typeof(ZoneManager))]
    public class ZoneInitialiser : MonoBehaviour
    {
        [SerializeField] GameObject zonePrefab;

        private ZoneManager zoneManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (zoneManager == null)
            {
                zoneManager = GetComponent<ZoneManager>();
            }

            zoneManager.SpawnZone(zonePrefab);
            enabled = false;
        }
    }
}
