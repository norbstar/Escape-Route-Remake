using UnityEngine;

namespace Tests
{
    public class ObjectProperties : MonoBehaviour
    {
        [SerializeField] ObjectPropertyEnum properties;
        
        public ObjectPropertyEnum Properties => properties;
    }
}
