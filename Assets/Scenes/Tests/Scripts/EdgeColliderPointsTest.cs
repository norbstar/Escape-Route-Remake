using UnityEngine;

namespace Tests
{
    public class EdgeColliderPointsTest : MonoBehaviour
    {
        private EdgeCollider2D edgeCollider;

        void Awake() => edgeCollider = GetComponent<EdgeCollider2D>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // edgeCollider.points[0].y += 1f;

            Vector2[] points = edgeCollider.points;

            points.SetValue(new Vector2(-0.35f, -0.15f), 0);
            
            edgeCollider.points = points;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
