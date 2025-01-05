using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.State
{
    public class VariableJumpState : State
    {
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;

        private InputSystem_Actions inputActions;
        private bool jumpReleased, execJump;
        
        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable()
        {
            inputActions.Enable();
            inputActions.Player.JumpPress.performed += OnJumpPressIntent;
            inputActions.Player.JumpRelease.performed += OnJumpReleaseIntent;
        }

        void OnDisable()
        {
            inputActions.Player.JumpPress.performed -= OnJumpPressIntent;
            inputActions.Player.JumpRelease.performed -= OnJumpReleaseIntent;
            inputActions.Disable();
        }

        private IEnumerator Co_MonitorJumpIntent()
        {
            // Debug.Log($"Co_MonitorJumpIntent State: {Essentials.PlayerState()}");

            execJump = true;
            jumpReleased = false;

            while (Essentials.PlayerState() != PlayerStateEnum.Falling)
            {
                if (jumpReleased && !execJump) Essentials.RigidBody().linearVelocityY = 0f;
                yield return null;
            }

            // while (!jumpReleased)
            // {
            //     if (Essentials.PlayerState() == PlayerStateEnum.Jumping) Essentials.RigidBody().linearVelocityY = 0f;
            //     yield return null;
            // }
        }

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            if (!Essentials.IsGrounded()) return;
            StartCoroutine(Co_MonitorJumpIntent());
        }

        private void OnJumpReleaseIntent(InputAction.CallbackContext context) => jumpReleased = true;

        void FixedUpdate()
        {
            if (Essentials.IsInputSuspended()) return;

            if (execJump)
            {
                Essentials.RigidBody().AddForce(Vector2.up * jumpForce);
                Essentials.AudioSource().PlayOneShot(jumpClip, 1f);
                execJump = false;
            }
        }
    }
}
