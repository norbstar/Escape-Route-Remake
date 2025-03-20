using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;

using Cinemachine;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteShapeModifier))]
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(CustomInputSystem))]
    public class AbstractedPlayer : BasePlayer, PlayerEssentials
    {
        [Header("Components")]
        [SerializeField] ContactMap contactMap;
        [SerializeField] EdgeTriggerHandler edgeTriggerHandler;
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] Light2D spotLight;

        [Header("Player UI")]
        [SerializeField] Transform arrowBaseUI;

        [Header("Analytics")]
        [SerializeField] int instanceID;

        public static float MIN_REGISTERED_VALUE = 0.01f;

        private Rigidbody2D rigidBody;
        // private SpriteShapeRenderer spriteShapeRenderer;
        private SpriteShapeModifier spriteShapeModifier;
        private AudioSource audioSource;
        private CustomInputSystem inputSystem;
        private InputSystem_Actions inputActions;
        private bool isBlockedTop, isBlockedRight, isGrounded, isBlockedLeft, isHolding, isDashing, isGrabbable, isTraversable, isContactable, isCrouching, isSliding;
        // private Actions.Action[] actions;
        // private Events.Event[] events;
        private Trigger[] triggers;
        private PlayerStateEnum playerState;
        private bool /*suspendInput,*/ showArrow;
        private float originalGravityScale;
        private GameObject grabbable;
        private Events.FallEvent fallEvent;
        private bool hasLanded;

        public override Transform Transform() => transform;

        public override Rigidbody2D RigidBody() => rigidBody;

        public override float OriginalGravityScale() => originalGravityScale;

        public override SpriteShapeModifier SpriteShapeModifier() => spriteShapeModifier;

        public override AudioSource AudioSource() => audioSource;

        public override CustomInputSystem InputSystem() => inputSystem;

        public override PlayerStateEnum PlayerState() => playerState;

        public override bool IsMoving() => Mathf.Abs(rigidBody.linearVelocity.x) != MIN_REGISTERED_VALUE;

        public override bool IsJumping() => rigidBody.linearVelocity.y > MIN_REGISTERED_VALUE;

        public override bool IsBlockedTop() => isBlockedTop;

        public override bool IsBlockedRight() => isBlockedRight;

        public override bool IsGrounded() => isGrounded;

        public override bool IsBlockedLeft() => isBlockedLeft;

        public override void SetDashing(bool isDashing) => this.isDashing = isDashing;

        public override bool IsDashing() => isDashing;

        public override void SetHolding(bool isHolding) => this.isHolding = isHolding;

        public override bool IsHolding() => isHolding;

        public override void SetGrabbable(bool isGrabbable) => this.isGrabbable = isGrabbable;

        public override bool IsGrabbable() => isGrabbable;

        public override GameObject GrabbableGameObject() => grabbable;

        public override void SetTraversable(bool isTraversable) => this.isTraversable = isTraversable;

        public override bool IsTraversable() => isTraversable;

        public override void SetContactable(bool isContactable) => this.isContactable = isContactable;

        public override bool IsContactable() => isContactable;

        public override void SetCrouching(bool isCrouching) => this.isCrouching = isCrouching;
        
        public override bool IsCrouching() => isCrouching;

        public override void SetSliding(bool isSliding) => this.isSliding = isSliding;
        
        public override bool IsSliding() => isSliding;

        public override bool IsGravityEnabled() => rigidBody.gravityScale != 0f;

        // public override void SetSuspendInput(bool suspendInput) => this.suspendInput = suspendInput;

        // public override bool IsInputSuspended() => suspendInput;

        public override void SetSuspendInput(bool suspendInput) => inputSystem.Enable(!suspendInput);

        public override bool IsInputSuspended() => inputSystem.IsEnabled();

        public override void ShowArrow(bool showArrow) => this.showArrow = showArrow;

        public override void Awake()
        {
            base.Awake();

            rigidBody = GetComponent<Rigidbody2D>();
            originalGravityScale = rigidBody.gravityScale;
            spriteShapeModifier = GetComponent<SpriteShapeModifier>();
            audioSource = GetComponent<AudioSource>();
            inputSystem = GetComponent<CustomInputSystem>();
            inputActions = inputSystem.InputActions;
            
            triggers = GetComponents<Trigger>();
            
            foreach (var trigger in triggers)
            {
                trigger.Essentials = this;

                if (trigger.GetType() == typeof(Events.FallEvent))
                {
                    fallEvent = (Events.FallEvent) trigger;
                }
            }

            // actions = GetComponents<Actions.Action>();

            // foreach (var action in actions)
            // {
            //     action.Essentials = this;
            // }

            // events = GetComponents<Events.Event>();
            
            // foreach (var evt in events)
            // {
            //     evt.Essentials = this;

            //     if (evt.GetType() == typeof(Events.FallEvent))
            //     {
            //         fallEvent = (Events.FallEvent) evt;
            //     }
            // }

            instanceID = gameObject.GetInstanceID();
        }

        // void OnEnable()
        // {
        //     var virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();

        //     if (virtualCamera != null)
        //     {
        //         var follow = new GameObject("Follow");
        //         follow.transform.position = Vector3.zero;
        //         follow.transform.SetParent(transform);
        //         virtualCamera.Follow = follow.transform;

        //         virtualCamera.Follow = transform;
        //     }
        // }

        void OnEnable() => fallEvent.Subscribe(OnLanded);

        void OnDisable() => fallEvent.Unsubscribe(OnLanded);

        private void OnLanded(Events.FallEvent instance)
        {
            if (!hasLanded)
            {
                hasLanded = true;

                var virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();

                if (virtualCamera != null)
                {
                    var follow = new GameObject("Follow");
                    follow.transform.position = Vector3.zero;
                    follow.transform.SetParent(transform);
                    virtualCamera.Follow = follow.transform;
                }
            }
        }

        private void OnContactWithMap(ContactMap contactMap, Collider2D collider, List<ContactMap.ContactInfo> contacts)
        {
            isGrabbable = isTraversable = false;

            if (contacts.Count > 0)
            {
                var contact = contacts[0];
                isGrabbable = contact.properties.Contains(ObjectPropertyEnum.Grabbable);
                isTraversable = contact.properties.Contains(ObjectPropertyEnum.Traversable);
            }

            if (isGrabbable)
            {
                grabbable = collider.gameObject;
            }
        }

        private void OnLostContactWithMap(ContactMap contactMap, Collider2D collider, List<ContactMap.ContactInfo> contacts)
        {
            isGrabbable = isTraversable = false;
            
            if (contacts.Count > 0)
            {
                var contact = contacts[0];
                isGrabbable = contact.properties.Contains(ObjectPropertyEnum.Grabbable);
                isTraversable = contact.properties.Contains(ObjectPropertyEnum.Traversable);
            }
            
            if (!isGrabbable)
            {
                grabbable = null;
            }
        }

        private void OnContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, EdgeTriggerHandler.Edge edge)
        {
            switch (edge)
            {
                case EdgeTriggerHandler.Edge.Top:
                    isBlockedTop = true;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnBlockedTop();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnBlockedTop();
                    // }

                    // foreach (var evt in events)
                    // {
                    //     evt.OnBlockedTop();
                    // }
                    break;

                case EdgeTriggerHandler.Edge.Right:
                    isBlockedRight = true;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnBlockedRight();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnBlockedRight();
                    // }

                    // foreach (var evt in events)
                    // {
                    //     evt.OnBlockedRight();
                    // }
                    break;

                case EdgeTriggerHandler.Edge.Bottom:
                    isGrounded = true;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnGrounded();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnGrounded();
                    // }

                    // foreach (var evt in events)
                    // {
                    //     evt.OnGrounded();
                    // }
                    break;

                case EdgeTriggerHandler.Edge.Left:
                    isBlockedLeft = true;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnBlockedLeft();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnBlockedLeft();
                    // }

                    // foreach (var evt in events)
                    // {
                    //     evt.OnBlockedLeft();
                    // }
                    break;
            }
        }

        private void OnLostContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, EdgeTriggerHandler.Edge edge)
        {
            switch (edge)
            {
                case EdgeTriggerHandler.Edge.Top:
                    isBlockedTop = false;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnNotBlockedTop();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnNotBlockedTop();
                    // }
                    break;

                case EdgeTriggerHandler.Edge.Right:
                    isBlockedRight = false;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnNotBlockedRight();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnNotBlockedRight();
                    // }
                    break;

                case EdgeTriggerHandler.Edge.Bottom:
                    isGrounded = false;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnNotGrounded();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnNotGrounded();
                    // }
                    break;

                case EdgeTriggerHandler.Edge.Left:
                    isBlockedLeft = false;

                    foreach(var trigger in triggers)
                    {
                        trigger.OnNotBlockedLeft();
                    }

                    // foreach (var state in actions)
                    // {
                    //     state.OnNotBlockedLeft();
                    // }
                    break;
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            contactMap.Register(new ContactMap.Events
            {
                OnContact = OnContactWithMap,
                OnLostContact = OnLostContactWithMap
            });

            edgeTriggerHandler.Register(new EdgeTriggerHandler.Events
            {
                OnContact = OnContactWithEdge,
                OnLostContact = OnLostContactWithEdge
            });
        }

        public override void Activate()
        {
            // if (spriteShapeRenderer == null)
            // {
            //     spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
            // }
            
            // spriteShapeRenderer.enabled = spotLight.enabled = true;

            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            // if (spriteShapeRenderer == null)
            // {
            //     spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
            // }

            // spriteShapeRenderer.enabled = spotLight.enabled = false;

            gameObject.SetActive(false);
        }

        private void AscertState()
        {
            playerState = 0;

            if (isGrounded)
            {
                if (Mathf.Abs(rigidBody.linearVelocity.x) > MIN_REGISTERED_VALUE)
                {
                    if (isDashing)
                    {
                        playerState = playerState.Set(PlayerStateEnum.Dashing);
                    }
                    else if (isSliding)
                    {
                        playerState = playerState.Set(PlayerStateEnum.Sliding);
                    }
                    else if (isCrouching)
                    {
                        playerState = playerState.Set(PlayerStateEnum.Sneaking);
                    }
                    else
                    {
                        playerState = playerState.Set(PlayerStateEnum.Moving);
                    }
                }
                else if (isCrouching)
                {
                    playerState = playerState.Set(PlayerStateEnum.Crouching);
                }
            }
            else
            {
                if (Mathf.Abs(rigidBody.linearVelocity.x) > MIN_REGISTERED_VALUE)
                {
                    playerState = playerState.Set(PlayerStateEnum.Shifting);
                }
            }

            if (rigidBody.linearVelocity.y > MIN_REGISTERED_VALUE)
            {
                playerState = playerState.Set(PlayerStateEnum.Jumping);
            }
            else if (rigidBody.linearVelocity.y < -MIN_REGISTERED_VALUE)
            {
                playerState = playerState.Set(PlayerStateEnum.Falling);
            }

            if (isGrabbable && isHolding)
            {
                playerState = playerState.Set(PlayerStateEnum.Grabbing);
            }
            else if (isTraversable && isHolding)
            {
                playerState = playerState.Set(PlayerStateEnum.Traversing);
            }

            if (System.Convert.ToInt64(playerState) == 0)
            {
                playerState = playerState.Set(PlayerStateEnum.Idle);
            }
        }

        private void AscertStatus() => isHolding = inputActions.Player.Hold.IsPressed();

        private void UpdatePlayerUI()
        {
            if (showArrow && arrowBaseUI != null)
            {
                var moveValue = inputActions.Player.Move.ReadValue<Vector2>();
                var moveAngle = moveValue.ToAngle();
                arrowBaseUI.eulerAngles = new Vector3(0f, 0f, moveAngle);
            }

            arrowBaseUI.gameObject.SetActive(showArrow);
        }

        // Update is called once per frame
        void Update()
        {
            var diasableGravity = (isGrabbable || isTraversable) && isHolding;
            rigidBody.gravityScale = diasableGravity ? 0f : originalGravityScale;
            trailRenderer.enabled = isDashing;

            UpdatePlayerUI();
            AscertStatus();
            AscertState();
            
            isContactable = isGrabbable || isTraversable;
        }
    }
}
