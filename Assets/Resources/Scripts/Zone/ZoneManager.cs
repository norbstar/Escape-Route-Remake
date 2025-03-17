using UnityEngine;

using Tests;

namespace Zone
{
    public class ZoneManager : MonoBehaviour
    {
        [SerializeField] private BasePlayer playerPrefab;
        [SerializeField] ZoneTransitionAction zoneTransitionAction;

        [Header("Analytics")]
        [SerializeField] [ReadOnly] GameObject activeZone;

        // private GameObject activeZone;
        // private PointID entryPointID;

        public GameObject ActiveZone => activeZone;

        // public PointID EntryPointID => entryPointID;

        public ZoneTransitionAction ZoneTransitionAction => zoneTransitionAction;

        public (bool exists, GameObject zone, bool isActive) ZoneStatus(GameObject zonePrefab)
        {
            bool zoneExists = false;
            GameObject zone = null;
            bool zoneActive = false;

            foreach (Transform child in transform)
            {
                if (child.name.Equals(zonePrefab.name))
                {
                    zoneExists = true;
                    zone = child.gameObject;
                    zoneActive = child.gameObject.activeSelf;
                    break;
                }
            }

            return (zoneExists, zone, zoneActive);
        }

        // TURN destroyActiveZoneOnLoad INTO AN ENUM with options NONE, DESTROY and DEACTIVATE

        // DISABLING AN ACTIVE ZONE WILL PREVENT IT FROM BEING DESTROYED AND WILL ALLOW IT TO BE REACTIVATED
        // DESTROYING AN ACTIVE ZONE WILL DESTROY IT AND WILL REQUIRE IT TO BE REINSTANTIATED

        // public void Spawn(GameObject zonePrefab) => Spawn(zonePrefab, null);

        public EntryPoint ResolveEntryPoint(ExitPoint exitPoint)
        {
            var entryPoints = activeZone.GetComponentsInChildren<EntryPoint>();

            foreach (var entryPoint in entryPoints)
            {
                if (entryPoint.EntryPointID == exitPoint.EntryPointID)
                {
                    return entryPoint;
                }
            }

            return null;
        }

        public ExitPoint ResolveExitPoint(EntryPoint entryPoint)
        {
            var exitPoints = activeZone.GetComponentsInChildren<ExitPoint>();

            foreach (var exitPoint in exitPoints)
            {
                if (exitPoint.EntryPointID == entryPoint.EntryPointID)
                {
                    return exitPoint;
                }
            }

            return null;
        }

        public BasePlayer ResolvePlayer()
        {
            var basePlayer = FindAnyObjectByType<BasePlayer>();

            if (basePlayer == null)
            {
                var zoneSetup = activeZone.GetComponentInChildren<ZoneSetup>();                
                var instance = Instantiate(playerPrefab, zoneSetup.DefaultEntryPoint.transform.position, Quaternion.identity);
                basePlayer = instance.GetComponent<BasePlayer>();
            }

            return basePlayer;
        }

        public GameObject SpawnZone(GameObject zonePrefab/*, PointID? entryPointID*//*, bool spawnPlayer = false*/)
        {
            // if (entryPointID != null)
            // {
            //     this.entryPointID = entryPointID.Value;
            // }

            GameObject zone = null;

            var zoneStatus = ZoneStatus(zonePrefab);

            if (zoneStatus.exists)
            {
                if (!zoneStatus.isActive)
                {
                    zoneStatus.zone.SetActive(true);
                }
                
                zone = zoneStatus.zone;
            }
            else
            {
                zone = Instantiate(zonePrefab, Vector3.zero, Quaternion.identity, transform);
                zone.name = zonePrefab.name;
            }

            // if (spawnPlayer)
            // {
            //     var basePlayer = FindAnyObjectByType<BasePlayer>();

            //     if (basePlayer == null)
            //     {
            //         var zoneSetup = zone.GetComponentInChildren<ZoneSetup>();                
            //         Instantiate(playerPrefab, zoneSetup.DefaultEntryPoint.transform.position, Quaternion.identity);
            //     }
            // }

            if (activeZone != null)
            {
                switch (zoneTransitionAction)
                {
                    case ZoneTransitionAction.DESPAWN:
                        Despawn(activeZone);
                        break;
                    case ZoneTransitionAction.DISABLE:
                        activeZone.SetActive(false);
                        break;
                }
            }

            activeZone = zone;
            return zone;

#if false
            if (activeZone == null || !activeZone.name.Equals(zonePrefab.name))
            {
                GameObject zone = null;

                foreach (Transform child in transform)
                {
                    if (child.name.Equals(zonePrefab.name))
                    {
                        if (!child.gameObject.activeSelf)
                        {
                            zone = child.gameObject;
                        }
                    }
                }

                // NEED TO CHECK IF THE ZONE IS ALREADS SPAWNED BEFORE INSTANTIATING A NEW ONE

                if (zone != null)
                {
                    zone.SetActive(true);
                }
                else
                {
                    zone = Instantiate(zonePrefab, Vector3.zero, Quaternion.identity, transform);
                    zone.name = zonePrefab.name;
                }              

                var basePlayer = FindAnyObjectByType<BasePlayer>();

                if (basePlayer == null)
                {
                    var zoneSetup = zone.GetComponentInChildren<ZoneSetup>();                
                    Instantiate(playerPrefab, zoneSetup.transform.position, Quaternion.identity);
                }




                if (zoneTransitionAction == ZoneTransitionAction.DESPAWN && activeZone != null)
                {
                    Despawn(activeZone);
                }

                if (activeZone != null)
                {
                    switch (zoneTransitionAction)
                    {
                        case ZoneTransitionAction.DESPAWN:
                            Despawn(activeZone);
                            break;
                        case ZoneTransitionAction.DISABLE:
                            activeZone.SetActive(false);
                            break;
                    }
                }

                activeZone = zone;
            }
#endif
        }

        public void Despawn(GameObject zone) => Destroy(zone);

        public void DespawnAll()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
