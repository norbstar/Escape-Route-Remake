using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.States
{
    public class VariableJumpState : State
    {
        [Header("Configuration")]
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;

        private InputSystem_Actions inputActions;
        private bool jumpReleased, execJump;
        
        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable()
        {
            inputActions.Enable();
            inputActions.Player.JumpPress.performed += OnPressIntent;
            inputActions.Player.JumpRelease.performed += OnReleaseIntent;
        }

        void OnDisable()
        {
            inputActions.Player.JumpPress.performed -= OnPressIntent;
            inputActions.Player.JumpRelease.performed -= OnReleaseIntent;
            inputActions.Disable();
        }

        private IEnumerator Co_MonitorJumpIntent()
        {
            execJump = true;
            jumpReleased = false;

            while (!jumpReleased)
            {
                yield return null;
            }
            
            if (Essentials.PlayerState().HasFlag(PlayerStateEnum.Jumping))
            {
                Essentials.RigidBody().linearVelocityY = 0f;
                // Essentials.RigidBody().AddForce(Vector2.down * jumpForce);
            }
        }

        private void OnPressIntent(InputAction.CallbackContext context)
        {
            if (!Essentials.PlayerStateActivation().CanJump) return;

            if (Essentials.IsInputSuspended()) return;

            var canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            if (!canExec) return;

            if (!Essentials.IsGrounded()) return;

            StartCoroutine(Co_MonitorJumpIntent());
        }

        private void OnReleaseIntent(InputAction.CallbackContext context) => jumpReleased = true;

        private void ExecuteIntent()
        {
            var rigidBody = Essentials.RigidBody();
            rigidBody.AddForce(Vector2.up * jumpForce);
            var audioSource = Essentials.AudioSource();
            audioSource.PlayOneShot(jumpClip, 1f);
            
            execJump = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (execJump)
            {
                ExecuteIntent();
            }
        }
    }
}
