using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

using UI;

namespace Tests
{
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour
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
        [Range(10f, 20f)]
        [SerializeField] float moveSpeed = 15f;

        [Header("Dodge")]
        [Range(0f, 5f)]
        [SerializeField] float dodgeSpeed = 2.5f;
        // [Range(100f, 400f)]
        // [SerializeField] float dodgeForce = 200f;
        // [SerializeField] float minDodgeThreshold = 0.1f;
        // [Range(0f, 1f)]
        // [SerializeField] float dodgeSpeedModifier = 0.5f;
        // [Range(5f, 15f)]
        // [SerializeField] float minDodgeSpeed = 7.5f;

        [Header("Jump")]
        [Range(200f, 1000f)]
        [SerializeField] float jumpForce = 800f;
        [Range(600f, 1400f)]
        [SerializeField] float powerJumpForce = 1200f;

        [Header("Dash")]
        [Range(10f, 100f)]
        [SerializeField] float dashSpeed = 50f;
        [SerializeField] float dashDuration = 0.05f;

        [Header("Move Stats")]
        [SerializeField] Vector2 rawValue;
        [SerializeField] float moveXValue;
        // [SerializeField] float trueMoveXValue;
        [SerializeField] float moveYValue;
        // [SerializeField] float trueMoveYValue;
        [SerializeField] Vector2 relativeMoveSpeed;
        [SerializeField] Vector2 relativeDodgeSpeed;

        [Header("Rigidbody")]
        [SerializeField] float velocityX;
        [SerializeField] float velocityY;

        [Header("Inferences")]
        [SerializeField] bool isGrounded;

        public static float POWER_MOVE_MIN_ENERGY_VALUE = 0.5f;
        public static float MIN_INPUT_VALUE = 0.1f;
        // public static float MIN_Y_INPUT_VALUE = 0.25f;

        private InputActionMapping scene;
        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        // private new Collider2D collider;
        private AudioSource audioSource;
        private PlayerState state;
        private bool execRun, execJump, execDodge/*, execPowerJump*/, execDash;
        private float jumpPressStartTime, jumpHeldDuration;
        private float jumpXVelocity;
        private bool suspendInput, monitorDash, isDashing;
        private float linearDamping;
        private int layerMask;
        // private bool hasHit;
        // private RaycastHit hit;

        void Awake()
        {
            scene = FindAnyObjectByType<InputActionMapping>();
            rigidBody = GetComponent<Rigidbody2D>();
            // collider = GetComponent<Collider2D>();
            audioSource = GetComponent<AudioSource>();
            inputActions = new InputSystem_Actions();
            linearDamping = rigidBody.linearDamping;
            layerMask = LayerMask.GetMask("Player");
        }

        void OnEnable()
        {
            inputActions.Enable();

            // inputActions.Player.Move.performed += OnMoveIntent;
            // inputActions.Player.Jump.performed += OnJumpIntent;
            // inputActions.Player.PowerJump.performed += OnPowerJumpIntent;
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

        // private void OnMoveIntent(InputAction.CallbackContext context)
        // {
        //     Debug.Log("OnMoveIntent");

        //     var rawValue = inputActions.Player.Move.ReadValue<Vector2>();
        //     moveXValue = rawValue.x;
        //     moveYValue = rawValue.y;

        //     var value = rawValue * Time.deltaTime * moveSpeed;
        //     trueMoveXValue = value.x;
        //     trueMoveYValue = value.y;

        //     if (isGrounded && Mathf.Abs(trueMoveXValue) > MIN_FLOAT_VALUE)
        //     {
        //         execRun = true;
        //     }
        // }

        // private void OnJumpIntent(InputAction.CallbackContext context)
        // {
        //     Debug.Log("OnJumpIntent");

        //     if (isGrounded)
        //     {
        //         exeJump = true;
        //     }
        // }

        // private void OnPowerJumpIntent(InputAction.CallbackContext context)
        // {
        //     Debug.Log("OnPowerJumpIntent");

        //     if (isGrounded)
        //     {
        //         execPowerJump = true;
        //     }
        // }

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            // Debug.Log($"OnJumpPressIntent State: {state}");
            
            if (suspendInput) return;

            jumpPressStartTime = Time.time;
        }

