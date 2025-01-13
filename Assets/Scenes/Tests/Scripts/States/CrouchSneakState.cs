using UnityEngine;

namespace Tests.States
{
    public class CrouchSneakState : BaseCrouchState
    {
        [Range(1f, 5f)]
        [SerializeField] float speed = 5f;

        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execCrouch, execMove, canExec;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void Evaluate()
        {
            var crouchValue = inputActions.Player.Crouch.IsPressed();
            execCrouch = Essentials.IsGrounded() && crouchValue;

            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            execMove = Essentials.IsGrounded() && Essentials.IsCrouching() && Mathf.Abs(moveValue.x) != 0f;
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            if (canExec)
            {
                canExec = !Essentials.IsSliding();
            }

            if (canExec)
            {
                Evaluate();
            }
        }

        private void ApplyCrouch() => Crouch();

        private void ApplyReset() => Reset();

        private void ApplyMove()
        {
            Essentials.RigidBody().linearVelocityX = moveValue.x * speed;
            // execMove = false;
        }

        void FixedUpdate()
        {
            if (!canExec) return;

            if (Essentials.IsInputSuspended()) return;

            if (execCrouch)
            {
                ApplyCrouch();
            }
            else
            {
                ApplyReset();
            }

            if (execMove)
            {
                ApplyMove();
            }
            else if (Essentials.IsGrounded())
            {
                Essentials.RigidBody().linearVelocityX = 0f;
            }
        }
    }    
}
