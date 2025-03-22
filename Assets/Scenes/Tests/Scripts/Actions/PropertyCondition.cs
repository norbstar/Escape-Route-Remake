using System;

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
