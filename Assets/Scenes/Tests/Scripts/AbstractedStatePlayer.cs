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
#if false
        [SerializeField] EdgeCollisionHandler edgeCollisionHandler;
#endif
#if true
        [SerializeField] EdgeTriggerHandler edgeTriggerHandler;
#endif

        public static float MIN_INPUT_VALUE = 0.1f;

        private Rigidbody2D rigidBody;
        private SpriteShapeController spriteShapeController;
        private AudioSource audioSource;
        private InputSystem_Actions inputActions;
        private bool isBlockedTop, isBlockedRight, isGrounded, isBlockedLeft, isDashing, isGripping;
        private State.State[] states;
        private PlayerStateEnum playerState;
        private bool suspendInput;

        public override Rigidbody2D RigidBody() => rigidBody;

        public override SpriteShapeController SpriteShapeController() => spriteShapeController;

        public override AudioSource AudioSource() => audioSource;

        public override InputSystem_Actions InputActions() => inputActions;

        public override PlayerStateEnum PlayerState() => playerState;

        public override bool IsBlockedTop() => isBlockedTop;

        public override bool IsBlockedRight() => isBlockedRight;

        public override bool IsGrounded() => isGrounded;

        public override bool IsBlockedLeft() => isBlockedLeft;

        public override void Dashing(bool isDashing) => this.isDashing = isDashing;

        public override bool IsDashing() => isDashing;

        public override void Gripping(bool isGripping) => this.isGripping = isGripping;

        public override bool IsGripping() => isGripping;

        public override void SuspendInput(bool suspendInput) => this.suspendInput = suspendInput;

        public override bool IsInputSuspended() => suspendInput;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
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
        private void OnContact(Collision2D collision)
        {
            Debug.Log($"OnContact {collision.gameObject.name}");

            if (collision.gameObject.TryGetComponent<ObjectProperties>(out var objectProperties))
            {
                Debug.Log($"OnContact {collision.gameObject.name} {objectProperties}");
            }
        }

        public void OnContactWithEdge(EdgeCollisionHandler instance, Collision2D collision, EdgeCollisionHandler.Edge edge)
        {
            switch (edge)
            {
                case EdgeCollisionHandler.Edge.Top:
                    isBlockedTop = true;

                    foreach (var state in states)
                    {
                        state.OnBlockedTop();
                    }
                    break;

                case EdgeCollisionHandler.Edge.Right:
                    isBlockedRight = true;

                    foreach (var state in states)
                    {
                        state.OnBlockedRight();
                    }
                    break;

                case EdgeCollisionHandler.Edge.Bottom:
                    isGrounded = true;

                    foreach (var state in states)
                    {
                        state.OnGrounded();
                    }
                    break;

                case EdgeCollisionHandler.Edge.Left:
                    isBlockedLeft = true;

                    foreach (var state in states)
                    {
                        state.OnBlockedLeft();
                    }
                    break;
            }

            OnContact(collision);
        }

        private void OnLostContact(Collision2D collision)
        {
            Debug.Log($"OnLostContact {collision.gameObject.name}");

            if (collision.gameObject.TryGetComponent<ObjectProperties>(out var objectProperties))
            {
                Debug.Log($"OnLostContact {collision.gameObject.name} {objectProperties}");
            }
        }

        public void OnLostContactWithEdge(EdgeCollisionHandler instance, Collision2D collision, EdgeCollisionHandler.Edge edge)
        {
            switch (edge)
            {
                case EdgeCollisionHandler.Edge.Top:
                    isBlockedTop = false;

                    foreach (var state in states)
                    {
                        state.OnNotBlockedTop();
                    }
                    break;

                case EdgeCollisionHandler.Edge.Right:
                    isBlockedRight = false;

                    foreach (var state in states)
                    {
                        state.OnNotBlockedRight();
                    }
                    break;

                case EdgeCollisionHandler.Edge.Bottom:
                    isGrounded = false;

                    foreach (var state in states)
                    {
                        state.OnNotGrounded();
                    }
                    break;

                case EdgeCollisionHandler.Edge.Left:
                    isBlockedLeft = false;

                    foreach (var state in states)
                    {
                        state.OnNotBlockedLeft();
                    }
                    break;
            }

            OnLostContact(collision);
        }
#endif
#if true
        private void OnContact(Collider2D collider)
        {
            Debug.Log($"OnContact {collider.gameObject.name}");

            if (collider.gameObject.TryGetComponent<ObjectProperties>(out var objectProperties))
            {
                Debug.Log($"OnContact {collider.gameObject.name} {objectProperties}");
            }
        }

        public void OnContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, EdgeTriggerHandler.Edge edge)
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

            OnContact(collider);
        }

        private void OnLostContact(Collider2D collider)
        {
            Debug.Log($"OnLostContact {collider.gameObject.name}");

            if (collider.gameObject.TryGetComponent<ObjectProperties>(out var objectProperties))
            {
                Debug.Log($"OnLostContact {collider.gameObject.name} {objectProperties}");
            }
        }

        public void OnLostContactWithEdge(EdgeTriggerHandler instance, Collider2D collider, EdgeTriggerHandler.Edge edge)
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

            OnLostContact(collider);
        }
#endif
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
#if false
            edgeCollisionHandler.Register(new EdgeCollisionHandler.Events
            {
                OnContact = OnContactWithEdge,
                OnLostContact = OnLostContactWithEdge
            });
#endif
#if true
            edgeTriggerHandler.Register(new EdgeTriggerHandler.Events
            {
                OnContact = OnContactWithEdge,
                OnLostContact = OnLostContactWithEdge
            });
        }
#endif
        private void AscertainState()
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

            if (System.Convert.ToInt64(playerState) == 0)
            {
                playerState = playerState.Set(PlayerStateEnum.Idle);
            }
        }

        // Update is called once per frame
        void Update() => AscertainState();
    }
}
