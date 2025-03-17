using UnityEngine;

namespace Tests.States
{
    public class SneakState : CrouchState
    {
        [Range(1f, 5f)]
        [SerializeField] float speed = 5f;

        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execCrouch, execMove, canExec;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void EvaluateIntent()
        {
            if (!Essentials.PlayerStateActivation().CanSneak) return;

            var crouchValue = inputActions.Player.Crouch.IsPressed();
            execCrouch = Essentials.IsGrounded() && crouchValue;

            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            execMove = Essentials.IsGrounded() && Essentials.IsCrouching() && Mathf.Abs(moveValue.x) != AbstractedStatePlayer.MIN_REGISTERED_VALUE;
        }

        // Update is called once per frame
        void Update()
        {
            canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            if (canExec)
            {
                canExec = !Essentials.IsSliding();
            }

            if (canExec)
            {
                EvaluateIntent();
            }
        }

        private void ExecuteCrouch() => Crouch();

        private void ExecuteMove()
        {
            Essentials.RigidBody().linearVelocityX = moveValue.x * speed;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!canExec) return;

            if (Essentials.IsInputSuspended()) return;

            if (execCrouch)
            {
                ExecuteCrouch();
            }
            else
            {
                Reset();
            }

            if (execMove)
            {
                ExecuteMove();
            }
            else if (Essentials.IsGrounded())
            {
                Essentials.RigidBody().linearVelocityX = 0f;
            }
        }
    }    
}
