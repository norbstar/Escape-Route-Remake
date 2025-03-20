using UnityEngine;

namespace Tests
{
    public interface PlayerEssentials
    {
        Transform Transform();
        Rigidbody2D RigidBody();
        float OriginalGravityScale();
        SpriteShapeModifier SpriteShapeModifier();
        AudioSource AudioSource();
        CustomInputSystem InputSystem();
        PlayerStateEnum PlayerState();
        PlayerActions PlayerStateActivation();
        bool IsMoving();
        bool IsJumping();
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
        void SetContactable(bool isContactable);
        bool IsContactable();
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
