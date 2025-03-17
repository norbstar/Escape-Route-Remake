using UnityEngine;

using Tests;

namespace Zone
{
    [RequireComponent(typeof(ZoneManager))]
    public class ZoneTransitionManager : MonoBehaviour
    {
        private ZoneManager zoneManager;

        void Awake() => zoneManager = GetComponent<ZoneManager>();

        public virtual void Transition(ExitPoint exitPoint, GameObject zonePrefab, PointID entryPointID)
        {
            var basePlayer = (BasePlayer) BasePlayer.Instance;

            var activeZone = zoneManager.ActiveZone;

            if (activeZone != null)
            {
                var isSameZone = activeZone.name.Equals(zonePrefab.name);

                if (isSameZone || !isSameZone && zoneManager.ZoneTransitionAction != ZoneTransitionAction.PERSIST)
                {
                    var rigidBody = basePlayer.GetComponent<Rigidbody2D>();
                    rigidBody.linearVelocity = Vector3.zero;

                    basePlayer.Deactivate();
                }

                if (isSameZone)
                {
                    // The transition is internal to the active zone
                    var internalEntryPoints = activeZone.GetComponentsInChildren<EntryPoint>();

                    foreach (var entryPoint in internalEntryPoints)
                    {
                        if (entryPointID == entryPoint.EntryPointID)
                        {
                            basePlayer.gameObject.transform.position = entryPoint.transform.position;
                            basePlayer.Activate();
                            return;
                        }
                    }
                }

                if (zoneManager.ZoneTransitionAction == ZoneTransitionAction.PERSIST)
                {
                    var matchingEntryPoint = zoneManager.ResolveEntryPoint(exitPoint);
                    
                    if (matchingEntryPoint != null)
                    {
                        Destroy(matchingEntryPoint.gameObject);
                    }

                    Destroy(exitPoint.gameObject);
                }
            }

            var zone = zoneManager.SpawnZone(zonePrefab/*, entryPointID*/);
            var entryPoints = zone.GetComponentsInChildren<EntryPoint>();

            foreach (var entryPoint in entryPoints)
            {
                if (entryPointID == entryPoint.EntryPointID)
                {
                    if (zoneManager.ZoneTransitionAction == ZoneTransitionAction.PERSIST)
                    {
                        var matchingExitPoint = zoneManager.ResolveExitPoint(entryPoint);
                        Destroy(entryPoint.gameObject);

                        if (matchingExitPoint != null)
                        {
                            Destroy(matchingExitPoint.gameObject);
                        }
                    }
                    
                    if (!basePlayer.IsActive)
                    {
                        basePlayer.gameObject.transform.position = entryPoint.transform.position;
                        basePlayer.Activate();
                    }
                    return;
                }
            }
        }
    }
}
