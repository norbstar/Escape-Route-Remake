using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

using UI;
using System.Linq;

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
        [Range(10f, 50f)]
        [SerializeField] float moveSpeed = 25f;

        [Header("Jump")]
        [Range(200f, 400f)]
        [SerializeField] float minJumpSpeed = 300f;
        [Range(400f, 600f)]
        [SerializeField] float midJumpSpeed = 500f;
        [Range(600f, 800f)]
        [SerializeField] float maxJumpSpeed = 700f;

        [Header("Dash")]
        [Range(10f, 100f)]
        [SerializeField] float dashSpeed = 50f;
        [SerializeField] float dashDuration = 0.05f;

        [Header("Move Stats")]
        [SerializeField] float moveXValue;
        [SerializeField] float trueMoveXValue;
        [SerializeField] float moveYValue;
        [SerializeField] float trueMoveYValue;

        [Header("Rigidbody")]
        [SerializeField] float velocityX;
        [SerializeField] float velocityY;

        [Header("Inferences")]
        [SerializeField] bool isGrounded;

        public static float POWER_MOVE_MIN_ENERGY_VALUE = 0.5f;
        public static float MIN_FLOAT_VALUE = 0.01f;

        private InputActionMapping scene;
        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        // private new Collider2D collider;
        private AudioSource audioSource;
        private PlayerState state;
        private bool execRun, execJump/*, execPowerJump*/, execDash;
        private float jumpPressStartTime, jumpHeldDuration;
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
            var rawValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveXValue = rawValue.x;
            moveYValue = rawValue.y;

            var value = rawValue * Time.deltaTime * moveSpeed;
            trueMoveXValue = value.x;
            trueMoveYValue = value.y;

            OnRunIntent();
        }

        private void OnRunIntent()
        {
            // Debug.Log($"OnRunIntent State: {state}");

            if (suspendInput) return;

            if (isGrounded && Mathf.Abs(trueMoveXValue) > MIN_FLOAT_VALUE)
            {
                execRun = true;
            }
        }

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

            rigidBody.linearVelocityX = trueMoveXValue * moveSpeed;
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

            if (heldDuration > 0.4f)
            {
                rigidBody.AddForce(Vector2.up * maxJumpSpeed);
            }
            else if (heldDuration > 0.2f)
            {
                rigidBody.AddForce(Vector2.up * midJumpSpeed);
            }
            else
            {
                rigidBody.AddForce(Vector2.up * minJumpSpeed);
            }

            rigidBody.linearDamping = 0f;
            execJump = false;
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
            
            audioSource.PlayOneShot(jumpClip, 1f);
            // isGrounded = false;
        }

        private void AscertState()
        {
            state = PlayerState.Idle;

            if (Mathf.Abs(rigidBody.linearVelocity.x) > MIN_FLOAT_VALUE)
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

            if (rigidBody.linearVelocity.y > MIN_FLOAT_VALUE)
            {
                state = PlayerState.Jumping;
            }
            else if (rigidBody.linearVelocity.y < -MIN_FLOAT_VALUE)
            {
                state = PlayerState.Falling;
            }
        }

        private void UpdateIsGrounded() => isGrounded = Physics2D.RaycastAll(transform.position, -transform.up, transform.localScale.y * (0.5f + 0.1f), layerMask).Count() > 0;

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