using UnityEngine;
using UnityEngine.U2D;

namespace Tests
{
    public interface PlayerEssentials
    {
        Transform Transform();
        Rigidbody2D RigidBody();
        float OriginalGravityScale();
        SpriteShapeController SpriteShapeController();
        AudioSource AudioSource();
        PlayerStateEnum PlayerState();
        bool IsBlockedTop();
        bool IsBlockedRight();
        bool IsGrounded();
        bool IsBlockedLeft();
        void SetDashing(bool isDashing);
        bool IsDashing();
        void SetHolding(bool isHolding);
        bool IsHolding();
        void SetGrabbable(bool isGrabbable);
        bool IsGrabbable();
        GameObject GrabbableGameObject();
        void SetTraversable(bool isTraversable);
        bool IsTraversable();
        void SetCrouching(bool isCrouched);
        bool IsCrouching();
        void SetSliding(bool isSliding);
        bool IsSliding();
        bool IsGravityEnabled();
        void SetSuspendInput(bool suspendInput);
        bool IsInputSuspended();
        void ShowArrow(bool showArrow);
    }
}
