using UnityEngine;

namespace Tests
{
    public class VelocityDamping : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] Rigidbody2D targetA;
        [SerializeField] Rigidbody2D targetB;
        [SerializeField] Rigidbody2D targetC;
        [SerializeField] Rigidbody2D targetD;

        [Header("Move")]
        [Range(10f, 50f)]
        [SerializeField] float moveSpeed = 25f;

        private bool preventUpdate;

        void FixedUpdate()
        {
            if (preventUpdate) return;

            targetA.linearVelocityX = 1f * moveSpeed;
            targetB.linearVelocityX = 1f * moveSpeed;
            targetC.linearVelocityX = 1f * moveSpeed;
            targetD.linearVelocityX = 1f * moveSpeed;

            preventUpdate = true;
        }
    }
}