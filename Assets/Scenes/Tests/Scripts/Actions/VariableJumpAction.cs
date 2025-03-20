using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.Actions
{
    public class VariableJumpAction : Action
    {
        [Header("Configuration")]
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] AudioClip jumpClip;

        private InputSystem_Actions inputActions;
        private bool jumpReleased, canDo;
        
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
            canDo = true;
            jumpReleased = false;

            while (!jumpReleased)
            {
                yield return null;
            }
            
            // if (Essentials.PlayerState().HasFlag(PlayerStateEnum.Jumping))
            // {
            //     Essentials.RigidBody().linearVelocityY = 0f;
            // }

            var moveValue = inputActions.Player.Move.ReadValue<Vector2>();

            if (moveValue.y < -AbstractedPlayer.MIN_REGISTERED_VALUE)
            {
                Essentials.RigidBody().AddForce(Vector2.down * jumpForce);
            }
            else
            {
                Essentials.RigidBody().linearVelocityY = 0f;
            }
        }

        private void OnPressIntent(InputAction.CallbackContext context)
        {
            if (!Essentials.PlayerStateActivation().CanJump) return;

            // if (Essentials.IsInputSuspended()) return;

            // var canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            // if (!canExec) return;

            if (!Essentials.IsGrounded()) return;

            StartCoroutine(Co_MonitorJumpIntent());
        }

        private void OnReleaseIntent(InputAction.CallbackContext context) => jumpReleased = true;

        private void Execute()
        {
            var rigidBody = Essentials.RigidBody();
            rigidBody.AddForce(Vector2.up * jumpForce);
            var audioSource = Essentials.AudioSource();
            audioSource.PlayOneShot(jumpClip, 1f);
            
            canDo = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (canDo)
            {
                Execute();
            }
        }
    }
}
