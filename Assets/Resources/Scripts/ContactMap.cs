using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Tests
{
    public class ContactMap : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;

        // [Header("Analytics")]
        // [SerializeField] List<string> contactNames;

        public class ContactInfo
        {
            public ObjectProperties properties;
            public int sortingOrder;
        }

        public delegate void HasContact(ContactMap instance, Collider2D collider, List<ContactInfo> contactInfo);
        public delegate void HasLostContact(ContactMap instance, Collider2D collider, List<ContactInfo> contactInfo);

        public class Events
        {
            public HasContact OnContact { get; set; }

            public HasLostContact OnLostContact { get; set; }
        }

        private Dictionary<int, ContactInfo> contacts;
        private int count;
        private Events events;

        void Awake() => contacts = new Dictionary<int, ContactInfo>();
    
        public void Register(Events events) => this.events = events;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
            {
                if (collider.gameObject.TryGetComponent<ObjectProperties>(out var objectProperties))
                {
                    if (collider.gameObject.TryGetComponent<Renderer>(out var renderer))
                    {
                        contacts.Add(collider.gameObject.GetInstanceID(), new ContactInfo
                        {
                            properties = objectProperties,
                            sortingOrder = renderer.sortingOrder
                        });

                        var sortedContacts = contacts.OrderByDescending(c => c.Value.sortingOrder);
                        // contactNames = sortedContacts.Select(c => c.Value.properties.name).ToList();
                        events?.OnContact?.Invoke(this, collider, sortedContacts.Select(c => c.Value).ToList());
                        ++count;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
            {
                if (contacts.Remove(collider.gameObject.GetInstanceID()))
                {
                    var sortedContacts = contacts.OrderByDescending(c => c.Value.sortingOrder);
                    // contactNames = sortedContacts.Select(c => c.Value.properties.name).ToList();
                    events?.OnLostContact?.Invoke(this, collider, sortedContacts.Select(c => c.Value).ToList());
                    --count;
                }
            }
        }

        public List<ContactInfo> Contacts => contacts.Select(c => c.Value).ToList();

        public bool HasContacts => count > 0;
    }
}