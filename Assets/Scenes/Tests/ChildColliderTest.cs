using UnityEngine;

namespace Tests
{
    public class ChildColliderTest : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] OnTrigger2DHandler trigger;

        public void OnGainedContact(OnTrigger2DHandler instance, Collider2D collider) => Debug.Log($"OnGainedContact {collider.name}");

        public void OnLostContact(OnTrigger2DHandler instance, Collider2D collider) => Debug.Log($"OnLostContact {collider.name}");

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            trigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContact,
                Lost = OnLostContact
            });
        }
    }
}