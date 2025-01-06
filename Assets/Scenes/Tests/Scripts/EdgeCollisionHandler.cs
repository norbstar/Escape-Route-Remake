using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(BaseEdgeModifier))]
    public class EdgeCollisionHandler : MonoBehaviour
    {
        public delegate void HasContactWithEdge(EdgeCollisionHandler instance, Collision2D collision, Edge edge);
        public delegate void HasLostContactWithEdge(EdgeCollisionHandler instance, Collision2D collision, Edge edge);

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
        private OnCollision2DHandler topEdgeHandler, rightEdgeHandler, bottomEdgeHandler, leftEdgeHandler;
        private EdgeCollider2D topEdgeCollider, rightEdgeCollider, bottomEdgeCollider, leftEdgeCollider;

        void Awake()
        {
            layerMask = LayerMask.GetMask("Player");
            baseEdgeModifier = GetComponent<BaseEdgeModifier>();
            
            topEdgeCollider = baseEdgeModifier.TopEdgeCollider;
            topEdgeHandler = topEdgeCollider.GetComponent<OnCollision2DHandler>();

            rightEdgeCollider = baseEdgeModifier.RightEdgeCollider;
            rightEdgeHandler = rightEdgeCollider.GetComponent<OnCollision2DHandler>();

            bottomEdgeCollider = baseEdgeModifier.BottomEdgeCollider;
            bottomEdgeHandler = bottomEdgeCollider.GetComponent<OnCollision2DHandler>();

            leftEdgeCollider = baseEdgeModifier.LeftEdgeCollider;
            leftEdgeHandler = leftEdgeCollider.GetComponent<OnCollision2DHandler>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            topEdgeHandler.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            rightEdgeHandler.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            bottomEdgeHandler.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            leftEdgeHandler.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);
        }

        public void Register(Events events) => this.events = events;

        public void OnContactWithEdge(OnCollision2DHandler instance, Collision2D collision)
        {
            if (instance == topEdgeCollider)
            {
                events?.OnContact?.Invoke(this, collision, Edge.Top);
            }
            else if (instance == rightEdgeCollider)
            {
                events?.OnContact?.Invoke(this, collision, Edge.Right);
            }
            else if (instance == bottomEdgeCollider)
            {
                events?.OnContact?.Invoke(this, collision, Edge.Bottom);
            }
            else if (instance == leftEdgeCollider)
            {
                events?.OnContact?.Invoke(this, collision, Edge.Left);
            }
        }

        public void OnLostContactWithEdge(OnCollision2DHandler instance, Collision2D collision)
        {
            if (instance == topEdgeCollider)
            {
                events?.OnLostContact?.Invoke(this, collision, Edge.Top);
            }
            else if (instance == rightEdgeCollider)
            {
                events?.OnLostContact?.Invoke(this, collision, Edge.Right);
            }
            else if (instance == bottomEdgeCollider)
            {
                events?.OnLostContact?.Invoke(this, collision, Edge.Bottom);
            }
            else if (instance == leftEdgeCollider)
            {
                events?.OnLostContact?.Invoke(this, collision, Edge.Left);
            }
        }
    }
}
