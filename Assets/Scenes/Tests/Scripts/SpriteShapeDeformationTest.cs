using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Tests
{
    [RequireComponent(typeof(SpriteShapeRenderer))]
    [RequireComponent(typeof(SpriteShapeController))]
    [RequireComponent(typeof(EdgeCollider2D))]
    public class SpriteShapeDeformationTest : MonoBehaviour
    {
        [SerializeField] float speed = 2.5f;

        private SpriteShapeController spriteShapeController;
        private EdgeCollider2D edgeCollider2D;
        private int pointCount;
        private List<Vector2> points;

        void Awake()
        {
            spriteShapeController = GetComponent<SpriteShapeController>();
            edgeCollider2D = GetComponent<EdgeCollider2D>();
            points = new List<Vector2>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            pointCount = spriteShapeController.spline.GetPointCount();
            // Debug.Log($"Spline Point Count: {pointCount}");

    #if false
            for (int itr = 0; itr < pointCount; itr++)
            {
                var point = spriteShapeController.spline.GetPosition(itr);
                Debug.Log($"Point[{itr}] Position: [{point.x}, {point.y}]");

                if (itr == 0)
                {
                    spriteShapeController.spline.SetPosition(itr, new Vector3(-0.5f, -1f, 0.1f));
                }
            }
    #endif

            StartCoroutine(Co_DeformTopDown());
        }

        private IEnumerator Co_DeformTopDown()
        {
            var posY = 1f;
            float elapsedTime = 0f;

            var p1 = spriteShapeController.spline.GetPosition(1);
            var p2 = spriteShapeController.spline.GetPosition(2);

            while (posY > 0f)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(1f, 0f, elapsedTime * speed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
                yield return null;
            }

            StartCoroutine(Co_ReformTopDown());
        }

        private IEnumerator Co_ReformTopDown()
        {
            var posY = 0f;
            float elapsedTime = 0f;

            var p1 = spriteShapeController.spline.GetPosition(1);
            var p2 = spriteShapeController.spline.GetPosition(2);

            while (posY < 1f)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(0f, 1f, elapsedTime * speed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
                yield return null;
            }

            StartCoroutine(Co_DeformTopDown());
        }

    #if false
        private void UpdateEdgeCollider()
        {
            points.Clear();

            for (int itr = 0; itr < pointCount; itr++)
            {
                points.Add(spriteShapeController.spline.GetPosition(itr));
            }
            
            points.Add(spriteShapeController.spline.GetPosition(0));
            edgeCollider2D.SetPoints(points);
        }

        // Update is called once per frame
        void Update() => UpdateEdgeCollider();
    #endif
    }
}