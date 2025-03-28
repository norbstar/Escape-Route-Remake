using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(EdgeModifier))]
    public class EdgeTriggerHandler : MonoBehaviour
    {
        public delegate void HasContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, Edge edge);
        public delegate void HasLostContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, Edge edge);

        public enum Edge
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public class Events
        {
            public HasContactWithEdge OnContact { get; set; }

            public HasLostContactWithEdge OnLostContact { get; set; }
        }

        private Events events;
        private LayerMask layerMask;
        private EdgeModifier edgeModifier;
        private OnTrigger2DHandler topEdgeHandler, rightEdgeHandler, bottomEdgeHandler, leftEdgeHandler;
        private EdgeCollider2D topEdgeCollider, rightEdgeCollider, bottomEdgeCollider, leftEdgeCollider;

        void Awake()
        {
            layerMask = LayerMask.GetMask("Player");
            edgeModifier = GetComponent<EdgeModifier>();
            
            topEdgeCollider = edgeModifier.TopEdgeCollider;
            topEdgeHandler = topEdgeCollider.GetComponent<OnTrigger2DHandler>();

            rightEdgeCollider = edgeModifier.RightEdgeCollider;
            rightEdgeHandler = rightEdgeCollider.GetComponent<OnTrigger2DHandler>();

            bottomEdgeCollider = edgeModifier.BottomEdgeCollider;
            bottomEdgeHandler = bottomEdgeCollider.GetComponent<OnTrigger2DHandler>();

            leftEdgeCollider = edgeModifier.LeftEdgeCollider;
            leftEdgeHandler = leftEdgeCollider.GetComponent<OnTrigger2DHandler>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            topEdgeHandler.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            rightEdgeHandler.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            bottomEdgeHandler.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            leftEdgeHandler.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);
        }

        public void Register(Events events) => this.events = events;

        public void OnContactWithEdge(OnTrigger2DHandler instance, Collider2D collider)
        {
            if (instance == topEdgeHandler)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Top);
            }
            else if (instance == rightEdgeHandler)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Right);
            }
            else if (instance == bottomEdgeHandler)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Bottom);
            }
            else if (instance == leftEdgeHandler)
            {
                events?.OnContact?.Invoke(this, collider, Edge.Left);
            }
        }

        public void OnLostContactWithEdge(OnTrigger2DHandler instance, Collider2D collider)
        {
            if (instance == topEdgeHandler)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Top);
            }
            else if (instance == rightEdgeHandler)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Right);
            }
            else if (instance == bottomEdgeHandler)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Bottom);
            }
            else if (instance == leftEdgeHandler)
            {
                events?.OnLostContact?.Invoke(this, collider, Edge.Left);
            }
        }
    }
}
