using UnityEngine;

using Cinemachine;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailTest : MonoBehaviour
    {
        [Header("Configuration")]
        [Range(-10f, 10f)]
        [SerializeField] float speed = 10f;

        private Rigidbody2D rigidBody;
        private TrailRenderer trailRenderer;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            trailRenderer = GetComponent<TrailRenderer>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            var virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                virtualCamera.Follow = transform;
            }
        }

        void FixedUpdate() => rigidBody.linearVelocityX = speed/* * Time.fixedDeltaTime*/;
    }
}
