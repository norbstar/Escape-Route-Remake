using UnityEngine;

namespace Tests
{
    public class CallbackTimingsTest : MonoBehaviour
    {
        // [SerializeField] OnTrigger2DHandler trigger;
        [SerializeField] OnCollision2DHandler trigger;

        private float startTime, endTime;
        private float registeredStartTime, registeredEndTime;
        private LayerMask layerMask;

        void Awake() => layerMask = LayerMask.GetMask("Player");

        void OnEnable()
        {
            // trigger.Register(new OnTrigger2DHandler.Events
            // {
            //     Gained = OnGainedContactViaRegistration,
            //     Lost = OnLostContactViaRegistration
            // });

            trigger.Register(new OnCollision2DHandler.Events
            {
                Gained = OnGainedContactViaRegistration
            }, layerMask);
        }

        // public void OnGainedContactViaRegistration(OnTrigger2DHandler instance, Collider2D collider)
        // {
        //     Debug.Log($"OnGainedContactViaRegistration");
        //     registeredStartTime = Time.time;
        // }

        // public void OnLostContactViaRegistration(OnTrigger2DHandler instance, Collider2D collider)
        // {
        //     Debug.Log($"OnLostContactViaRegistration");
        //     registeredEndTime = Time.time;
        //     Debug.Log($"Registered Duration: {registeredEndTime - registeredStartTime}");
        // }

        // private void OnTriggerEnter2D(Collider2D collider)
        // {
        //     Debug.Log($"OnTriggerEnter2D");
        //     startTime = Time.time;
        // }

        // private void OnTriggerExit2D(Collider2D collider)
        // {
        //     Debug.Log($"OnTriggerExit2D");
        //     endTime = Time.time;
        //     Debug.Log($"Duration: {endTime - startTime}");
        // }

        public void OnGainedContactViaRegistration(OnCollision2DHandler instance, Collision2D collision)
        {
            registeredEndTime = Time.time;
            Debug.Log($"OnGainedContactViaRegistration Duration: {registeredEndTime - registeredStartTime}");
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            endTime = Time.time;
            Debug.Log($"OnCollisionEnter2D Duration: {endTime - startTime}");
        }
    }
}
