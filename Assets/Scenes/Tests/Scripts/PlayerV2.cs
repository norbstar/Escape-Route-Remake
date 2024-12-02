using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

using UI;

namespace Tests
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerV2 : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] OnTrigger2DHandler groundTrigger;
        [SerializeField] Transform minEnergyThreshold;

        [Header("Audio")]
        [SerializeField] AudioClip jumpClip;
        [SerializeField] AudioClip landClip;
        [SerializeField] AudioClip dashClip;

        [Header("UI")]
        [SerializeField] AttributeUI isGroundedUI;
        [SerializeField] AttributeUI velocityXUI;
        [SerializeField] AttributeUI velocityYUI;
        [SerializeField] AttributeUI stateUI;

        [Header("Move")]
        [Range(5f, 10f)]
        [SerializeField] float moveSpeed = 5f;

        [Header("Jump")]
        [Range(200f, 1000f)]
        [SerializeField] float jumpForce = 800f;
        [Range(600f, 1400f)]
        [SerializeField] float powerJumpForce = 1200f;
        [SerializeField] float powerJumpThreshold = 0.2f;

        [Header("Dash")]
        [Range(10f, 100f)]
        [SerializeField] float dashSpeed = 50f;
        [SerializeField] float dashDuration = 0.05f;

        [Header("Move Stats")]
        [SerializeField] Vector2 rawValue;
        [SerializeField] float moveXValue;
        [SerializeField] float moveYValue;
        [SerializeField] Vector2 relativeMoveSpeed;
        [SerializeField] Vector2 relativeDodgeSpeed;

        [Header("Rigidbody")]
        [SerializeField] float velocityX;
        [SerializeField] float velocityY;

        [Header("Inferences")]
        [SerializeField] bool isGrounded;

        public static float POWER_MOVE_MIN_ENERGY_VALUE = 0.5f;
        public static float MIN_INPUT_VALUE = 0.1f;

        private InputActionMapping scene;
        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        private AudioSource audioSource;
        private PlayerState state;
        private bool execRun, execJump, execPowerJump, execDodge, execDash;
        private float jumpPressStartTime;
        private bool suspendInput, monitorDash, isDashing, jumpReleased;
        private int layerMask;

        void Awake()
        {
            scene = FindAnyObjectByType<InputActionMapping>();
            rigidBody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            inputActions = new InputSystem_Actions();
            layerMask = LayerMask.GetMask("Player");
        }

        void OnEnable()
        {
            inputActions.Enable();

            inputActions.Player.JumpPress.performed += OnJumpPressIntent;
            inputActions.Player.JumpRelease.performed += OnJumpReleaseIntent;
            inputActions.Player.Dash.performed += OnDashIntent;

            groundTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnGround,
                Lost = OffGround
            });
        }

        void OnDisable() => inputActions.Disable();

        private IEnumerator Co_MonitorJumpIntent()
        {
            jumpPressStartTime = Time.time;
            jumpReleased = false;

            while (true)
            {
                if (suspendInput) break;

                if (jumpReleased)
                {
                    execJump = true;
                    break;
                }
                else if (Time.time - jumpPressStartTime > powerJumpThreshold)
                {
                    execPowerJump = true;
                    break;
                }

                yield return null;
            }
        }

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            if (suspendInput) return;

            StartCoroutine(Co_MonitorJumpIntent());
        }

        private void OnJumpReleaseIntent(InputAction.CallbackContext context)
        {
            if (suspendInput) return;

            jumpReleased = true;
        }

        private void OnDashIntent(InputAction.CallbackContext context)
        {
            if (suspendInput) return;

            if (state == PlayerState.Running)
            {
                suspendInput = true;
                execDash = true;
            }
        }

        private void ScanRawIntents()
        {
            rawValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveXValue = rawValue.x;
            moveYValue = rawValue.y;

            if (Mathf.Abs(rawValue.x) > Mathf.Epsilon)
            {
                OnMoveXIntent();
            }
            else

            {
                rigidBody.linearVelocityX = 0f;
            }
        }

        private void OnMoveXIntent()
        {
            if (suspendInput) return;

            if (isGrounded)
            {
                execRun = true;
            }
            else
            {
                execDodge = true;
            }
        }

        private void UpdateUI()
        {
            if (isGroundedUI != null)
            {
                isGroundedUI.Value = isGrounded ? "True" : "False";
            }

            velocityX = rigidBody.linearVelocityX;
            velocityY = rigidBody.linearVelocityY;

            if (velocityXUI != null)
            {
                velocityXUI.Value = velocityX.ToString("0.00");
                velocityYUI.Value = velocityY.ToString("0.00");
            }

            if (stateUI != null)
            {
                stateUI.Value = state.ToString();
            }
            
            minEnergyThreshold.gameObject.SetActive(scene.EnergyBarUI.Value >= POWER_MOVE_MIN_ENERGY_VALUE);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateUI();

            if (monitorDash)
            {
                monitorDash = false;
                StartCoroutine(Co_Dash());
            }
        }

        private void ApplyRun()
        {
            relativeMoveSpeed = rawValue * moveSpeed;
            rigidBody.linearVelocityX = relativeMoveSpeed.x;
            execRun = false;
        }

        private void ApplyJump()
        {
            rigidBody.AddForce(Vector2.up * jumpForce);
            audioSource.PlayOneShot(jumpClip, 1f);
            execJump = false;
        }

        private void ApplyPowerJump()
        {
            rigidBody.AddForce(Vector2.up * powerJumpForce);

            if (scene.EnergyBarUI != null)
            {
                scene.EnergyBarUI.Value -= POWER_MOVE_MIN_ENERGY_VALUE;
            }

            audioSource.PlayOneShot(jumpClip, 1f);
            execPowerJump = false;
        }

        private void ApplyDodge()
        {
            relativeDodgeSpeed = rawValue * moveSpeed;
            var foo = rigidBody.linearVelocityX + relativeDodgeSpeed.x;
            rigidBody.linearVelocityX = Mathf.Clamp(foo, -Mathf.Abs(moveSpeed), Mathf.Abs(moveSpeed));
            execDodge = false;
        }

        private IEnumerator Co_Dash()
        {
            var elapsedTime = 0f;

            while (elapsedTime < dashDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isDashing = false;
            suspendInput = false;
        }

        private void ApplyDash()
        {
            if (scene.EnergyBarUI.Value > POWER_MOVE_MIN_ENERGY_VALUE)
            {
                var direction = Mathf.Sign(rigidBody.linearVelocityX);
                rigidBody.linearVelocityX = direction * dashSpeed;

                if (scene.EnergyBarUI != null)
                {
                    scene.EnergyBarUI.Value -= POWER_MOVE_MIN_ENERGY_VALUE;
                }

                audioSource.PlayOneShot(dashClip, 1f);
            }

            execDash = false;
            monitorDash = true;
            isDashing = true;
        }

        public void OnGround(Collider2D collider) => audioSource.PlayOneShot(landClip, 1f);

        public void OffGround(Collider2D collider) { }

        private void AscertState()
        {
            state = PlayerState.Idle;

            if (isGrounded)
            {
                if (Mathf.Abs(rigidBody.linearVelocity.x) > MIN_INPUT_VALUE)
                {
                    if (isDashing)
                    {
                        state = PlayerState.Dashing;
                    }
                    else if (isGrounded)
                    {
                        state = PlayerState.Running;
                    }
                }
            }
            else
            {
                if (rigidBody.linearVelocity.y >= MIN_INPUT_VALUE)
                {
                    state = PlayerState.Jumping;
                }
                else if (rigidBody.linearVelocity.y <= -MIN_INPUT_VALUE)
                {
                    state = PlayerState.Falling;
                }
            }
        }

        private void UpdateIsGrounded() => isGrounded = Physics2D.RaycastAll(transform.position, -transform.up, transform.localScale.y * (0.5f + 0.1f), layerMask).Length > 0;

        void FixedUpdate()
        {
            AscertState();
            UpdateIsGrounded();
            ScanRawIntents();

            if (execRun)
            {
                ApplyRun();
            }

            if (execJump)
            {
                ApplyJump();
            }

            if (execPowerJump)
            {
                ApplyPowerJump();
            }

            if (execDodge)
            {
                ApplyDodge();
            }

            if (execDash)
            {
                ApplyDash();
            }
        }
    }
}