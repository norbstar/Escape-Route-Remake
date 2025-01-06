using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(BaseEdgeModifier))]
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
        private BaseEdgeModifier baseEdgeModifier;
        private OnTrigger2DHandler topEdgeHandler, rightEdgeHandler, bottomEdgeHandler, leftEdgeHandler;
        private EdgeCollider2D topEdgeCollider, rightEdgeCollider, bottomEdgeCollider, leftEdgeCollider;

        void Awake()
        {
            layerMask = LayerMask.GetMask("Player");
            baseEdgeModifier = GetComponent<BaseEdgeModifier>();
            
            topEdgeCollider = baseEdgeModifier.TopEdgeCollider;
            topEdgeHandler = topEdgeCollider.GetComponent<OnTrigger2DHandler>();

            rightEdgeCollider = baseEdgeModifier.RightEdgeCollider;
            rightEdgeHandler = rightEdgeCollider.GetComponent<OnTrigger2DHandler>();

            bottomEdgeCollider = baseEdgeModifier.BottomEdgeCollider;
            bottomEdgeHandler = bottomEdgeCollider.GetComponent<OnTrigger2DHandler>();

            leftEdgeCollider = baseEdgeModifier.LeftEdgeCollider;
            leftEdgeHandler = leftEdgeCollider.GetComponent<OnTrigger2DHandler>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            topEdgeHandler.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            rightEdgeHandler.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            bottomEdgeHandler.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            leftEdgeHandler.Register(new OnTrigger2DHandler.Events
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
