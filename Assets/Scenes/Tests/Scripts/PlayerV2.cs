using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

using UI;
using System.Net.Http.Headers;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteShapeRenderer))]
    [RequireComponent(typeof(SpriteShapeController))]
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(AudioSource))]
    // [RequireComponent(typeof(ContactMap))]
    [RequireComponent(typeof(Analytics))]
    public class PlayerV2 : BasePlayer
    {
        [Header("Components")]
        [SerializeField] OnCollision2DHandler topEdgeCollision;
        [SerializeField] OnCollision2DHandler rightEdgeCollision;
        [SerializeField] OnCollision2DHandler bottomEdgeCollision;
        [SerializeField] OnCollision2DHandler leftEdgeCollision;
        [SerializeField] ContactMap contactMap;

        [Header("Settings")]
        [SerializeField] Transform minEnergyThreshold;
        // [SerializeField] DamageThresholds damageThresholds;
        // [SerializeField] AnimationCurve blendCurve;

        [Header("Audio")]
        [SerializeField] AudioClip jumpClip;
        [SerializeField] AudioClip landClip;
        [SerializeField] AudioClip dashClip;

        [Header("UI")]
        [SerializeField] AttributeUI leftThumbstickXUI;
        [SerializeField] AttributeUI leftThumbstickYUI;
        [SerializeField] AttributeUI rightThumbstickXUI;
        [SerializeField] AttributeUI rightThumbstickYUI;
        [SerializeField] AttributeUI updatesUI;
        [SerializeField] AttributeUI fixedUpdatesUI;
        [SerializeField] AttributeUI isBlockedAboveUI;
        [SerializeField] AttributeUI isBlockedRightUI;
        [SerializeField] AttributeUI isGroundedUI;
        [SerializeField] AttributeUI isBlockedLeftUI;
        [SerializeField] AttributeUI isGrippingUI;
        [SerializeField] AttributeUI angleUI;
        [SerializeField] AttributeUI bearingUI;
        [SerializeField] AttributeUI velocityXUI;
        [SerializeField] AttributeUI velocityYUI;
        [SerializeField] AttributeUI stateUI;

        [Header("Player UI")]
        [SerializeField] Transform arrowBaseUI;

        [Header("Move")]
        [Range(1f, 10f)]
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] bool applySensitivityProfile;
        [SerializeField] bool applyBlending;
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
        [SerializeField] float relativeMoveSpeed;
        [SerializeField] Vector2 lookValue;

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
        [SerializeField] bool isClimbing;
        [SerializeField] bool isGripping;
#if false
        [Header("Sandbox")]
        [SerializeField] LineRenderer circleRenderer;
#endif
        public static float POWER_MOVE_MIN_ENERGY_VALUE = 0.5f;
        public static float MIN_DAMAGE_VELOCITY = -14;
        public static float DAMAGE_PER_POINT = 0.1f;
        public static float SQUISH_PER_POINT = 0.1f;
        public static float MIN_INPUT_VALUE = 0.1f;
        // public static int STEPPED_ANGLE_DEGREES = 15;
        public static Color ORANGE;

        private SceneObjectMapping scene;
        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        // private new Collider2D collider;
        private SpriteShapeController spriteShapeController;
        private AudioSource audioSource;
        // private ContactMap contactMap;
        private Analytics analytics;
        private PlayerStateEnum state;
        private bool execRun, execJump, execPowerJump, execDodge, execDash;
        private Vector2 bearing;
        private float jumpPressStartTime;
        private bool suspendInput, monitorDash, jumpReleased;
        private float lastLinearVelocityY;
        private int pointCount;
        private float defaultGravityScale;
        private int layerMask;

        void Awake()
        {
            scene = FindAnyObjectByType<SceneObjectMapping>();
            rigidBody = GetComponent<Rigidbody2D>();
            // collider = GetComponent<Collider2D>();
            spriteShapeController = GetComponent<SpriteShapeController>();
            audioSource = GetComponent<AudioSource>();
            // contactMap = GetComponent<ContactMap>();
            analytics = GetComponent<Analytics>();
            inputActions = new InputSystem_Actions();
            defaultGravityScale = rigidBody.gravityScale;
            layerMask = LayerMask.GetMask("Player");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // RenderCircle(100, 0.1f);

            ColorUtility.TryParseHtmlString("#FF6100", out Color orange);
            ORANGE = orange;

            pointCount = spriteShapeController.spline.GetPointCount();
            // Debug.Log($"Spline Point Count: {pointCount}");

            topEdgeCollision.Register(new OnCollision2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            rightEdgeCollision.Register(new OnCollision2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            bottomEdgeCollision.Register(new OnCollision2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            });

            leftEdgeCollision.Register(new OnCollision2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            });
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
        }

        void OnDisable() => inputActions.Disable();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log($"OnTriggerEnter2D Name: {collider.name} Tag: {collider.tag}");
            
            if (collider.tag.Equals("Pill"))
            {
                // TODO
            }
        }

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
            var direction = Mathf.Sign(moveValue.y);
            transform.position += Vector3.up * direction *  moveSpeed * 0.5f * Time.fixedDeltaTime;
        }

        private void EvaluateMove()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();

            if (Mathf.Abs(moveValue.x) != 0f)
            {
                OnMoveXIntent();
            }
            else if (isGrounded)
            {
                rigidBody.linearVelocityX = 0f;
            }

            if (Mathf.Abs(moveValue.y) > 0f)
            {
                rigidBody.linearVelocityY = 0f;
                OnMoveYIntent();
            }
            else if (isGrounded)
            {
                rigidBody.linearVelocityY = 0f;
            }
        }

        private void EvaluateLook() => lookValue = inputActions.Player.Look.ReadValue<Vector2>();

        private void EvaluateGrip() => isGripping = inputActions.Player.Grip.IsPressed();

        private void EvaluateRawIntents()
        {
            if (suspendInput) return;

#if false
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveXValue = moveValue.x;
            moveYValue = moveValue.y;
            // moveXValue = Mathf.Sign(moveValue.x) * blendCurve.Evaluate(Mathf.Abs(moveValue.x));
            // moveYValue = Mathf.Sign(moveValue.y) * blendCurve.Evaluate(Mathf.Abs(moveValue.y));

            if (Mathf.Abs(moveXValue) != 0f)
            {
                OnMoveXIntent();
            }
            else if (isGrounded)
            {
                // Apply the brakes
                rigidBody.linearVelocityX = 0f;
            }

            // rigidBody.gravityScale = Mathf.Abs(moveYValue) == 0f ? defaultGravityScale : 0f;

            if (hasContact && Mathf.Abs(moveYValue) > 0f)
            {
                rigidBody.linearVelocityY = 0f;
                OnMoveYIntent();
            }
            else/* if (!isClimbing)*/
            {
                // Apply the brakes
                rigidBody.linearVelocityY = 0f;
            }

            isGripping = inputActions.Player.Grip.IsPressed();
#endif

            EvaluateMove();
            EvaluateLook();
            EvaluateGrip();
        }

        private float Vector2ToAngle(Vector2 value)
        {
            var radians = Mathf.Atan2(value.y, value.x);
            return radians * (180f / Mathf.PI) - 90f;
        }
