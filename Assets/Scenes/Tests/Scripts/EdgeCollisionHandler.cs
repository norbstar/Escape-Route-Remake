using UnityEngine;

namespace Tests
{
    public class EdgeCollisionHandler : MonoBehaviour
    {
        public enum Edge
        {
            Top,
            Right,
            Bottom,
            Left
        }

        [Header("Components")]
        [SerializeField] OnCollision2DHandler topEdgeCollider;
        [SerializeField] OnCollision2DHandler rightEdgeCollider;
        [SerializeField] OnCollision2DHandler bottomEdgeCollider;
        [SerializeField] OnCollision2DHandler leftEdgeCollider;

        public delegate void HasContactWithEdge(EdgeCollisionHandler instance, Collision2D collision, Edge edge);
        public delegate void HasLostContactWithEdge(EdgeCollisionHandler instance, Collision2D collision, Edge edge);

        public class Events
        {
            public HasContactWithEdge OnContact { get; set; }

            public HasLostContactWithEdge OnLostContact { get; set; }
        }

        private Events events;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            topEdgeCollider.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            rightEdgeCollider.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            bottomEdgeCollider.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            leftEdgeCollider.Register(new OnCollision2DHandler.Events
            {
                Gained = OnContactWithEdge,
                Lost = OnLostContactWithEdge
            });
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