        private void OnJumpReleaseIntent(InputAction.CallbackContext context)
        {
            // Debug.Log($"OnJumpReleaseIntent State: {state}");
            
            if (suspendInput) return;

            jumpHeldDuration = Time.time - jumpPressStartTime;
            jumpXVelocity = rigidBody.linearVelocity.x;
            execJump = true;
        }

        private void OnDashIntent(InputAction.CallbackContext context)
        {
            // Debug.Log($"OnDashIntent State: {state}");
            
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

            // Debug.Log($"Move: [{moveXValue}, {moveYValue}]");

            // var value = rawValue * Time.fixedDeltaTime * moveSpeed;
            // trueMoveXValue = value.x;
            // trueMoveYValue = value.y;

            // if (Mathf.Abs(trueMoveXValue) > MIN_FLOAT_VALUE)
            // {
            //     OnMoveXIntent();
            // }

            if (Mathf.Abs(rawValue.x) > Mathf.Epsilon)
            {
                OnMoveXIntent();
            }
        }

        private void OnMoveXIntent()
        {
            // Debug.Log($"OnMoveXIntent State: {state}");

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

#if false
        private void OnRunIntent()
        {
            // Debug.Log($"OnRunIntent State: {state}");

            if (suspendInput) return;

            if (/*isGrounded && */Mathf.Abs(trueMoveXValue) > MIN_FLOAT_VALUE)
            {
                if (isGrounded)
                {
                    execRun = true;
                }
                else
                {
                    execDodge = true;
                }
            }

            // if (Mathf.Abs(rigidBody.linearVelocity.x) > minDodgeThreshold)
        }
#endif
        private void UpdateUI()
        {
            isGroundedUI.Value = isGrounded ? "True" : "False";

            velocityX = rigidBody.linearVelocityX;
            velocityY = rigidBody.linearVelocityY;

            velocityXUI.Value = velocityX.ToString("0.00");
            velocityYUI.Value = velocityY.ToString("0.00");
            stateUI.Value = state.ToString();

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
            // Debug.Log("ApplyRun");

            relativeMoveSpeed = rawValue * moveSpeed;
            // Debug.Log($"ApplyRun: [{relativeMoveSpeed.x}, {relativeMoveSpeed.y}]");

            rigidBody.linearVelocityX = /*trueMoveXValue*/relativeMoveSpeed.x;
            execRun = false;
        }

        // private void ApplyJump()
        // {
        //     rigidBody.AddForce(Vector2.up * minJumpSpeed);
        //     execJump = false;
        // }

        // private void ApplyPowerJump()
        // {
        //     rigidBody.AddForce(Vector2.up * maxJumpSpeed);
        //     execPowerJump = false;
        // }

        private void ApplyJump(float heldDuration)
        {
            // Debug.Log($"ApplyJump Held Duration: {heldDuration}");

            if (scene.EnergyBarUI.Value > POWER_MOVE_MIN_ENERGY_VALUE && heldDuration > 0.2f)
            {
                rigidBody.AddForce(Vector2.up * powerJumpForce);
                scene.EnergyBarUI.Value -= POWER_MOVE_MIN_ENERGY_VALUE;
            }
            else
            {
                rigidBody.AddForce(Vector2.up * jumpForce);
            }

            audioSource.PlayOneShot(jumpClip, 1f);
            // rigidBody.linearDamping = 0f;
            execJump = false;
        }

        private float InverseClamp(float value, float minValue, float maxValue)
        {
            if (value > minValue && value < maxValue)
            {
                var mid = (maxValue - minValue) / 2 + minValue;
                return value < mid ? minValue : maxValue;
            }

            return value;
        }

        private void ApplyDodge()
        {
#if false
            Debug.Log($"ApplyDodge JumpXVelocity: {jumpXVelocity}");

            rigidBody.linearVelocityX = rigidBody.linearVelocityX + /*trueMoveXValue*/relativeMoveSpeed.x * dodgeSpeedModifier;

            Debug.Log($"ApplyDodge PRE LinearVelocityX: {rigidBody.linearVelocityX}");

            rigidBody.linearVelocityX = Mathf.Clamp(rigidBody.linearVelocityX, -jumpXVelocity, jumpXVelocity);

            Debug.Log($"ApplyDodge POST LinearVelocityX: {rigidBody.linearVelocityX}");
            
            // rigidBody.AddForce(Vector2.right * trueMoveXValue * dodgeForce);
#endif

            relativeDodgeSpeed = rawValue * dodgeSpeed;
            // Debug.Log($"ApplyDodge: [{relativeDodgeSpeed.x}, {relativeDodgeSpeed.y}]");

            // var foo = rigidBody.linearVelocityX + relativeMoveSpeed.x * dodgeSpeedModifier;
            var foo = rigidBody.linearVelocityX + relativeDodgeSpeed.x;
            // foo = InverseClamp(foo, -minDodgeSpeed, minDodgeSpeed);
            // rigidBody.linearVelocityX = Mathf.Clamp(foo, -Mathf.Abs(jumpXVelocity), Mathf.Abs(jumpXVelocity));
            // foo = InverseClamp(foo, -dodgeSpeed, dodgeSpeed);
            rigidBody.linearVelocityX = Mathf.Clamp(foo, -Mathf.Abs(dodgeSpeed), Mathf.Abs(dodgeSpeed));

            execDodge = false;
        }

        private IEnumerator Co_Dash()
        {
            // Debug.Log($"Co_Dash VelocityX: {rigidBody.linearVelocityX}");

            var elapsedTime = 0f;

            while (elapsedTime < dashDuration)
            {
                // Debug.Log($"Co_Dash VelocityX: {rigidBody.linearVelocityX}");
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isDashing = false;
            suspendInput = false;
        }

        private void ApplyDash()
        {
            // Debug.Log("ApplyDash");

            if (scene.EnergyBarUI.Value > POWER_MOVE_MIN_ENERGY_VALUE)
            {
                var direction = Mathf.Sign(rigidBody.linearVelocityX);
                rigidBody.linearVelocityX = direction * dashSpeed;
                scene.EnergyBarUI.Value -= POWER_MOVE_MIN_ENERGY_VALUE;
                audioSource.PlayOneShot(dashClip, 1f);
            }

            execDash = false;
            monitorDash = true;
            isDashing = true;
        }

        public void OnGround(Collider2D collider)
        {
            // Debug.Log("OnGround");

            audioSource.PlayOneShot(landClip, 1f);
            rigidBody.linearDamping = linearDamping;
            // isGrounded = true;
        }

        public void OffGround(Collider2D collider)
        {
            // Debug.Log("OffGround");
            
            // audioSource.PlayOneShot(jumpClip, 1f);
            rigidBody.linearDamping = 0f;
            // isGrounded = false;
        }

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
                // else
                // {
                //     state = PlayerState.InAir;
                // }
            }

            // Debug.Log($"State: {state}");
        }

