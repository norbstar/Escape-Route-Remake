using UnityEngine;

namespace Tests
{
    public class EdgeTriggerHandler : MonoBehaviour
    {
        public enum Edge
        {
            Top,
            Right,
            Bottom,
            Left
        }

        [Header("Components")]
        [SerializeField] OnTrigger2DHandler topEdgeTrigger;
        [SerializeField] OnTrigger2DHandler rightEdgeTrigger;
        [SerializeField] OnTrigger2DHandler bottomEdgeTrigger;
        [SerializeField] OnTrigger2DHandler leftEdgeTrigger;

        public delegate void HasContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, Edge edge);
        public delegate void HasLostContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, Edge edge);

        public class Events
        {
            public HasContactWithEdge OnContact { get; set; }

            public HasLostContactWithEdge OnLostContact { get; set; }
        }

        private Events events;
        private LayerMask layerMask;

        void Awake() => layerMask = LayerMask.GetMask("Player");

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            topEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            rightEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            bottomEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            leftEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);
        }

        public void Register(Events events) => this.events = events;

        public void OnContactWithEdge(OnTrigger2DHandler instance, Collider2D collider)
        {
            if (instance == topEdgeTrigger)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Top);
            }
            else if (instance == rightEdgeTrigger)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Right);
            }
            else if (instance == bottomEdgeTrigger)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Bottom);
            }
            else if (instance == leftEdgeTrigger)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Left);
            }
        }

        public void OnLostContactWithEdge(OnTrigger2DHandler instance, Collider2D collider)
        {
            if (instance == topEdgeTrigger)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Top);
            }
            else if (instance == rightEdgeTrigger)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Right);
            }
            else if (instance == bottomEdgeTrigger)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Bottom);
            }
            else if (instance == leftEdgeTrigger)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Left);
            }
        }
    }
}
