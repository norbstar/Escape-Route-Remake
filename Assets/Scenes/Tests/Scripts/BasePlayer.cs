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
    public abstract class BasePlayer : MonoBehaviour, PlayerEssentials
    {
        public abstract Rigidbody2D RigidBody();

        public abstract SpriteShapeController SpriteShapeController();

        public abstract AudioSource AudioSource();

        public abstract InputSystem_Actions InputActions();

        public abstract PlayerStateEnum PlayerState();

        public abstract bool IsBlockedTop();

        public abstract bool IsBlockedRight();

        public abstract bool IsGrounded();

        public abstract bool IsBlockedLeft();

        public abstract void Dashing(bool isDashing);

        public abstract bool IsDashing();

        public abstract void Gripping(bool isGripping);

        public abstract bool IsGripping();

        public abstract void SuspendInput(bool suspendInput);

        public abstract bool IsInputSuspended();
    }
}
