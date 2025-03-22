using System;
using System.Collections.Generic;
using System.Linq;

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
            // UnityEngine.Debug.Log($"BinaryConditions 1 Conditions: {conditions != null}");

            if (conditions == null)
            {
                // UnityEngine.Debug.Log($"BinaryConditions 2 new");
                conditions = new List<BinaryCondition>();
            }

            // UnityEngine.Debug.Log($"BinaryConditions 3 Conditions: {conditions}");

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
