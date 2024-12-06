using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

using UI;

namespace Tests
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Analytics))]
    public class PlayerV2 : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] OnTrigger2DHandler topEdgeTrigger;
        [SerializeField] OnTrigger2DHandler rightEdgeTrigger;
        [SerializeField] OnTrigger2DHandler bottomEdgeTrigger;
        [SerializeField] OnTrigger2DHandler leftEdgeTrigger;

        [Header("Settings")]
        [SerializeField] Transform minEnergyThreshold;
        // [SerializeField] DamageThresholds damageThresholds;

        [Header("Audio")]
        [SerializeField] AudioClip jumpClip;
        [SerializeField] AudioClip landClip;
        [SerializeField] AudioClip dashClip;

        [Header("UI")]
        [SerializeField] AttributeUI thumbstickXUI;
        [SerializeField] AttributeUI thumbstickYUI;
        [SerializeField] AttributeUI updatesUI;
        [SerializeField] AttributeUI fixedUpdatesUI;
        [SerializeField] AttributeUI isBlockedAboveUI;
        [SerializeField] AttributeUI isBlockedRightUI;
        [SerializeField] AttributeUI isGroundedUI;
        [SerializeField] AttributeUI isBlockedLeftUI;
        [SerializeField] AttributeUI isGrippingUI;
        [SerializeField] AttributeUI velocityXUI;
        [SerializeField] AttributeUI velocityYUI;
        [SerializeField] AttributeUI stateUI;

        [Header("Player UI")]
        [SerializeField] Transform arrowBaseUI;

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

        [Header("Stats")]
        [SerializeField] Vector2 moveValue;
        [SerializeField] float gripValue;
        [SerializeField] float moveXValue;
        [SerializeField] float moveYValue;
        [SerializeField] Vector2 relativeMoveSpeed;
        [SerializeField] Vector2 relativeDodgeSpeed;

        [Header("Rigidbody")]
        [SerializeField] float velocityX;
        [SerializeField] float velocityY;

        [Header("Features")]
        [SerializeField] bool wallsAreSlippy;

        [Header("Inferences")]
        [SerializeField] bool isBlockedAbove;
        [SerializeField] bool isBlockedRight;
        [SerializeField] bool isGrounded;
        [SerializeField] bool isBlockedLeft;
        [SerializeField] bool isDashing;
        [SerializeField] bool isGripping;
#if false
        [Header("Sandbox")]
        [SerializeField] LineRenderer circleRenderer;
