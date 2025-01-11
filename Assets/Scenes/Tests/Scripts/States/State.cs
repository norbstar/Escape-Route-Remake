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
                IsBlockedLeft,
            }

            public StateCondition(StateEnum @enum) => this.@enum = @enum;

            private StateEnum @enum;
            
            public StateEnum Enum { get => @enum; set => @enum = value; }

            public bool trueFalse;
        }

        [Serializable]
        public class StateConditions
        {
            private List<StateCondition> conditions;

            private StateCondition.StateEnum @enum;
            
            public StateCondition.StateEnum Enum { get => @enum; set => @enum = value; }

            public List<StateCondition> Conditions { get => conditions.ToList(); set => conditions = value; }

            public void AddCondition(StateCondition.StateEnum condition)
            {
                if (conditions.Exists(c => c.Enum == condition)) return;
                conditions.Add(new StateCondition(condition));
            }

            public void RevokeCondition(StateCondition.StateEnum condition) => conditions.RemoveAll(c => c.Enum == condition);
        }

        [Serializable]
        public class InputCondition
        {
            public enum InputEnum
            {
                Move
            }

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
            
            public InputCondition(InputEnum @enum) => this.@enum = @enum;

            private InputEnum @enum;
            
            public InputEnum Enum { get => @enum; set => @enum = value; }

            public MoveAxisValue xAxis;
            public MoveAxisValue yAxis;
        }

        [Serializable]
        public class InputConditions
        {
            private List<InputCondition> conditions;

            public InputCondition.InputEnum @enum;
            
            public InputCondition.InputEnum Enum { get => @enum; set => @enum = value; }

            public List<InputCondition> Conditions { get => conditions.ToList(); set => conditions = value; }

            public void AddCondition(InputCondition.InputEnum condition)
            {
                if (conditions.Exists(c => c.Enum == condition)) return;
                conditions.Add(new InputCondition(condition));
            }

            public void RevokeCondition(InputCondition.InputEnum condition) => conditions.RemoveAll(c => c.Enum == condition);
        }

        public StateConditions StateCollection { get; set; }

        public InputConditions InputCollection { get; set; }
        
        public PlayerEssentials Essentials { get; set; }

        public bool CanExecute()
        {
            bool canExecute = true;

            // foreach (var condition in StateConditions)
            // {

            // }

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
