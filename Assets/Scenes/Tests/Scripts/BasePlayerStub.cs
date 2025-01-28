using UnityEngine;

namespace Tests
{
    public class BasePlayerStub : BasePlayer
    {
        public override Transform Transform() => transform;

            public override Rigidbody2D RigidBody() => null;

            public override float OriginalGravityScale() => 0f;

            public override SpriteShapeModifier SpriteShapeModifier() => null;

            public override AudioSource AudioSource() => null;

            public override PlayerStateEnum PlayerState() => PlayerStateEnum.Idle;

            public override bool IsBlockedTop() => false;

            public override bool IsBlockedRight() => false;

            public override bool IsGrounded() => false;

            public override bool IsBlockedLeft() => false;

            public override void SetDashing(bool isDashing) { }

            public override bool IsDashing() => false;

            public override void SetHolding(bool isHolding) { }

            public override bool IsHolding() => false;

            public override void SetGrabbable(bool isGrabbable) { }

            public override bool IsGrabbable() => false;

            public override GameObject GrabbableGameObject() => null;

            public override void SetTraversable(bool isTraversable) { }

            public override bool IsTraversable() => false;

            public override void SetContactable(bool isContactable) { }

            public override bool IsContactable() => false;

            public override void SetCrouching(bool isCrouching) { }
            
            public override bool IsCrouching() => false;

            public override void SetSliding(bool isSliding) { }
            
            public override bool IsSliding() => false;

            public override bool IsGravityEnabled() => false;

            public override void SetSuspendInput(bool suspendInput) { }

            public override bool IsInputSuspended() => false;

            public override void ShowArrow(bool showArrow) { }
    }
}
