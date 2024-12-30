using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(AnimationCurve))]
    public class VelocityTest : MonoBehaviour
    {
        [Header("Move")]
        [Range(0.1f, 10f)]
        [SerializeField] float moveSpeed = 0.5f;

        [Header("Stats")]
        // [SerializeField] AnimationCurve velocityCurve;
        [SerializeField] float velocity;

        private Rigidbody2D rigidBody;

        void Awake() => rigidBody = GetComponent<Rigidbody2D>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() => rigidBody.linearVelocityX = moveSpeed;

        // Update is called once per frame
        void Update() => velocity = rigidBody.linearVelocityX;
    }
}