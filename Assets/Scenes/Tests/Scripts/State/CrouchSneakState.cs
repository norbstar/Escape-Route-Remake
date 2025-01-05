using UnityEngine;

namespace Tests.State
{
    public class CrouchSneakState : AbstractCrouchState
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
        protected override void Update()
        {
            base.Update();

            canExec = !((Essentials.IsGrabbable() || Essentials.IsTraversable()) && Essentials.IsHolding());

            if (canExec)
            {
                canExec = !Essentials.IsSliding();
            }

            if (canExec)
            {
                Evaluate();
            }
        }

        private void ApplyCrouch()
        {
            if (Essentials.IsCrouching()) return;
            // Debug.Log($"ApplyCrouch");
            StartCoroutine(Co_Crouch());
        }

        private void ApplyReset()
        {
            if (!Essentials.IsCrouching()) return;
            // Debug.Log($"ApplyReset");
            StartCoroutine(Co_Reset());
        }

        private void ApplyMove()
        {
            // Debug.Log($"ApplyMove");
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
