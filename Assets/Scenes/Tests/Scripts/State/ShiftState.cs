using UnityEngine;

namespace Tests.State
{
    public class ShiftState : State
    {
        [Range(1f, 10f)]
        [SerializeField] float speed = 5f;
        // [SerializeField] float smoothInputSpeed = 0.2f;
        
        private Vector2 moveValue;
        private bool execShift, canExec;
        // private Vector2 cachedMoveVector, smoothInputVelocity;

        private void Evaluate()
        {
            if (!Essentials.IsGrounded())
            {
                moveValue = Essentials.InputActions().Player.Move.ReadValue<Vector2>();

                if (Mathf.Abs(moveValue.x) != 0f)
                {
                    execShift = true;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            canExec = !((Essentials.IsGrabbable() || Essentials.IsTraversable()) && Essentials.IsHolding());

            if (canExec)
            {
                Evaluate();
            }
        }

        private void ApplyShift()
        {
            // cachedMoveVector = Vector2.SmoothDamp(cachedMoveVector, moveValue, ref smoothInputVelocity, smoothInputSpeed);
            // Essentials.RigidBody().linearVelocityX = cachedMoveVector.x * speed;
            Essentials.RigidBody().linearVelocityX = moveValue.x * speed;
            execShift = false;
        }

        void FixedUpdate()
        {
            if (!canExec) return;

            if (execShift)
            {
                ApplyShift();
            }
        }
    }
}
