using System;

[Serializable]
public class EnumCondition
{
    public enum FooEnum
    {
        Alpha,
        Beta,
        Delta
    }

    public float floatValue;
    public FooEnum enumValue;
    
    public float FloatValue { get => floatValue; set => floatValue = value; }
    
    public FooEnum EnumValue { get => enumValue; set => enumValue = value; }

}
