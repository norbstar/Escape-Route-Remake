using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.State
{
    public class LeapState : State
    {
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;

        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execJump, canExec;

        void Awake() => inputActions = Essentials.InputActions();

        void OnEnable() => inputActions.Player.JumpPress.performed += OnJumpPressIntent;

        void OnDisable() => inputActions.Player.JumpPress.performed -= OnJumpPressIntent;

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            if (canExec) execJump = true;
        }
        
        private void Evaluate()
        {
            moveValue = Essentials.InputActions().Player.Move.ReadValue<Vector2>();
            Essentials.RigidBody().linearVelocity = Vector2.zero;
        }

        // Update is called once per frame
        void Update()
        {
            canExec = Essentials.IsGrabbable() && Essentials.IsHolding();
            Essentials.ShowArrow(canExec);

            if (canExec)
            {
                Evaluate();
            }
        }

        private void ApplyJump()
        {
            var gameObject = Essentials.GrabbableGameObject();

            if (gameObject.TryGetComponent<Grabbable>(out var grabbable))
            {
                grabbable.DisableColliderTemporarily();
            }

            Essentials.RigidBody().AddForce(moveValue * jumpForce);
            Essentials.AudioSource().PlayOneShot(jumpClip, 1f);
            execJump = false;
        }

        void FixedUpdate()
        {
            if (!canExec) return;

            if (execJump)
            {
                ApplyJump();
            }
        }
    }
}
