using UnityEngine;

namespace Tests.States
{
    public class ShiftState : State
    {
        [Header("Configuration")]
        [Range(1f, 10f)]
        [SerializeField] float speed = 5f;
        // [SerializeField] float smoothInputSpeed = 0.2f;
        
        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execShift, canExec;
        // private Vector2 cachedMoveVector, smoothInputVelocity;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();
        
        private void Evaluate()
        {
            if (!Essentials.IsGrounded())
            {
                moveValue = inputActions.Player.Move.ReadValue<Vector2>();

                if (Mathf.Abs(moveValue.x) != 0f)
                {
                    execShift = true;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

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

            if (Essentials.IsInputSuspended()) return;

            if (execShift)
            {
                ApplyShift();
            }
        }
    }
}
