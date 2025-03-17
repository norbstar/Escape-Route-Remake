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
    public abstract class BasePlayer : SingletonMonoBehaviour<BasePlayer>, PlayerEssentials
    {
        private PlayerStateActivation playerStateActivation;

        public override void Awake()
        {
            base.Awake();
            playerStateActivation = GetComponent<PlayerStateActivation>();
        }

        public virtual void Activate() { }

        public bool IsActive => gameObject.activeSelf;

        public virtual void Deactivate() { }

        public abstract Transform Transform();

        public abstract Rigidbody2D RigidBody();

        public abstract float OriginalGravityScale();

        public abstract SpriteShapeModifier SpriteShapeModifier();

        public abstract AudioSource AudioSource();

        public abstract PlayerStateEnum PlayerState();

        public PlayerStateActivation PlayerStateActivation() => playerStateActivation;

        public abstract bool IsMoving();

        public abstract bool IsJumping();

        public abstract bool IsBlockedTop();

        public abstract bool IsBlockedRight();

        public abstract bool IsGrounded();

        public abstract bool IsBlockedLeft();

        public abstract void SetDashing(bool isDashing);

        public abstract bool IsDashing();

        public abstract void SetHolding(bool isHolding);

        public abstract bool IsHolding();

        public abstract void SetGrabbable(bool isGrabbable);

        public abstract bool IsGrabbable();

        public abstract GameObject GrabbableGameObject();

        public abstract void SetTraversable(bool isTraversing);

        public abstract bool IsTraversable();

        public abstract void SetContactable(bool isContactable);

        public abstract bool IsContactable();

        public abstract void SetCrouching(bool isCrouched);
        
        public abstract bool IsCrouching();

        public abstract void SetSliding(bool isSliding);
        
        public abstract bool IsSliding();

        public abstract bool IsGravityEnabled();

        public abstract void SetSuspendInput(bool suspendInput);

        public abstract bool IsInputSuspended();

        public abstract void ShowArrow(bool showArrow);
    }
}
