using System;

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

    private bool boolean;

    public bool Boolean { get => boolean; set => boolean = value; }
}
