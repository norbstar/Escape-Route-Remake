using UnityEngine;

using Tests;

namespace Zone
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] PointID entryPointID;
        // [SerializeField] PointFXType pointFXType;

        public PointID EntryPointID => entryPointID;

        // void OnEnable()
        // {
        //     var basePlayer = (BasePlayer) BasePlayer.Instance;

        //     if (basePlayer == null) return;

        //     var zoneManager = FindAnyObjectByType<ZoneManager>();

        //     if (zoneManager == null || entryPointID != zoneManager.EntryPointID) return;

        //     basePlayer.gameObject.transform.position = transform.position;
        //     basePlayer.Activate();
        // }
    }
}