#if false
        private float AngleToSteppedAngle(float angle, int steps) => (int) angle / steps * steps; 
#endif
        private void UpdatePlayerUI()
        {
            if (arrowBaseUI != null)
            {
                var angle = Vector2ToAngle(new Vector2(moveValue.x, moveValue.y));
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
            if (leftThumbstickXUI != null)
            {
                leftThumbstickXUI.Value = moveValue.x.ToString("0.00");
            }

            if (leftThumbstickYUI != null)
            {
                leftThumbstickYUI.Value = moveValue.y.ToString("0.00");
            }

            if (rightThumbstickXUI != null)
            {
                rightThumbstickXUI.Value = lookValue.x.ToString("0.00");
            }

            if (rightThumbstickYUI != null)
            {
                rightThumbstickYUI.Value = lookValue.y.ToString("0.00");
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

        private Vector2 RadianToVector2(float radian) => new Vector2(-Mathf.Sin(radian), Mathf.Cos(radian));
        
        private Vector2 DegreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);
#endif

        // Update is called once per frame
        void Update()
        {
            UpdateUI();
            
            // bearing = DegreeToVector2(arrowBaseUI.eulerAngles.z);
            bearing = new Vector2(moveValue.x, moveValue.y);

            if (contactMap.HasContacts)
            {
                // Debug.Log($"Contacts: {contactMap.Contacts.Count}");

                foreach (var contact in contactMap.Contacts.Values)
                {
                    Debug.Log($"Contact: {contact.tag}");
                }
            }

            if (monitorDash)
            {
                monitorDash = false;
                StartCoroutine(Co_Dash());
            }

            // RenderCircle(25, 0.55f, 0.015f, Color.grey);
        }

        private void ApplyRun()
        {
            if (applySensitivityProfile)
            {
                var inputSensitivity = sensitivityProfile.Evaluate(Mathf.Abs(moveValue.x));
                relativeMoveSpeed = Mathf.Sign(moveValue.x) * moveSpeed * inputSensitivity;
            }
            else
            {
                relativeMoveSpeed = moveValue.x * moveSpeed;
            }
            
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
            if (scene.EnergyBar == null) return;
            scene.EnergyBar.Value -= value;
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
            var inputSensitivity = sensitivityProfile.Evaluate(Mathf.Abs(moveValue.x));
            relativeMoveSpeed = dodgeSpeed * Mathf.Sign(moveValue.x) * inputSensitivity;
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

        private void OnCollision(Collision2D collision) { }

        private void OnTopCollision(Collision2D collision) => OnCollision(collision);
        
        private void OnRightCollision(Collision2D collision) => OnCollision(collision);

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

        private void OnBottomCollision(Collision2D collision)
        {
            OnCollision(collision);

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
                scene.HealthBar.Value -= damage;
            }
        }

        private void OnLeftCollision(Collision2D collision) => OnCollision(collision);

        public void OnGainedContactWithEdge(OnCollision2DHandler instance, Collision2D collision)
        {
            if (instance == topEdgeCollision)
            {
                isBlockedAbove = true;
                OnTopCollision(collision);
            }
            else if (instance == rightEdgeCollision)
            {
                isBlockedRight = true;
                OnRightCollision(collision);
            }
            else if (instance == bottomEdgeCollision)
            {
                isGrounded = true;
                OnBottomCollision(collision);
            }
            else if (instance == leftEdgeCollision)
            {
                isBlockedLeft = true;
                OnLeftCollision(collision);
            }
        }

        public void OnLostContactWithEdge(OnCollision2DHandler instance, Collision2D collision)
        {
            if (instance == topEdgeCollision)
            {
                isBlockedAbove = false;
            }
            else if (instance == rightEdgeCollision)
            {
                isBlockedRight = false;
            }
            else if (instance == bottomEdgeCollision)
            {
                isGrounded = false;
            }
            else if (instance == leftEdgeCollision)
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

        private void AscertGravity() => rigidBody.gravityScale = (isClimbing) ? 0f : defaultGravityScale;

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
            AscertGravity();
            // UpdateCollisionStatus();
            EvaluateRawIntents();

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