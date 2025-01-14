using UnityEngine;

namespace Tests
{
    public class BaseEdgeHandler : MonoBehaviour
    {
        protected class Coords
        {
            public float x1;
            public float y1;
            public float x2;
            public float y2;
        }

        [Header("Components")]
        [SerializeField] protected EdgeCollider2D topEdgeCollider;
        [SerializeField] protected EdgeCollider2D rightEdgeCollider;
        [SerializeField] protected EdgeCollider2D bottomEdgeCollider;
        [SerializeField] protected EdgeCollider2D leftEdgeCollider;

        private Coords defaultTopEdgeCoords, defaultRightEdgeCoords, defaultBottomEdgeCoords, defaultLeftEdgeCoords;

        public virtual void Awake()
        {
            defaultTopEdgeCoords = new Coords
            {
                x1 = topEdgeCollider.points[0].x,
                y1 = topEdgeCollider.points[0].y,
                x2 = topEdgeCollider.points[1].x,
                y2 = topEdgeCollider.points[1].y
            };
            // Debug.Log($"DefaultTopEdgeCoords: {defaultTopEdgeCoords}");

            defaultRightEdgeCoords = new Coords
            {
                x1 = rightEdgeCollider.points[0].x,
                y1 = rightEdgeCollider.points[0].y,
                x2 = rightEdgeCollider.points[1].x,
                y2 = rightEdgeCollider.points[1].y
            };
            // Debug.Log($"DefaultRightEdgeCoords: {defaultRightEdgeCoords}");

            defaultBottomEdgeCoords = new Coords
            {
                x1 = bottomEdgeCollider.points[0].x,
                y1 = bottomEdgeCollider.points[0].y,
                x2 = bottomEdgeCollider.points[1].x,
                y2 = bottomEdgeCollider.points[1].y
            };
            // Debug.Log($"DefaultBottomEdgeCoords: {defaultBottomEdgeCoords}");

            defaultLeftEdgeCoords = new Coords
            {
                x1 = leftEdgeCollider.points[0].x,
                y1 = leftEdgeCollider.points[0].y,
                x2 = leftEdgeCollider.points[1].x,
                y2 = leftEdgeCollider.points[1].y
            };
            // Debug.Log($"DefaultLeftEdgeCoords: {defaultLeftEdgeCoords}");
        }

        public void OffsetHeight(float offset)
        {
            topEdgeCollider.points[0].y = defaultTopEdgeCoords.y1 - offset;
            topEdgeCollider.points[1].y = defaultTopEdgeCoords.y2 - offset;
            leftEdgeCollider.points[0].y = defaultLeftEdgeCoords.y1 - offset;
            rightEdgeCollider.points[0].y = defaultRightEdgeCoords.y1 - offset;
        }
    }
}
