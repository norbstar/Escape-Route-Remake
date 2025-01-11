#if (UNITY_EDITOR)
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Conditions))]
public class ConditionsEditor : Editor
{
    private Conditions conditions;

    // private void AddBox()
    // {
    //     EditorGUILayout.BeginHorizontal("Box");

    //     EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel, GUILayout.Width(110));

    //     GUILayout.FlexibleSpace();

    //     EditorGUILayout.EndHorizontal();
    // }

    private void AddBooleanCondition(Conditions.BooleanCondition condition)
    {
        EditorGUILayout.BeginHorizontal("Box");

        // var label = ((Conditions.ConditionEnum) (int) condition.condition).ToString();
        var label = condition.condition.ToString();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(110));

        // GUILayout.FlexibleSpace();
        condition.qualifyingState = EditorGUILayout.Toggle("Qualifying State", condition.qualifyingState, GUILayout.Width(110));

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void AddMoveCondition(Conditions.MoveCondition moveCondition)
    {
        EditorGUILayout.BeginVertical("Box");

        EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel, GUILayout.Width(110));
    
        if (moveCondition.xAxis == null)
        {
            moveCondition.xAxis = new Conditions.MoveAxisValue();
        }

        moveCondition.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", moveCondition.xAxis.include);

        if (moveCondition.xAxis.include)
        {
            moveCondition.xAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", moveCondition.xAxis.isNonZero);

            if (moveCondition.xAxis.isNonZero)
            {
                // moveCondition.xAxis.nonZeroSign = (Conditions.SignEnum) EditorGUILayout.EnumFlagsField("Non Zero Sign", moveCondition.xAxis.nonZeroSign);
                moveCondition.xAxis.sign = (Conditions.SignEnum) EditorGUILayout.EnumPopup("Sign", moveCondition.xAxis.sign);
            }
        }

        EditorGUILayout.EndToggleGroup();

        if (moveCondition.yAxis == null)
        {
            moveCondition.yAxis = new Conditions.MoveAxisValue();
        }

        moveCondition.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", moveCondition.yAxis.include);

        if (moveCondition.yAxis.include)
        {
            moveCondition.yAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", moveCondition.yAxis.isNonZero);

            if (moveCondition.yAxis.isNonZero)
            {
                // moveCondition.yAxis.nonZeroSign = (Conditions.SignEnum) EditorGUILayout.EnumFlagsField("Non Zero Sign", moveCondition.yAxis.nonZeroSign);
                moveCondition.yAxis.sign = (Conditions.SignEnum) EditorGUILayout.EnumPopup("Sign", moveCondition.yAxis.sign);
            }
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();
    }

    private void AddDropdown()
    {
        conditions.Enum = (Conditions.ConditionEnum) EditorGUILayout.EnumPopup("Conditions", conditions.Enum);
        
        var rect = EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(110)))
        {
            // Debug.Log($"Clicked {conditions.Enum}!");
            conditions.AddCondition(conditions.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddActiveConfiguration()
    {
        foreach (var condition in conditions.Collection)
        {
            if (condition.condition == Conditions.ConditionEnum.Move)
            {
                AddMoveCondition((Conditions.MoveCondition) condition);
            }
            else
            {
                AddBooleanCondition((Conditions.BooleanCondition) condition);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        conditions = (Conditions) target;

        EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);

        AddDropdown();

        // AddBox();

        if (conditions.Collection == null)
        {
            conditions.Collection = new List<Conditions.Condition>();
        }

        AddActiveConfiguration();
    }
}
#endif
