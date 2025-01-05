using UnityEngine;

namespace Tests.State
{
    public class CrouchWalkState : State
    {
        [Range(1f, 5f)]
        [SerializeField] float speed = 2.5f;

        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execCrouchWalk, canExec;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void Evaluate()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            execCrouchWalk = Essentials.IsGrounded() && Essentials.IsCrouching() && Mathf.Abs(moveValue.x) != 0f;
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

        private void ApplyCrouchWalk()
        {
            var isCrouched = Essentials.IsCrouching();
            Essentials.RigidBody().linearVelocityX = moveValue.x * speed;
        }

        void FixedUpdate()
        {
            if (!canExec) return;

            if (Essentials.IsInputSuspended()) return;

            if (execCrouchWalk)
            {
                ApplyCrouchWalk();
            }
            else if (Essentials.IsGrounded())
            {
                Essentials.RigidBody().linearVelocityX = 0f;
            }
        }
    }
}
