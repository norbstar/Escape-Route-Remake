using System.Collections.Generic;

using UnityEngine;
using UnityEngine.U2D;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteShapeRenderer))]
    [RequireComponent(typeof(SpriteShapeController))]
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(AudioSource))]
    public class AbstractedStatePlayer : BasePlayer, PlayerEssentials
    {
        [Header("Components")]
        [SerializeField] ContactMap contactMap;
        [SerializeField] EdgeTriggerHandler edgeTriggerHandler;

        [Header("Player UI")]
        [SerializeField] Transform arrowBaseUI;

        public static float MIN_INPUT_VALUE = 0.1f;

        private Rigidbody2D rigidBody;
        private SpriteShapeController spriteShapeController;
        private AudioSource audioSource;
        private InputSystem_Actions inputActions;
        private bool isBlockedTop, isBlockedRight, isGrounded, isBlockedLeft, isHolding, isDashing, isGrabbable, isTraversable;
        private State.State[] states;
        private PlayerStateEnum playerState;
        private bool suspendInput, showArrow;
        private float originalGravityScale;
        private GameObject grabbable;

        public override Transform Transform() => transform;

        public override Rigidbody2D RigidBody() => rigidBody;

        public override float OriginalGravityScale() => originalGravityScale;

        public override SpriteShapeController SpriteShapeController() => spriteShapeController;

        public override AudioSource AudioSource() => audioSource;

        public override PlayerStateEnum PlayerState() => playerState;

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

        public override bool IsGravityEnabled() => rigidBody.gravityScale != 0f;

        public override void SetSuspendInput(bool suspendInput) => this.suspendInput = suspendInput;

        public override bool IsInputSuspended() => suspendInput;

        public override void ShowArrow(bool showArrow) => this.showArrow = showArrow;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            originalGravityScale = rigidBody.gravityScale;
            spriteShapeController = GetComponent<SpriteShapeController>();
            audioSource = GetComponent<AudioSource>();
            inputActions = new InputSystem_Actions();
            states = GetComponents<State.State>();

            foreach (var state in states)
            {
                state.Essentials = this;
            }
        }

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

#if false
        private void OnContact(Collider2D collider)
        {
            Debug.Log($"OnContact {collider.gameObject.name}");

            if (collider.gameObject.TryGetComponent<ObjectProperties>(out var objectProperties))
            {
                Debug.Log($"OnContact {collider.gameObject.name} {objectProperties}");
            }
        }
#endif
        private void OnContactWithMap(ContactMap contactMap, Collider2D collider, List<ContactMap.ContactInfo> contacts)
        {

            isGrabbable = isTraversable = false;

            if (contacts.Count > 0)
            {
                var contact = contacts[0];
                isGrabbable = contact.properties.Contains(ObjectPropertyEnum.Grabbable);
                isTraversable = contact.properties.Contains(ObjectPropertyEnum.Traversable);
            }

            // Debug.Log($"OnContactWithMap Collider: {collider.gameObject} Contacts Count: {contacts.Count} Is Grabbable: {isGrabbable} Is Traversable: {isTraversable}");
            
            if (isGrabbable)
            {
                grabbable = collider.gameObject;
            }

            // OnContact(collider);
        }

        private void OnContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, EdgeTriggerHandler.Edge edge)
        {
            switch (edge)
            {
                case EdgeTriggerHandler.Edge.Top:
                    isBlockedTop = true;

                    foreach (var state in states)
                    {
                        state.OnBlockedTop();
                    }
                    break;

                case EdgeTriggerHandler.Edge.Right:
                    isBlockedRight = true;

                    foreach (var state in states)
                    {
                        state.OnBlockedRight();
                    }
                    break;

                case EdgeTriggerHandler.Edge.Bottom:
                    isGrounded = true;

                    foreach (var state in states)
                    {
                        state.OnGrounded();
                    }
                    break;

                case EdgeTriggerHandler.Edge.Left:
                    isBlockedLeft = true;

                    foreach (var state in states)
                    {
                        state.OnBlockedLeft();
                    }
                    break;
            }

            // OnContact(collider);
        }
#if false
        private void OnLostContact(Collider2D collider)
        {
            Debug.Log($"OnLostContact {collider.gameObject.name}");

            if (collider.gameObject.TryGetComponent<ObjectProperties>(out var objectProperties))
            {
                Debug.Log($"OnLostContact {collider.gameObject.name} {objectProperties}");
            }
        }
#endif
        private void OnLostContactWithMap(ContactMap contactMap, Collider2D collider, List<ContactMap.ContactInfo> contacts)
        {
            isGrabbable = isTraversable = false;
            
            if (contacts.Count > 0)
            {
                var contact = contacts[0];
                isGrabbable = contact.properties.Contains(ObjectPropertyEnum.Grabbable);
                isTraversable = contact.properties.Contains(ObjectPropertyEnum.Traversable);
            }

            // Debug.Log($"OnLostContactWithMap Collider: {collider.gameObject} Contacts Count: {contacts.Count} Is Grabbable: {isGrabbable} Is Traversable: {isTraversable}");
            
            if (!isGrabbable)
            {
                grabbable = null;
            }

            // OnContact(collider);
        }

        private void OnLostContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, EdgeTriggerHandler.Edge edge)
        {
            switch (edge)
            {
                case EdgeTriggerHandler.Edge.Top:
                    isBlockedTop = false;

                    foreach (var state in states)
                    {
                        state.OnNotBlockedTop();
                    }
                    break;

                case EdgeTriggerHandler.Edge.Right:
                    isBlockedRight = false;

                    foreach (var state in states)
                    {
                        state.OnNotBlockedRight();
                    }
                    break;

                case EdgeTriggerHandler.Edge.Bottom:
                    isGrounded = false;

                    foreach (var state in states)
                    {
                        state.OnNotGrounded();
                    }
                    break;

                case EdgeTriggerHandler.Edge.Left:
                    isBlockedLeft = false;

                    foreach (var state in states)
                    {
                        state.OnNotBlockedLeft();
                    }
                    break;
            }

            // OnLostContact(collider);
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

        private void AscertState()
        {
            playerState = 0;

            if (isGrounded)
            {
                if (Mathf.Abs(rigidBody.linearVelocity.x) != 0/*>= MIN_INPUT_VALUE*/)
                {
                    playerState = playerState.Set(isDashing ? PlayerStateEnum.Dashing : PlayerStateEnum.Running);
                }
            }
            else
            {
                if (Mathf.Abs(rigidBody.linearVelocity.x) != 0/*>= MIN_INPUT_VALUE*/)
                {
                    playerState = playerState.Set(PlayerStateEnum.Shifting);
                }

                if (rigidBody.linearVelocity.y > 0/*>= MIN_INPUT_VALUE*/)
                {
                    playerState = playerState.Set(PlayerStateEnum.Jumping);
                }
                else if (rigidBody.linearVelocity.y < 0/*<= -MIN_INPUT_VALUE*/)
                {
                    playerState = playerState.Set(PlayerStateEnum.Falling);
                }
            }

            if (isGrabbable)
            {
                playerState = playerState.Set(PlayerStateEnum.Grabbing);
            }
            else if (isTraversable)
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

            UpdatePlayerUI();
            AscertStatus();
            AscertState();
        }
    }
}
