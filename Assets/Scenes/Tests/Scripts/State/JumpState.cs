using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.State
{
    public class JumpState : State
    {
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;
        [SerializeField] bool canDoubleJump;
        // [SerializeField] int maxJumps = 2;

        private InputSystem_Actions inputActions;
        private bool execJump, canExec;
        private int jumpCount;
        
        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable()
        {
            inputActions.Enable();
            inputActions.Player.JumpPress.performed += OnJumpPressIntent;
        }

        void OnDisable()
        {
            inputActions.Player.JumpPress.performed -= OnJumpPressIntent;
            inputActions.Disable();
        }

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            if (!canExec) return;

            if (jumpCount == 0 && !Essentials.IsGrounded()) return;

            if (canDoubleJump && jumpCount > 1 || !canDoubleJump && jumpCount > 0) return;
            // if (jumpCount == maxJumps) return;
            execJump = true;
        }

        // Update is called once per frame
        void Update() => canExec = !((Essentials.IsGrabbable() || Essentials.IsTraversable()) && Essentials.IsHolding());

        void FixedUpdate()
        {
            if (!canExec) return;

            if (execJump)
            {
                Essentials.RigidBody().linearVelocityY = 0f;
                Essentials.RigidBody().AddForce(Vector2.up * jumpForce);
                Essentials.AudioSource().PlayOneShot(jumpClip, 1f);
                ++jumpCount;
                execJump = false;
            }
        }

        public override void OnGrounded() => jumpCount = 0;
    }
}
