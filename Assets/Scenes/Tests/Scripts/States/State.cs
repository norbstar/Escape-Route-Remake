using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Tests.States
{
    public abstract class State : MonoBehaviour
    {
        [Serializable]
        public class StateCondition
        {
            public enum StateEnum
            {
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

            public StateCondition(StateEnum @enum) => this.@enum = @enum;

            private StateEnum @enum;
            
            public StateEnum Enum { get => @enum; set => @enum = value; }

            public bool boolean;
        }

        [Serializable]
        public class StateConditions
        {
            private List<StateCondition> conditions;

            private StateCondition.StateEnum @enum;
            
            public StateCondition.StateEnum Enum { get => @enum; set => @enum = value; }

            public List<StateCondition> Conditions
            {
                get
                {
                    if (conditions == null)
                    {
                        conditions = new List<StateCondition>();
                    }

                    return conditions.ToList();
                }
                
                set => conditions = value;
            }

            public void AddCondition(StateCondition.StateEnum condition)
            {
                if (conditions.Exists(c => c.Enum == condition)) return;
                conditions.Add(new StateCondition(condition));
            }

            public void RevokeCondition(StateCondition.StateEnum condition) => conditions.RemoveAll(c => c.Enum == condition);
        }

        [Serializable]
        public abstract class PropertyCondition
        {
            public enum InputEnum
            {
                Move
            }

            public PropertyCondition(InputEnum @enum) => this.@enum = @enum;

            private InputEnum @enum;
            
            public InputEnum Enum { get => @enum; set => @enum = value; }
        }

        [Serializable]
        public class MoveCondition : PropertyCondition
        {
            public enum SignEnum
            {
                Positive,
                Negative,
                Either
            }

            [Serializable]
            public class MoveAxisValue
            {
                public bool include;
                public bool isNonZero;
                public SignEnum sign;
            }
            
            public MoveCondition(InputEnum @enum) : base(@enum) { }

            public MoveAxisValue xAxis;
            public MoveAxisValue yAxis;
        }

        [Serializable]
        public class PropertyConditions
        {
            private List<PropertyCondition> conditions;

            public PropertyCondition.InputEnum @enum;
            
            public PropertyCondition.InputEnum Enum { get => @enum; set => @enum = value; }

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

            public void AddCondition(PropertyCondition.InputEnum condition)
            {
                if (conditions.Exists(c => c.Enum == condition)) return;

                switch (condition)
                {
                    case PropertyCondition.InputEnum.Move:
                        conditions.Add(new MoveCondition(condition));
                        break;
                }
            }

            public void RevokeCondition(PropertyCondition.InputEnum condition) => conditions.RemoveAll(c => c.Enum == condition);
        }

        public StateConditions StateCollection { get; set; } = new StateConditions();

        public PropertyConditions PropertyCollection { get; set; } = new PropertyConditions();
        
        public PlayerEssentials Essentials { get; set; }

        private bool TestBooleanCondition(StateCondition.StateEnum stateEnum, bool trueFalse) => trueFalse && Essentials.IsCrouching() || !trueFalse && !Essentials.IsCrouching();

        private bool TestMoveCondition(MoveCondition condition)
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
                        case MoveCondition.SignEnum.Positive:
                            canExecute = Essentials.RigidBody().linearVelocityX > 0f;
                            break;

                        case MoveCondition.SignEnum.Negative:
                            canExecute = Essentials.RigidBody().linearVelocityX < 0f;
                            break;

                        case MoveCondition.SignEnum.Either:
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
                        case MoveCondition.SignEnum.Positive:
                            canExecute = Essentials.RigidBody().linearVelocityY > 0f;
                            break;

                        case MoveCondition.SignEnum.Negative:
                            canExecute = Essentials.RigidBody().linearVelocityY < 0f;
                            break;

                        case MoveCondition.SignEnum.Either:
                            canExecute = Mathf.Abs(Essentials.RigidBody().linearVelocityY) > 0f;
                            break;
                    }

                    if (!canExecute) return false;
                }
            }

            return true;
        }

        public bool CanExecute()
        {
            bool canExecute = true;

            foreach (var condition in StateCollection.Conditions)
            {
                if (!canExecute) break;
                canExecute = TestBooleanCondition(condition.Enum, condition.boolean);
            }

            foreach (var condition in PropertyCollection.Conditions)
            {
                if (!canExecute) break;

                switch (condition.Enum)
                {
                    case PropertyCondition.InputEnum.Move:
                        canExecute = TestMoveCondition((MoveCondition) condition);
                        break;
                }
            }

            return canExecute;
        }

        public virtual void OnBlockedTop() { }

        public virtual void OnNotBlockedTop() { }

        public virtual void OnBlockedRight() { }

        public virtual void OnNotBlockedRight() { }

        public virtual void OnGrounded() { }

        public virtual void OnNotGrounded() { }

        public virtual void OnBlockedLeft() { }

        public virtual void OnNotBlockedLeft() { }
    }
}
