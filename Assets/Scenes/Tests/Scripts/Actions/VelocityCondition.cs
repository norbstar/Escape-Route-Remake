using System;

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
