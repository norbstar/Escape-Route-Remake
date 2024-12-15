using System.Collections.Generic;

using UnityEngine;

namespace Tests
{
    public class ContactMap : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;

        private Dictionary<int, GameObject> contacts;

        private int contactCount;

        void Awake() => contacts = new Dictionary<int, GameObject>();
    
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Debug.Log($"OnTriggerEnter2D Name: {collider.name}");

            if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
            {
                contacts.Add(collider.gameObject.GetInstanceID(), collider.gameObject);
                ++contactCount;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            // Debug.Log($"OnTriggerExit2D Name: {collider.name}");

            if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
            {
                contacts.Remove(collider.gameObject.GetInstanceID());
                --contactCount;
            }
        }

        public Dictionary<int, GameObject> Contacts => contacts;

        public bool HasContacts => contactCount > 0;
    }
}