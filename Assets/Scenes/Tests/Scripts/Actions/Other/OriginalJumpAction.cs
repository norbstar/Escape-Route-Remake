using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.Actions
{
    public class OriginalJumpAction : Action
    {
        [Header("Configuration")]
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;
        [SerializeField] bool canPowerJump;
        [Range(600f, 1000f)]
        [SerializeField] float powerJumpForce = 800f;
        [SerializeField] float powerJumpThreshold = 0.2f;

        private InputSystem_Actions inputActions;
        private float jumpPressStartTime;
        private bool jumpReleased, execJump, execPowerJump;
        
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
            jumpPressStartTime = Time.time;
            jumpReleased = false;

            while (true)
            {
                if (jumpReleased)
                {
                    execJump = true;
                    break;
                }
                else if (canPowerJump && Essentials.IsGrounded() && Time.time - jumpPressStartTime > powerJumpThreshold)
                {
                    execPowerJump = true;
                    break;
                }

                yield return null;
            }
        }

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            if (!Essentials.IsGrounded()) return;
            StartCoroutine(Co_MonitorJumpIntent());
        }

        private void OnJumpReleaseIntent(InputAction.CallbackContext context) => jumpReleased = true;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (execJump)
            {
                Essentials.RigidBody().AddForce(Vector2.up * jumpForce);
                Essentials.AudioSource().PlayOneShot(jumpClip, 1f);
                execJump = false;
            }
            else if (execPowerJump)
            {
                Essentials.RigidBody().AddForce(Vector2.up * powerJumpForce);
                Essentials.AudioSource().PlayOneShot(jumpClip, 1f);
                execPowerJump = false;
            }
        }
    }
}
