using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

using UI;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteShapeRenderer))]
    [RequireComponent(typeof(SpriteShapeController))]
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerV3 : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] OnTrigger2DHandler topEdgeTrigger;
        [SerializeField] OnTrigger2DHandler rightEdgeTrigger;
        [SerializeField] OnTrigger2DHandler bottomEdgeTrigger;
        [SerializeField] OnTrigger2DHandler leftEdgeTrigger;

        [Header("Settings")]
        [SerializeField] UnityEngine.Transform minEnergyThreshold;

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
        [SerializeField] AttributeUI isHoldingUI;
        [SerializeField] AttributeUI angleUI;
        [SerializeField] AttributeUI bearingUI;
        [SerializeField] AttributeUI velocityXUI;
        [SerializeField] AttributeUI velocityYUI;
        [SerializeField] AttributeUI stateUI;

        [Header("Player UI")]
        [SerializeField] UnityEngine.Transform arrowBaseUI;

        [Header("Move")]
        [Range(1f, 10f)]
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] AnimationCurve sensitivityProfile;

        [Header("Dodge")]
        [SerializeField] bool canDodge;
        [Range(1f, 10f)]
        [SerializeField] float dodgeSpeed = 5f;

        [Header("Jump")]
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] bool vectorBasedJump;
        [SerializeField] bool canPowerJump;
        [Range(600f, 1000f)]
        [SerializeField] float powerJumpForce = 800f;
        [SerializeField] float powerJumpThreshold = 0.2f;

        [Header("Dash")]
        [SerializeField] bool canDash;
        [Range(10f, 100f)]
        [SerializeField] float dashSpeed = 50f;
        [SerializeField] float dashDuration = 0.05f;

        [Header("Stats")]
        [SerializeField] Vector2 moveValue;
        [SerializeField] float moveXValue;
        [SerializeField] float moveYValue;
        [SerializeField] float relativeMoveSpeed;

        [Header("Rigidbody")]
        [SerializeField] float velocityX;
        [SerializeField] float velocityY;

        [Header("Features")]
        [SerializeField] bool wallsAreSlippy;
        [SerializeField] float deformationSpeed = 2.5f;

        [Header("Inferences")]
        [SerializeField] bool isBlockedAbove;
        [SerializeField] bool isBlockedRight;
        [SerializeField] bool isGrounded;
        [SerializeField] bool isBlockedLeft;
        [SerializeField] bool isDashing;
        [SerializeField] bool isHolding;

        public static float POWER_MOVE_MIN_ENERGY_VALUE = 0.5f;
        public static float MIN_DAMAGE_VELOCITY = -14;
        public static float DAMAGE_PER_POINT = 0.1f;
        public static float SQUISH_PER_POINT = 0.1f;
        public static float MIN_INPUT_VALUE = 0.1f;
        // public static int STEPPED_ANGLE_DEGREES = 15;
        public static Color ORANGE;

        private CanvasManager canvasManager;
        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        // private new Collider2D collider;
        private SpriteShapeController spriteShapeController;
        private AudioSource audioSource;
        private AnalyticsUI analytics;
        private PlayerStateEnum state;
        private bool execRun, execJump, execPowerJump, execDodge, execDash;
        private Vector2 bearing;
        private float jumpPressStartTime;
        private bool suspendInput, monitorDash, jumpReleased;
        private float lastLinearVelocityY;
        private int pointCount;
        private int layerMask;

        void Awake()
        {
            canvasManager = FindAnyObjectByType<CanvasManager>();
            rigidBody = GetComponent<Rigidbody2D>();
            // collider = GetComponent<Collider2D>();
            spriteShapeController = GetComponent<SpriteShapeController>();
            audioSource = GetComponent<AudioSource>();
            analytics = FindFirstObjectByType<AnalyticsUI>();
            inputActions = new InputSystem_Actions();
            layerMask = LayerMask.GetMask("Player");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ColorUtility.TryParseHtmlString("#FF6100", out Color orange);
            ORANGE = orange;

            pointCount = spriteShapeController.spline.GetPointCount();
            // Debug.Log($"Spline Point Count: {pointCount}");
        }

        void OnEnable()
        {
            inputActions.Enable();

            inputActions.Player.JumpPress.performed += OnJumpPressIntent;
            inputActions.Player.JumpRelease.performed += OnJumpReleaseIntent;
            inputActions.Player.Dash.performed += OnDashIntent;
            inputActions.Game.F1.performed += OnF1Intent;
            inputActions.Game.F2.performed += OnF2Intent;
            inputActions.Game.F3.performed += OnF3Intent;

            topEdgeTrigger.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            rightEdgeTrigger.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            bottomEdgeTrigger.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);

            leftEdgeTrigger.Subscribe(new OnTrigger2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);
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
                else if (canPowerJump && isGrounded && Time.time - jumpPressStartTime > powerJumpThreshold)
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

            if (canDash && state == PlayerStateEnum.Running)
            {
                suspendInput = true;
                execDash = true;
            }
        }

        private void OnF1Intent(InputAction.CallbackContext context)
        {
            if (canvasManager.Attributes == null) return;
            canvasManager.Attributes.gameObject.SetActive(!canvasManager.Attributes.gameObject.activeSelf);
        }

        private void OnF2Intent(InputAction.CallbackContext context)
        {
            if (canvasManager.Actuators == null) return;
            canvasManager.Actuators.gameObject.SetActive(!canvasManager.Actuators.gameObject.activeSelf);
        }

        private void OnF3Intent(InputAction.CallbackContext context)
        {
            if (canvasManager.Analytics == null) return;
            canvasManager.Analytics.gameObject.SetActive(!canvasManager.Analytics.gameObject.activeSelf);
        }

        private void ScanRawIntents()
        {
            if (suspendInput) return;

            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveXValue = moveValue.x;
            moveYValue = moveValue.y;

            if (Mathf.Abs(moveXValue) != 0f)
            {
                OnMoveXIntent();
            }
            else if (isGrounded)
            {
                rigidBody.linearVelocityX = 0f;
            }

            // if (Mathf.Abs(moveYValue) != 0f)
            // {
            //     OnMoveYIntent();
            // }
            // else
            // {
            //     rigidBody.linearVelocityY = 0f;
            // }

            isHolding = inputActions.Player.Hold.IsPressed();
        }

        private void OnMoveXIntent()
        {
            if (isGrounded)
            {
                execRun = true;
            }
            else
            {
                var direction = Mathf.Sign(moveValue.x);

                // if (wallsAreSlippy && (direction > 0f && isBlockedRight || direction < 0f && isBlockedLeft))
                // {
                //     rigidBody.linearVelocityX = 0f;
                //     return;
                // }

                if (canDodge) execDodge = true;
            }
        }

        private void OnMoveYIntent()
        {
            // TODO
        }

