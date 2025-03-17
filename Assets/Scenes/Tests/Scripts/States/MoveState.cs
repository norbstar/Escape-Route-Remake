using UnityEngine;

namespace Tests.States
{
    public class MoveState : State
    {
        [Header("Configuration")]
        [Range(1f, 10f)]
        [SerializeField] float speed = 10f;

        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execMove;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void EvaluateIntent()
        {
            if (!Essentials.PlayerStateActivation().CanMove) return;

            // if (Essentials.IsInputSuspended()) return;

            // var canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            // if (!canExec) return;

            // var canExec = !Essentials.IsCrouching();

            // if (!canExec) return;

            // moveValue = inputActions.Player.Move.ReadValue<Vector2>();

            // if (!Essentials.IsGrounded() || Essentials.IsCrouching() || Mathf.Abs(moveValue.x) == AbstractedStatePlayer.MIN_REGISTERED_VALUE) return;

            if (!Essentials.IsGrounded() || Essentials.IsCrouching()) return;
            
            execMove = true;
        }

        private void ExecuteIntent()
        {
            var rigidBody = Essentials.RigidBody();
            rigidBody.linearVelocityX = moveValue.x * speed;

            execMove = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // if (canExecute)
            // {
            //     ExecuteIntent();
            // }
            // else
            // {
            //     Essentials.RigidBody().linearVelocityX = 0f;
            // }

            EvaluateIntent();

            if (execMove)
            {
                ExecuteIntent();
            }
            else if (Essentials.IsGrounded())
            {
                Essentials.RigidBody().linearVelocityX = 0f;
            }
        }
    }
}
