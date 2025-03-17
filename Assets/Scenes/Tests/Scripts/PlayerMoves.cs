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
    public class PlayerMoves : MonoBehaviour, PlayerEssentials
    {
        [Header("Components")]
        [SerializeField] OnCollision2DHandler bottomEdgeCollision;

        [Header("Audio")]
        [SerializeField] AudioClip jumpClip;
        [SerializeField] AudioClip landClip;

        [Header("UI")]
        [SerializeField] AttributeUI isGroundedUI;
        
        [Header("Move")]
        [Range(1f, 10f)]
        [SerializeField] float moveSpeed = 5f;

        [Header("Jump")]
        [Range(400f, 800f)]
        [SerializeField] float jumpForce = 600f;
        [SerializeField] bool canPowerJump;
        [Range(600f, 1000f)]
        [SerializeField] float powerJumpForce = 800f;
        [SerializeField] float powerJumpThreshold = 0.2f;

        [Header("Deformation")]
        [SerializeField] float deformationSpeed = 2.5f;

        public static float MIN_DAMAGE_VELOCITY = -14;
        public static float DAMAGE_PER_POINT = 0.1f;
        public static float SQUISH_PER_POINT = 0.1f;

        private Rigidbody2D rigidBody;
        private SpriteShapeController spriteShapeController;
        private SpriteShapeModifier spriteShapeModifier;

        private AudioSource audioSource;
        private InputSystem_Actions inputActions;
        private bool execRun, execJump, execPowerJump;
        private Vector2 moveValue;
        private bool isBlockedTop, isBlockedRight, isGrounded, isBlockedLeft, isHolding, isDashing, isGrabbable, isTraversable, isContactable, isCrouching, isSliding;
        private float jumpPressStartTime;
        private bool jumpReleased;
        private float lastLinearVelocityY;
        private PlayerStateEnum playerState;
        private bool suspendInput, showArrow;
        private float originalGravityScale;
        private GameObject grabbable;
        private LayerMask layerMask;
        
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            originalGravityScale = rigidBody.gravityScale;
            spriteShapeController = GetComponent<SpriteShapeController>();
            audioSource = GetComponent<AudioSource>();
            inputActions = new InputSystem_Actions();
            layerMask = LayerMask.GetMask("Player");
            
            var states = GetComponents<States.State>();

            foreach (var state in states)
            {
                state.Essentials = this;
                // Debug.Log($"Registered State: {state.GetType()}");
            }
        }

        public UnityEngine.Transform Transform() => transform;

        public Rigidbody2D RigidBody() => rigidBody;

        public float OriginalGravityScale() => originalGravityScale;

        // public SpriteShapeController SpriteShapeController() => spriteShapeController;

        public SpriteShapeModifier SpriteShapeModifier() => spriteShapeModifier;

        public AudioSource AudioSource() => audioSource;

        public InputSystem_Actions InputActions() => inputActions;

        public PlayerStateEnum PlayerState() => playerState;

        public PlayerStateActivation PlayerStateActivation() => null;

        public bool IsMoving() => Mathf.Abs(rigidBody.linearVelocity.x) > 0f;

        public bool IsJumping() => rigidBody.linearVelocity.y > 0f;

        public bool IsBlockedTop() => isBlockedTop;

        public bool IsBlockedRight() => isBlockedRight;

        public bool IsGrounded() => isGrounded;

        public bool IsBlockedLeft() => isBlockedLeft;

        public void SetDashing(bool isDashing) => this.isDashing = isDashing;

        public bool IsDashing() => isDashing;

        public void SetHolding(bool isHolding) => this.isHolding = isHolding;

        public bool IsHolding() => isHolding;

        public void SetGrabbable(bool isGrabbable) => this.isGrabbable = isGrabbable;

        public bool IsGrabbable() => isGrabbable;

        public GameObject GrabbableGameObject() => grabbable;

        public void SetTraversable(bool isTraversing) => this.isTraversable = isTraversing;

        public bool IsTraversable() => isTraversable;

        public void SetContactable(bool isContactable) => this.isContactable = isContactable;

        public bool IsContactable() => isContactable;

        public void SetCrouching(bool isCrouching) => this.isCrouching = isCrouching;
        
        public bool IsCrouching() => isCrouching;

        public void SetSliding(bool isSliding) => this.isSliding = isSliding;
        
        public bool IsSliding() => isSliding;

        public bool IsGravityEnabled() => rigidBody.gravityScale != 0f;

        public void SetSuspendInput(bool suspendInput) => this.suspendInput = suspendInput;

        public bool IsInputSuspended() => suspendInput;

        public void ShowArrow(bool showArrow) => this.showArrow = showArrow;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            bottomEdgeCollision.Register(new OnCollision2DHandler.Events
            {
                Gained = OnGainedContactWithEdge,
                Lost = OnLostContactWithEdge
            }, layerMask);
        }

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
                }
                else if (canPowerJump && isGrounded && Time.time - jumpPressStartTime > powerJumpThreshold)
                {
                    execPowerJump = true;
                }

                yield return null;
            }
        }

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            if (!isGrounded) return;
            StartCoroutine(Co_MonitorJumpIntent());
        }

        private void OnJumpReleaseIntent(InputAction.CallbackContext context) => jumpReleased = true;

        private void OnRunIntent()
        {
            if (!isGrounded) return;
            execRun = true;
        }

        private void EvaluateMove()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();

            if (isGrounded)
            {
                if (Mathf.Abs(moveValue.x) != 0f)
                {
                    OnRunIntent();
                }
                else
                {
                    rigidBody.linearVelocityX = 0f;
                }
            }
        }

        private void EvaluateRawIntents() => EvaluateMove();

        private void UpdateUI()
        {
            if (isGroundedUI != null)
            {
                isGroundedUI.Value = isGrounded ? "True" : "False";
                isGroundedUI.Color = isGrounded ? Color.white : Color.grey;
            }
        }

        // Update is called once per frame
        void Update() => UpdateUI();

        private void ApplyRun()
        {
            rigidBody.linearVelocityX = moveValue.x * moveSpeed;
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
            audioSource.PlayOneShot(jumpClip, 1f);
            execPowerJump = false;
        }

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
            audioSource.PlayOneShot(landClip, 1f);

            var squishPoints = 0f - lastLinearVelocityY;

            if (squishPoints > 0f)
            {
                StartCoroutine(Co_Squish(Mathf.Clamp(SQUISH_PER_POINT * squishPoints, 0f, 1f)));
            }
        }

        public void OnGainedContactWithEdge(OnCollision2DHandler instance, Collision2D collision)
        {
            if (instance == bottomEdgeCollision)
            {
                isGrounded = true;
                OnBottomCollision(collision);
            }
        }

        public void OnLostContactWithEdge(OnCollision2DHandler instance, Collision2D collision)
        {
            if (instance == bottomEdgeCollision)
            {
                isGrounded = false;
            }
        }

        void FixedUpdate()
        {
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

            lastLinearVelocityY = rigidBody.linearVelocityY;
        }
    }
}
