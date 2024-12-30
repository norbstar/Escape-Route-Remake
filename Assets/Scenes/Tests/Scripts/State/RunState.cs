using UnityEngine;

namespace Tests.State
{
    public class RunState : State
    {
        [Range(1f, 10f)]
        [SerializeField] float speed = 5f;
        // [SerializeField] float smoothInputSpeed = 0.2f;

        private Vector2 moveValue;
        private bool execRun;
        // private Vector2 cachedMoveVector, smoothInputVelocity;

        private void Evaluate()
        {
            // if (Essentials.IsGrounded())
            // {
            //     moveValue = Essentials.InputActions().Player.Move.ReadValue<Vector2>();

            //     if (Mathf.Abs(moveValue.x) != 0f)
            //     {
            //         execRun = true;
            //     }
            // }

            moveValue = Essentials.InputActions().Player.Move.ReadValue<Vector2>();
            execRun = Essentials.IsGrounded() && Mathf.Abs(moveValue.x) != 0f; 
        }

        // Update is called once per frame
        void Update() => Evaluate();

        private void ApplyRun()
        {
            // cachedMoveVector = Vector2.SmoothDamp(cachedMoveVector, moveValue, ref smoothInputVelocity, smoothInputSpeed);
            // Essentials.RigidBody().linearVelocityX = cachedMoveVector.x * speed;
            Essentials.RigidBody().linearVelocityX = moveValue.x * speed;
            // execRun = false;
        }

        void FixedUpdate()
        {
            if (execRun)
            {
                ApplyRun();
            }
            else if (Essentials.IsGrounded())
            {
                Essentials.RigidBody().linearVelocityX = 0f;
            }
        }
    }
}
