using UnityEngine;
using UnityEngine.U2D;

namespace Tests
{
    public interface PlayerEssentials
    {
        Rigidbody2D RigidBody();
        float OriginalGravityScale();
        SpriteShapeController SpriteShapeController();
        AudioSource AudioSource();
        InputSystem_Actions InputActions();
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
        void SetSuspendInput(bool suspendInput);
        bool IsInputSuspended();
        void ShowArrow(bool showArrow);
    }
}
