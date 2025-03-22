using System;
using System.Collections.Generic;
using System.Linq;

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
