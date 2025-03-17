using UnityEngine;

namespace Zone
{
    public class ZoneSetup : MonoBehaviour
    {
        [SerializeField] Transform defaultEntryPoint;

        public Transform DefaultEntryPoint => defaultEntryPoint;
    }    
}
