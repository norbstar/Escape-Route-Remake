using UnityEngine;
using UnityEngine.U2D;

namespace Tests
{
    public interface PlayerEssentials
    {
        Rigidbody2D RigidBody();
        SpriteShapeController SpriteShapeController();
        AudioSource AudioSource();
        InputSystem_Actions InputActions();
        PlayerStateEnum PlayerState();
        bool IsBlockedTop();
        bool IsBlockedRight();
        bool IsGrounded();
        bool IsBlockedLeft();
        void Dashing(bool isDashing);
        bool IsDashing();
        void Gripping(bool isGripping);
        bool IsGripping();
        void SuspendInput(bool suspendInput);
        bool IsInputSuspended();
    }
}
