using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Tests.Actions
{
    public abstract class Action : Trigger
    {
        [Serializable]
        public class BinaryCondition
        {
            public enum BinaryEnum
            {
                IsMoving,
                IsJumping,
                IsCrouching,
                IsGrabbable,
                IsTraversable,
                IsHolding,
                IsDashing,
                IsSliding,
                IsInputSuspended,
                IsBlockedTop,
                IsBlockedRight,
                IsGrounded,
                IsBlockedLeft
            }

            public BinaryCondition(BinaryEnum @enum) => this.@enum = @enum;

            private BinaryEnum @enum;
            
            public BinaryEnum Enum { get => @enum; set => @enum = value; }

            public bool boolean;
        }

        [Serializable]
        public class BinaryConditions
        {
            private List<BinaryCondition> conditions;

            private BinaryCondition.BinaryEnum @enum;
            
            public BinaryCondition.BinaryEnum Enum { get => @enum; set => @enum = value; }

            public List<BinaryCondition> Conditions
            {
                get
                {
                    if (conditions == null)
                    {
                        conditions = new List<BinaryCondition>();
                    }

                    return conditions.ToList();
                }
                
                set => conditions = value;
            }

            public void AddCondition(BinaryCondition.BinaryEnum condition)
            {
                if (conditions.Exists(c => c.Enum == condition)) return;
                conditions.Add(new BinaryCondition(condition));
            }

            public void RevokeCondition(BinaryCondition.BinaryEnum condition) => conditions.RemoveAll(c => c.Enum == condition);
        }

        [Serializable]
        public abstract class PropertyCondition
        {
            public enum PropertyEnum
            {
                Velocity
            }

            public PropertyCondition(PropertyEnum @enum) => this.@enum = @enum;

            private PropertyEnum @enum;
            
            public PropertyEnum Enum { get => @enum; set => @enum = value; }
        }

        [Serializable]
        public class VelocityCondition : PropertyCondition
        {
            public enum SignEnum
            {
                Positive,
                Negative,
                Either
            }

            [Serializable]
            public class AxisValue
            {
                public bool include;
                public bool isNonZero;
                public SignEnum sign;
            }
            
            public VelocityCondition(PropertyEnum @enum) : base(@enum) { }

            public AxisValue xAxis;
            public AxisValue yAxis;
        }

        [Serializable]
        public class PropertyConditions
        {
            private List<PropertyCondition> conditions;

            public PropertyCondition.PropertyEnum @enum;
            
            public PropertyCondition.PropertyEnum Enum { get => @enum; set => @enum = value; }

            public List<PropertyCondition> Conditions
            {
                get
                {
                    if (conditions == null)
                    {
                        conditions = new List<PropertyCondition>();
                    }

                    return conditions.ToList();
                }
                
                set => conditions = value;
            }

            public void AddCondition(PropertyCondition.PropertyEnum condition)
            {
                if (conditions.Exists(c => c.Enum == condition)) return;

                switch (condition)
                {
                    case PropertyCondition.PropertyEnum.Velocity:
                        conditions.Add(new VelocityCondition(condition));
                        break;
                }
            }

            public void RevokeCondition(PropertyCondition.PropertyEnum condition) => conditions.RemoveAll(c => c.Enum == condition);
        }

        protected bool canExecute;

        public bool CanExecute => canExecute;

        public BinaryConditions BinaryCollection { get; set; } = new BinaryConditions();

        public PropertyConditions PropertyCollection { get; set; } = new PropertyConditions();
        
        private bool TestBinaryCondition(BinaryCondition.BinaryEnum state, bool value)
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

        private bool TestVelocityCondition(VelocityCondition condition)
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
        
        public void TestConditions()
        {
            var canExecute = true;

            foreach (var condition in BinaryCollection.Conditions)
            {
                canExecute = TestBinaryCondition(condition.Enum, condition.boolean);
                if (!canExecute) break;
            }

            if (canExecute)
            {
                foreach (var condition in PropertyCollection.Conditions)
                {
                    switch (condition.Enum)
                    {
                        case PropertyCondition.PropertyEnum.Velocity:
                            canExecute = TestVelocityCondition((VelocityCondition) condition);
                            break;
                    }

                    if (!canExecute) break;
                }
            }
            
            this.canExecute = canExecute;
        }

        public virtual void FixedUpdate() => TestConditions();
    }
}