#if false
        private float AngleToSteppedAngle(float angle, int steps) => (int) angle / steps * steps; 
#endif
        private void UpdatePlayerUI()
        {
            if (arrowBaseUI != null)
            {
                var angle = new Vector2(moveXValue, moveYValue).ToAngle();
                // Debug.Log($"Angle: {angle}");
#if false
                // angle = AngleToSteppedAngle(angle, STEPPED_ANGLE_DEGREES);
                // Debug.Log($"Stepped Angle: {angle}");
#endif

                arrowBaseUI.eulerAngles = new Vector3(0f, 0f, angle);
            }

            arrowBaseUI.gameObject.SetActive(vectorBasedJump && moveValue != Vector2.zero);
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

            if (isHoldingUI != null)
            {
                isHoldingUI.Value = isHolding ? "True" : "False";    
                isHoldingUI.Color = isHolding ? Color.green : ORANGE;
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

            if (angleUI != null)
            {
                angleUI.Value = arrowBaseUI.eulerAngles.z.ToString("0.00");
            }

            if (bearingUI != null)
            {
                bearingUI.Value = $"[{bearing.x.ToString("0.00")}, {bearing.y.ToString("0.00")}]";
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
            
            minEnergyThreshold.gameObject.SetActive(canvasManager.EnergyBar.Value >= POWER_MOVE_MIN_ENERGY_VALUE);

            UpdatePlayerUI();
        }

        // Update is called once per frame
        void Update()
        {
            // bearing = DegreeToVector2(arrowBaseUI.eulerAngles.z);
            bearing = new Vector2(moveXValue, moveYValue);

            UpdateUI();

            if (monitorDash)
            {
                monitorDash = false;
                StartCoroutine(Co_Dash());
            }
        }

        private void ApplyRun()
        {
            var inputSensitivity = sensitivityProfile.Evaluate(Mathf.Abs(moveXValue));
            relativeMoveSpeed = moveSpeed * Mathf.Sign(moveXValue) * inputSensitivity;
            rigidBody.linearVelocityX = relativeMoveSpeed;
            execRun = false;
        }

        private void ApplyJump()
        {
            if (vectorBasedJump)
            {
                rigidBody.AddForce(bearing * jumpForce);
            }
            else
            {
                rigidBody.AddForce(Vector2.up * jumpForce);
            }

            audioSource.PlayOneShot(jumpClip, 1f);
            execJump = false;
        }

        private void DrainEnergyLevel(float value)
        {
            if (canvasManager.EnergyBar == null) return;
            canvasManager.EnergyBar.Value -= value;
        }

        private void ApplyPowerJump()
        {
            if (vectorBasedJump)
            {
                rigidBody.AddForce(bearing * powerJumpForce);
            }
            else
            {
                rigidBody.AddForce(Vector2.up * powerJumpForce);
            }

            DrainEnergyLevel(POWER_MOVE_MIN_ENERGY_VALUE);
            audioSource.PlayOneShot(jumpClip, 1f);
            execPowerJump = false;
        }

        private void ApplyDodge()
        {
            var inputSensitivity = sensitivityProfile.Evaluate(Mathf.Abs(moveXValue));
            relativeMoveSpeed = dodgeSpeed * Mathf.Sign(moveXValue) * inputSensitivity;
            rigidBody.linearVelocityX = relativeMoveSpeed;
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
            if (canvasManager.EnergyBar.Value > POWER_MOVE_MIN_ENERGY_VALUE)
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

        private void OnCollision(Collider2D collider) { }

        private void OnTopCollision(Collider2D collider) => OnCollision(collider);
        
        private void OnRightCollision(Collider2D collider) => OnCollision(collider);

        private IEnumerator Co_Squish(float squishFactor)
        {
            var minPosY = 0.425f - 0.425f * squishFactor;
            var posY = 0.425f;
            float elapsedTime = 0f;

            var p1 = spriteShapeController.spline.GetPosition(1);
            var p2 = spriteShapeController.spline.GetPosition(2);

            while (posY > minPosY)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(0.425f, minPosY, elapsedTime * deformationSpeed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
                yield return null;
            }

            posY = minPosY;
            elapsedTime = 0f;

            while (posY < 0.425f)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(minPosY, 0.425f, elapsedTime * deformationSpeed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
                yield return null;
            }
        }

        private void OnBottomCollision(Collider2D collider)
        {
            OnCollision(collider);

            audioSource.PlayOneShot(landClip, 1f);

            var squishPoints = 0f - lastLinearVelocityY;

            if (squishPoints > 0f)
            {
                StartCoroutine(Co_Squish(Mathf.Clamp(SQUISH_PER_POINT * squishPoints, 0f, 1f)));
            }

            var damagePoints = MIN_DAMAGE_VELOCITY - lastLinearVelocityY;

            if (damagePoints > 0f)
            {
                var damage = Mathf.Clamp(DAMAGE_PER_POINT * damagePoints, 0f, 1f);
                canvasManager.HealthBar.Value -= damage;
            }
        }

        private void OnLeftCollision(Collider2D collider) => OnCollision(collider);

        public void OnGainedContactWithEdge(OnTrigger2DHandler instance, Collider2D collider)
        {
            if (instance == topEdgeTrigger)
            {
                isBlockedAbove = true;
                OnTopCollision(collider);
            }
            else if (instance == rightEdgeTrigger)
            {
                isBlockedRight = true;
                OnRightCollision(collider);
            }
            else if (instance == bottomEdgeTrigger)
            {
                isGrounded = true;
                OnBottomCollision(collider);
            }
            else if (instance == leftEdgeTrigger)
            {
                isBlockedLeft = true;
                OnLeftCollision(collider);
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
            state = PlayerStateEnum.Idle;

            if (isGrounded)
            {
                if (Mathf.Abs(rigidBody.linearVelocity.x) > MIN_INPUT_VALUE)
                {
                    if (isDashing)
                    {
                        state = PlayerStateEnum.Dashing;
                    }
                    else if (isGrounded)
                    {
                        state = PlayerStateEnum.Running;
                    }
                }
            }
            else
            {
                if (rigidBody.linearVelocity.y >= MIN_INPUT_VALUE)
                {
                    state = PlayerStateEnum.Jumping;
                }
                else if (rigidBody.linearVelocity.y <= -MIN_INPUT_VALUE)
                {
                    state = PlayerStateEnum.Falling;
                }
            }
        }

        void FixedUpdate()
        {
            AscertState();
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