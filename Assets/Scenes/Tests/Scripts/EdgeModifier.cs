using UnityEngine;

namespace Tests
{
    public class EdgeModifier : MonoBehaviour
    {
        protected class Coords
        {
            public float x1;
            public float y1;
            public float x2;
            public float y2;
        }

        [Header("Components")]
        [SerializeField] EdgeCollider2D topEdgeCollider;
        [SerializeField] EdgeCollider2D rightEdgeCollider;
        [SerializeField] EdgeCollider2D bottomEdgeCollider;
        [SerializeField] EdgeCollider2D leftEdgeCollider;

        private Coords defaultTopEdgeCoords, defaultRightEdgeCoords, defaultBottomEdgeCoords, defaultLeftEdgeCoords;

        public EdgeCollider2D TopEdgeCollider => topEdgeCollider;

        public EdgeCollider2D RightEdgeCollider => rightEdgeCollider;

        public EdgeCollider2D BottomEdgeCollider => bottomEdgeCollider;

        public EdgeCollider2D LeftEdgeCollider => leftEdgeCollider;

        public virtual void Awake()
        {
            defaultTopEdgeCoords = new Coords
            {
                x1 = topEdgeCollider.points[0].x,
                y1 = topEdgeCollider.points[0].y,
                x2 = topEdgeCollider.points[1].x,
                y2 = topEdgeCollider.points[1].y
            };

            defaultRightEdgeCoords = new Coords
            {
                x1 = rightEdgeCollider.points[0].x,
                y1 = rightEdgeCollider.points[0].y,
                x2 = rightEdgeCollider.points[1].x,
                y2 = rightEdgeCollider.points[1].y
            };

            defaultBottomEdgeCoords = new Coords
            {
                x1 = bottomEdgeCollider.points[0].x,
                y1 = bottomEdgeCollider.points[0].y,
                x2 = bottomEdgeCollider.points[1].x,
                y2 = bottomEdgeCollider.points[1].y
            };

            defaultLeftEdgeCoords = new Coords
            {
                x1 = leftEdgeCollider.points[0].x,
                y1 = leftEdgeCollider.points[0].y,
                x2 = leftEdgeCollider.points[1].x,
                y2 = leftEdgeCollider.points[1].y
            };
        }

        public void SetTopOffset(float offset)
        {
            Vector2[] points = topEdgeCollider.points;
            points.SetValue(new Vector2(points[0].x, defaultTopEdgeCoords.y1 + offset), 0);
            points.SetValue(new Vector2(points[1].x, defaultTopEdgeCoords.y2 + offset), 1);
            topEdgeCollider.points = points;

            points = leftEdgeCollider.points;
            points.SetValue(new Vector2(points[0].x, defaultLeftEdgeCoords.y1 + offset), 0);
            leftEdgeCollider.points = points;

            points = rightEdgeCollider.points;
            points.SetValue(new Vector2(points[0].x, defaultRightEdgeCoords.y1 + offset), 0);
            rightEdgeCollider.points = points;
        }
    }
}
