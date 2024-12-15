using System.Collections.Generic;

using UnityEngine;

using UI;

namespace Tests
{
    public class ContactMapTest : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] AttributeUI hasContactUI;

        [SerializeField] LayerMask layerMask;

        public static Color ORANGE;

        private Dictionary<int, GameObject> contacts;

        private int contactCount;

        void Awake() => contacts = new Dictionary<int, GameObject>();
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ColorUtility.TryParseHtmlString("#FF6100", out Color orange);
            ORANGE = orange;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Debug.Log($"OnTriggerEnter2D {collider.name}");

            // if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
            // {
            //     Debug.Log($"OnTriggerEnter2D {collider.name} in layermask");
            // }
            // else
            // {
            //     Debug.Log($"OnTriggerEnter2D {collider.name} NOT in layermask");
            // }

            if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
            {
                contacts.Add(collider.gameObject.GetInstanceID(), collider.gameObject);
                ++contactCount;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            // Debug.Log($"OnTriggerExit2D {collider.name}");

            if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
            {
                contacts.Remove(collider.gameObject.GetInstanceID());
                --contactCount;
            }
        }

        public Dictionary<int, GameObject> Contacts => contacts;

        // Update is called once per frame
        void Update()
        {
            hasContactUI.Value = (contactCount > 0).ToString();
            hasContactUI.Color = contactCount > 0 ? Color.green : ORANGE;
        }
    }
}