using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.States
{
    public class JumpState : State
    {
        [Header("Configuration")]
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;
        [SerializeField] bool canDoubleJump;
        // [SerializeField] int maxJumps = 2;

        private InputSystem_Actions inputActions;
        private bool execJump;
        private int jumpCount;
        
        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable()
        {
            inputActions.Enable();
            inputActions.Player.JumpPress.performed += OnIntent;
        }

        void OnDisable()
        {
            inputActions.Player.JumpPress.performed -= OnIntent;
            inputActions.Disable();
        }

        private void OnIntent(InputAction.CallbackContext context)
        {
            if (!Essentials.PlayerStateActivation().CanJump) return;

            if (Essentials.IsInputSuspended()) return;

            var canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            if (!canExec) return;

            if (jumpCount == 0 && !Essentials.IsGrounded()) return;

            if (canDoubleJump && jumpCount > 1 || !canDoubleJump && jumpCount > 0) return;

            // if (jumpCount == maxJumps) return;

            execJump = true;
        }
        
        private void ExecuteIntent()
        {
            var rigidBody = Essentials.RigidBody();
            rigidBody.linearVelocityY = 0f;
            rigidBody.AddForce(Vector2.up * jumpForce);
            var audioSource = Essentials.AudioSource();
            audioSource.PlayOneShot(jumpClip, 1f);
            ++jumpCount;

            execJump = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // if (canExecute)
            // {
            //     ExecuteIntent();
            // }

            if (execJump)
            {
                ExecuteIntent();
            }
        }

        public override void OnGrounded() => jumpCount = 0;
    }
}
