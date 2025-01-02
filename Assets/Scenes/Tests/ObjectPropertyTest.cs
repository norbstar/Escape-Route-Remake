using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(ObjectProperties))]
    public class ObjectPropertyTest : MonoBehaviour
    {
        private ObjectProperties objectProperties;
        
        void Awake() => objectProperties = GetComponent<ObjectProperties>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            var isGrabbable = objectProperties.Contains(ObjectPropertyEnum.Grabbable);
            Debug.Log($"Is Grabbable: {isGrabbable}");

            var isTraversable = objectProperties.Contains(ObjectPropertyEnum.Traversable);
            Debug.Log($"Is Traversable: {isTraversable}");
        }
    }
}
