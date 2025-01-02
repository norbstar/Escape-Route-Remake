using UnityEngine;

namespace Tests
{
    public class ObjectProperties : MonoBehaviour
    {
        [SerializeField] ObjectPropertyEnum properties;
        
        public ObjectPropertyEnum Properties => properties;

        public bool Contains(ObjectPropertyEnum properties) => this.properties.HasFlag(properties);
    }
}
