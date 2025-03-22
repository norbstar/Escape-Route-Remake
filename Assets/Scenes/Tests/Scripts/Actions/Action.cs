using UnityEngine;

namespace Tests.Actions
{
    public abstract class Action : Trigger
    {
        protected bool canAction;

        public bool CanAction => canAction;

        private BinaryConditions binaryConditions = new BinaryConditions();

        public BinaryConditions BinaryConditions => binaryConditions;

        private PropertyConditions propertyConditions = new PropertyConditions();

        public PropertyConditions PropertyConditions => propertyConditions;
        
        private bool TestCondition(BinaryCondition.BinaryEnum state, bool value)
        {
            bool result = false;

            switch (state)
            {
                case BinaryCondition.BinaryEnum.IsMoving:
                    result = value && Essentials.IsMoving() || !value && !Essentials.IsMoving();
                    break;

                case BinaryCondition.BinaryEnum.IsJumping:
                    result = value && Essentials.IsJumping() || !value && !Essentials.IsJumping();
                    break;

                case BinaryCondition.BinaryEnum.IsCrouching:
                    result = value && Essentials.IsCrouching() || !value && !Essentials.IsCrouching();
                    break;

                case BinaryCondition.BinaryEnum.IsGrabbable:
                    result = value && Essentials.IsGrabbable() || !value && !Essentials.IsGrabbable();
                    break;

                case BinaryCondition.BinaryEnum.IsTraversable:
                    result = value && Essentials.IsTraversable() || !value && !Essentials.IsTraversable();
                    break;

                case BinaryCondition.BinaryEnum.IsHolding:
                    result = value && Essentials.IsHolding() || !value && !Essentials.IsHolding();
                    break;

                case BinaryCondition.BinaryEnum.IsDashing:
                    result = value && Essentials.IsDashing() || !value && !Essentials.IsDashing();
                    break;

                case BinaryCondition.BinaryEnum.IsSliding:
                    result = value && Essentials.IsSliding() || !value && !Essentials.IsSliding();
                    break;

                case BinaryCondition.BinaryEnum.IsInputSuspended:
                    result = value && Essentials.IsInputSuspended() || !value && !Essentials.IsInputSuspended();
                    break;

                case BinaryCondition.BinaryEnum.IsBlockedTop:
                    result = value && Essentials.IsBlockedTop() || !value && !Essentials.IsBlockedTop();
                    break;

                case BinaryCondition.BinaryEnum.IsBlockedRight:
                    result = value && Essentials.IsBlockedRight() || !value && !Essentials.IsBlockedRight();
                    break;

                case BinaryCondition.BinaryEnum.IsGrounded:
                    result = value && Essentials.IsGrounded() || !value && !Essentials.IsGrounded();
                    break;

                case BinaryCondition.BinaryEnum.IsBlockedLeft:
                    result = value && Essentials.IsBlockedLeft() || !value && !Essentials.IsBlockedLeft();
                    break;
            }

            return result;
        }

        private bool TestCondition(VelocityCondition condition)
        {
            if (condition.xAxis.include)
            {
                var isNonZero = condition.xAxis.isNonZero;

                if ((isNonZero && Mathf.Abs(Essentials.RigidBody().linearVelocityX) == 0f) || (!isNonZero && Mathf.Abs(Essentials.RigidBody().linearVelocityX) > 0f))
                {
                    return false;
                }

                if (isNonZero)
                {
                    bool canExecute = true;

                    switch (condition.xAxis.sign)
                    {
                        case VelocityCondition.SignEnum.Positive:
                            canExecute = Essentials.RigidBody().linearVelocityX > 0f;
                            break;

                        case VelocityCondition.SignEnum.Negative:
                            canExecute = Essentials.RigidBody().linearVelocityX < 0f;
                            break;

                        case VelocityCondition.SignEnum.Either:
                            canExecute = Mathf.Abs(Essentials.RigidBody().linearVelocityX) > 0f;
                            break;
                    }

                    if (!canExecute) return false;
                }
            }

            if (condition.yAxis.include)
            {
                var isNonZero = condition.yAxis.isNonZero;

                if ((isNonZero && Mathf.Abs(Essentials.RigidBody().linearVelocityY) == 0f) || (!isNonZero && Mathf.Abs(Essentials.RigidBody().linearVelocityY) > 0f))
                {
                    return false;
                }

                if (isNonZero)
                {
                    bool canExecute = true;

                    switch (condition.yAxis.sign)
                    {
                        case VelocityCondition.SignEnum.Positive:
                            canExecute = Essentials.RigidBody().linearVelocityY > 0f;
                            break;

                        case VelocityCondition.SignEnum.Negative:
                            canExecute = Essentials.RigidBody().linearVelocityY < 0f;
                            break;

                        case VelocityCondition.SignEnum.Either:
                            canExecute = Mathf.Abs(Essentials.RigidBody().linearVelocityY) > 0f;
                            break;
                    }

                    if (!canExecute) return false;
                }
            }

            return true;
        }
        
        public void TestAction()
        {
            foreach (var condition in binaryConditions.Conditions)
            {
                canAction = TestCondition(condition.Enum, condition.Boolean);
                if (!canAction) break;
            }

            if (!canAction) return;

            foreach (var condition in propertyConditions.Conditions)
            {
                switch (condition.Enum)
                {
                    case PropertyCondition.PropertyEnum.Velocity:
                        canAction = TestCondition((VelocityCondition) condition);
                        break;
                }

                if (!canAction) break;
            }

            // if (!canAction) return;
        }

        public virtual void FixedUpdate() => TestAction();
    }
}
