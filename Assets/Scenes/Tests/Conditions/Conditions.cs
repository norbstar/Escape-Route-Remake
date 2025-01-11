using System;
using System.Collections.Generic;

using UnityEngine;

public class Conditions : MonoBehaviour
{
    public enum ConditionEnum
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
        Move
    }

    public enum StateEnum
    {
        IsCrouching,
        //...
    }

    public enum InputEnum
    {
        Move
    }

    public enum ConditionTypeEnum
    {
        Boolean,
        Complex
    }

    [Serializable]
    public abstract class Condition
    {
        public Condition(ConditionEnum condition)
        {
            this.condition = condition;
            conditionType = (condition == ConditionEnum.Move) ? ConditionTypeEnum.Complex : ConditionTypeEnum.Boolean; 

            // int foo = (int) condition;
            // var bar = (ConditionEnum) foo;
            // Debug.Log($"Condition: {bar}");
        }

        public ConditionEnum condition;
        public ConditionTypeEnum conditionType;
    }

    [Serializable]
    public class BooleanCondition : Condition
    {
        public BooleanCondition(ConditionEnum condition) : base(condition) { }

        public bool qualifyingState;
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

    [Serializable]
    public class MoveCondition : Condition
    {
        public MoveCondition(ConditionEnum condition) : base(condition) { }
        public MoveAxisValue xAxis;
        public MoveAxisValue yAxis;
    }

    [SerializeField] List<Condition> collection;

    private ConditionEnum @enum;
    
    public List<Condition> Collection { get => collection; set => collection = value; }

    public ConditionEnum Enum { get => @enum; set => @enum = value; }

    public void AddCondition(ConditionEnum condition)
    {
        if (condition == ConditionEnum.Move)
        {
            collection.Add(new MoveCondition(condition));
        }
        else
        {
            int lastBooleanIndex = 0;

            if (collection.Count > 0)
            {
                lastBooleanIndex = collection.FindLastIndex(c => c.conditionType == ConditionTypeEnum.Boolean);
                lastBooleanIndex = (lastBooleanIndex != -1) ? ++lastBooleanIndex : 0;

                // if (lastBooleanIndex != -1)
                // {
                //     ++lastBooleanIndex;
                // }
                // else
                // {
                //     lastBooleanIndex = 0;
                // }
            }
            
            collection.Insert(lastBooleanIndex, new BooleanCondition(condition));
        }
    }

    public void RevokeCondition(ConditionEnum condition) => collection.RemoveAll(c => c.condition == condition);
}
