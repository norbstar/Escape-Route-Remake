using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ForceTest : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rigidBody;
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] float delaySec = 0.25f;

        private float startTime;
        private bool hasJumped;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startTime = Time.time;
            rigidBody.AddForce(Vector2.up * jumpForce);
        }

        // Update is called once per frame
        void Update()
        {
            if (!hasJumped && Time.time > startTime + delaySec)
            {
                rigidBody.AddForce(Vector2.up * jumpForce);
                hasJumped = true;
            }
        }
    }
}