        private void UpdateIsGrounded() => isGrounded = Physics2D.RaycastAll(transform.position, -transform.up, transform.localScale.y * (0.5f + 0.1f), layerMask).Length > 0;

        void FixedUpdate()
        {
            AscertState();

            // hasHit = Physics.BoxCast(collider.bounds.center, transform.localScale * 0.5f, -transform.up, out hit, transform.rotation, 0.5f);
            
            // var hits = Physics2D.RaycastAll(transform.position, -transform.up, transform.localScale.y * (0.5f + 0.1f), layerMask);

            // foreach (var hit in hits)
            // {
            //     if (!hit.collider.gameObject.tag.Equals("Player"))
            //     {
            //         Debug.Log($"Hit: {hit.collider.gameObject.name}");
            //     }
            // }

            UpdateIsGrounded();
            ScanRawIntents();

            if (execRun)
            {
                ApplyRun();
            }

            // if (execJump)
            // {
            //     ApplyJump();
            // }

            // if (execPowerJump)
            // {
            //     ApplyPowerJump();
            // }

            if (execJump)
            {
                ApplyJump(jumpHeldDuration);
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

        // void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.yellow;

        //     if (collider != null)
        //     {
        //         Gizmos.DrawRay(transform.position, -transform.up * transform.localScale.y * (0.5f + 0.1f));
        //     }

        //         Gizmos.color = Color.red;

        //     if (hasHit)
        //     {
        //         Gizmos.DrawRay(transform.position, -transform.up * hit.distance);
        //         Gizmos.DrawWireCube(transform.position + -transform.up * hit.distance, transform.localScale);
        //     }
        //     else
        //     {
        //         Gizmos.DrawRay(transform.position, -transform.up * 0.5f);
        //         Gizmos.DrawWireCube(transform.position + -transform.up * 0.5f, transform.localScale);
        //     }
        // }
    }
}