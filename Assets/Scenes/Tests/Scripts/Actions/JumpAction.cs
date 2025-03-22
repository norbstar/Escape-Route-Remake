using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.Actions
{
    public class JumpAction : Action
    {
        [Header("Configuration")]
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;
        [SerializeField] bool canDoubleJump;
        // [SerializeField] int maxJumps = 2;

        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        private int jumpCount;
        private bool canDo;
        
        void Awake()
        {
            inputActions = Essentials.Transform().gameObject.GetComponent<CustomInputSystem>().InputActions;
            rigidBody = Essentials.RigidBody();
        }

        void OnEnable() => inputActions.Player.JumpPress.performed += OnIntent;

        void OnDisable() => inputActions.Player.JumpPress.performed -= OnIntent;

        private void OnIntent(InputAction.CallbackContext context)
        {
            if (!Essentials.PlayerStateActivation().CanJump) return;

            // if (Essentials.IsInputSuspended()) return;

            // var canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            // if (!canExec) return;

            if (jumpCount == 0 && !Essentials.IsGrounded()) return;

            if (canDoubleJump && jumpCount > 1 || !canDoubleJump && jumpCount > 0) return;

            // if (jumpCount == maxJumps) return;

            canDo = true;
        }
        
        private void Execute()
        {
            rigidBody.linearVelocityY = 0f;
            rigidBody.AddForce(Vector2.up * jumpForce);
            var audioSource = Essentials.AudioSource();
            audioSource.PlayOneShot(jumpClip, 1f);
            ++jumpCount;

            canDo = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // if (canExecute)
            // {
            //     ExecuteIntent();
            // }

            if (canDo)
            {
                Execute();
            }
        }

        public override void OnGrounded() => jumpCount = 0;
    }
}