#endif
        public static float POWER_MOVE_MIN_ENERGY_VALUE = 0.5f;
        public static float MIN_DAMAGE_VELOCITY = -14;
        public static float DAMAGE_PER_POINT = 0.1f;
        public static float MIN_INPUT_VALUE = 0.1f;
        public static int STEPPED_ANGLE_DEGREES = 15;
        public static Color ORANGE;

        private SceneObjectMapping scene;
        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        private new Collider2D collider;
        private AudioSource audioSource;
        private Analytics analytics;
        private PlayerState state;
        private bool execRun, execJump, execPowerJump, execDodge, execDash;
        private float jumpPressStartTime;
        private bool suspendInput, monitorDash, jumpReleased;
        private float lastLinearVelocityY;
        private int layerMask;

        void Awake()
        {
            scene = FindAnyObjectByType<SceneObjectMapping>();
            rigidBody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            audioSource = GetComponent<AudioSource>();
            analytics = GetComponent<Analytics>();
            inputActions = new InputSystem_Actions();
            layerMask = LayerMask.GetMask("Player");

            ColorUtility.TryParseHtmlString("#FF6100", out Color orange);
            ORANGE = orange;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // void Start() => RenderCircle(100, 0.1f);

        void OnEnable()
        {
            inputActions.Enable();

            inputActions.Player.JumpPress.performed += OnJumpPressIntent;
            inputActions.Player.JumpRelease.performed += OnJumpReleaseIntent;
            inputActions.Player.Dash.performed += OnDashIntent;
            inputActions.Game.F1.performed += OnF1Intent;
            inputActions.Game.F2.performed += OnF2Intent;
            inputActions.Game.F3.performed += OnF3Intent;

            topEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            rightEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            bottomEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            leftEdgeTrigger.Register(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
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
                else if (isGrounded && Time.time - jumpPressStartTime > powerJumpThreshold)
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

            if (isGrounded || isBlockedLeft || isBlockedRight)
            {
                StartCoroutine(Co_MonitorJumpIntent());
            }
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

        private void OnF1Intent(InputAction.CallbackContext context)
        {
            if (scene.Attributes == null) return;
            scene.Attributes.gameObject.SetActive(!scene.Attributes.gameObject.activeSelf);
        }

        private void OnF2Intent(InputAction.CallbackContext context)
        {
            if (scene.Actuators == null) return;
            scene.Actuators.gameObject.SetActive(!scene.Actuators.gameObject.activeSelf);
        }

        private void OnF3Intent(InputAction.CallbackContext context)
        {
            if (scene.Analytics == null) return;
            scene.Analytics.gameObject.SetActive(!scene.Analytics.gameObject.activeSelf);
        }

        private void ScanRawIntents()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveXValue = moveValue.x;
            moveYValue = moveValue.y;

            if (Mathf.Abs(moveValue.x) > Mathf.Epsilon)
            {
                OnMoveXIntent();
            }
            else
            {
                rigidBody.linearVelocityX = 0f;
            }

            isGripping = inputActions.Player.Grip.IsPressed();
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

                var direction = Mathf.Sign(moveValue.x);

                if (wallsAreSlippy && (direction > 0f && isBlockedRight || direction < 0f && isBlockedLeft))
                {
                    rigidBody.linearVelocityX = 0f;
                    return;
                }

                execDodge = true;
            }
        }

        private float Vector2ToAngle(Vector2 value)
        {
            var radians = Mathf.Atan2(value.y, value.x);
            return radians * (180f / Mathf.PI) - 90f;
        }

        private float AngleToSteppedAngle(float angle, int steps) => (int) angle / steps * steps; 

        private void UpdatePlayerUI()
        {
            if (arrowBaseUI != null)
            {
                var angle = Vector2ToAngle(new Vector2(moveXValue, moveYValue));
                // Debug.Log($"Angle: {angle}");

                var steppedAngle = AngleToSteppedAngle(angle, STEPPED_ANGLE_DEGREES);
                // Debug.Log($"Stepped Angle: {steppedAngle}");

                arrowBaseUI.eulerAngles = new Vector3(0f, 0f, steppedAngle);
            }

            arrowBaseUI.gameObject.SetActive(moveValue != Vector2.zero);
        }

        private void UpdateUI()
        {
            if (thumbstickXUI != null)
            {
                thumbstickXUI.Value = moveXValue.ToString("0.00");
            }

            if (thumbstickYUI != null)
            {
                thumbstickYUI.Value = moveYValue.ToString("0.00");
            }

            if (updatesUI != null)
            {
                updatesUI.Value = analytics.UpdatesPerSecond.ToString();
            }

            if (fixedUpdatesUI != null)
            {
                fixedUpdatesUI.Value = analytics.FixedUpdatesPerSecond.ToString();
            }

            if (isGrippingUI != null)
            {
                isGrippingUI.Value = isGripping ? "True" : "False";    
                isGrippingUI.Color = isGripping ? Color.green : ORANGE;
            }

            if (isBlockedAboveUI != null)
            {
                isBlockedAboveUI.Value = isBlockedAbove ? "True" : "False";
                isBlockedAboveUI.Color = isBlockedAbove ? Color.green : ORANGE;
            }

            if (isBlockedRightUI != null)
            {
                isBlockedRightUI.Value = isBlockedRight ? "True" : "False";
                isBlockedRightUI.Color = isBlockedRight ? Color.green : ORANGE;
            }

            if (isGroundedUI != null)
            {
                isGroundedUI.Value = isGrounded ? "True" : "False";
                isGroundedUI.Color = isGrounded ? Color.green : ORANGE;
            }

            if (isBlockedLeftUI != null)
            {
                isBlockedLeftUI.Value = isBlockedLeft ? "True" : "False";
                isBlockedLeftUI.Color = isBlockedLeft ? Color.green : ORANGE;
            }

            velocityX = rigidBody.linearVelocityX;
            velocityY = rigidBody.linearVelocityY;

            if (velocityXUI != null)
            {
                velocityXUI.Value = velocityX.ToString("0.00");
            }

            if (velocityYUI != null)
            {
                velocityYUI.Value = velocityY.ToString("0.00");
            }

            if (stateUI != null)
            {
                stateUI.Value = state.ToString();
            }
            
            minEnergyThreshold.gameObject.SetActive(scene.EnergyBar.Value >= POWER_MOVE_MIN_ENERGY_VALUE);

            UpdatePlayerUI();
        }

#if false
        private void RenderCircle(int steps, float radius, float width, Color color)
        {
            circleRenderer.positionCount = steps + 1;
            circleRenderer.startWidth = circleRenderer.endWidth = width;
            circleRenderer.startColor = circleRenderer.endColor = color;

            for (int itr = 0; itr <= steps; itr++)
            {
                var progress = itr < steps ? (float) itr / steps : 0f;
                var radians = progress * 2 * Mathf.PI;

                var xScaled = Mathf.Cos(radians);
                var x = xScaled * radius;

                var yScaled = Mathf.Sin(radians);
                var y = yScaled * radius;

                var position = new Vector3(transform.position.x + x, transform.position.y + y, 0f);
                circleRenderer.SetPosition(itr, position);
            }
        }
#endif
        // Update is called once per frame
        void Update()
        {
            UpdateUI();

            if (monitorDash)
            {
                monitorDash = false;
                StartCoroutine(Co_Dash());
            }

            // RenderCircle(25, 0.55f, 0.015f, Color.grey);
        }

        private void ApplyRun()
        {
            relativeMoveSpeed = moveValue * moveSpeed;
            rigidBody.linearVelocityX = relativeMoveSpeed.x;
            execRun = false;
        }

        private Vector2 RadianToVector2(float radian) => new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
        
        private Vector2 DegreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);

        private void ApplyJump()
        {
            var bearing = DegreeToVector2(arrowBaseUI.eulerAngles.z);
            // Debug.Log($"Bearing: {bearing}");

            rigidBody.AddForce(/*Vector2.up*/bearing * jumpForce);
            audioSource.PlayOneShot(jumpClip, 1f);
            execJump = false;
        }

        private void DrainEnergyLevel(float value)
        {
            if (scene.EnergyBar == null) return;
            scene.EnergyBar.Value -= value;
        }

        private void ApplyPowerJump()
        {
            var bearing = DegreeToVector2(arrowBaseUI.eulerAngles.z);
            // Debug.Log($"Bearing: {bearing}");

            rigidBody.AddForce(/*Vector2.up*/bearing * powerJumpForce);
            DrainEnergyLevel(POWER_MOVE_MIN_ENERGY_VALUE);
            audioSource.PlayOneShot(jumpClip, 1f);
            execPowerJump = false;
        }

        private void ApplyDodge()
        {
            relativeDodgeSpeed = moveValue * moveSpeed;
            var velocityX = rigidBody.linearVelocityX + relativeDodgeSpeed.x;
            rigidBody.linearVelocityX = Mathf.Clamp(velocityX, -Mathf.Abs(moveSpeed), Mathf.Abs(moveSpeed));
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
            if (scene.EnergyBar.Value > POWER_MOVE_MIN_ENERGY_VALUE)
            {
                var direction = Mathf.Sign(rigidBody.linearVelocityX);
                rigidBody.linearVelocityX = direction * dashSpeed;
                DrainEnergyLevel(POWER_MOVE_MIN_ENERGY_VALUE);
                audioSource.PlayOneShot(dashClip, 1f);
            }

            execDash = false;
            monitorDash = true;
            isDashing = true;
        }

        public void OnGainedContactWithEdge(OnTrigger2DHandler instance, Collider2D collider)
        {
            if (instance == topEdgeTrigger)
            {
                isBlockedAbove = true;
            }
            else if (instance == rightEdgeTrigger)
            {
                isBlockedRight = true;
            }
            else if (instance == bottomEdgeTrigger)
            {
                isGrounded = true;
                audioSource.PlayOneShot(landClip, 1f);

                var damagePoints = MIN_DAMAGE_VELOCITY - lastLinearVelocityY;

                if (damagePoints > 0f)
                {
                    var damage = Mathf.Clamp(DAMAGE_PER_POINT * damagePoints, 0f, 1f);
                    scene.HealthBar.Value -= damage;
                }
            }
            else if (instance == leftEdgeTrigger)
            {
                isBlockedLeft = true;
            }
        }

        public void OnLostContactWithEdge(OnTrigger2DHandler instance, Collider2D collider)
        {
            if (instance == topEdgeTrigger)
            {
                isBlockedAbove = false;
            }
            else if (instance == rightEdgeTrigger)
            {
                isBlockedRight = false;
            }
            else if (instance == bottomEdgeTrigger)
            {
                isGrounded = false;
            }
            else if (instance == leftEdgeTrigger)
            {
                isBlockedLeft = false;
            }
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
            }
        }

        // private void UpdateIsGrounded() => isGrounded = Physics2D.RaycastAll(transform.position, Vector2.down, transform.localScale.y * (0.5f + 0.1f), layerMask).Length > 0;

        // private void UpdateIsBlockedLeft() => isBlockedLeft = Physics2D.RaycastAll(transform.position, Vector2.left, transform.localScale.y * (0.5f + 0.1f), layerMask).Length > 0;

        // private void UpdateIsBlockedRight() => isBlockedRight = Physics2D.RaycastAll(transform.position, Vector2.right, transform.localScale.y * (0.5f + 0.1f), layerMask).Length > 0;

        // private void UpdateCollisionStatus()
        // {
        //     UpdateIsGrounded();
        //     UpdateIsBlockedLeft();
        //     UpdateIsBlockedRight();
        // }

        void FixedUpdate()
        {
            AscertState();
            // UpdateCollisionStatus();
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

            lastLinearVelocityY = rigidBody.linearVelocityY;
        }
    }
}