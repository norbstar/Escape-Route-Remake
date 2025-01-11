#if (UNITY_EDITOR) 
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using Tests.States;

[CustomEditor(typeof(State))]
public abstract class StateEditor : Editor
{
    private State state;

    private void AddStateDropdown()
    {
        var rect = EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        state.StateCollection.Enum = (State.StateCondition.StateEnum) EditorGUILayout.EnumPopup("States", state.StateCollection.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(95)))
        {
            state.StateCollection.AddCondition(state.StateCollection.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddStateConfiguration()
    {
        foreach (var condition in state.StateCollection.Conditions)
        {
            EditorGUILayout.BeginHorizontal("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(122));

            condition.trueFalse = EditorGUILayout.Toggle(condition.trueFalse);

            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                state.StateCollection.RevokeCondition(condition.Enum);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void AddInputDropdown()
    {
        var rect = EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        state.InputCollection.Enum = (State.InputCondition.InputEnum) EditorGUILayout.EnumPopup("Inputs", state.InputCollection.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(95)))
        {
            state.InputCollection.AddCondition(state.InputCollection.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void EmbedMoveConfiguration(State.InputCondition condition)
    {
        if (condition.xAxis == null)
        {
            condition.xAxis = new State.InputCondition.MoveAxisValue();
        }

        condition.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", condition.xAxis.include);

        if (condition.xAxis.include)
        {
            condition.xAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.xAxis.isNonZero);

            if (condition.xAxis.isNonZero)
            {
                condition.xAxis.sign = (State.InputCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.xAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        if (condition.yAxis == null)
        {
            condition.yAxis = new State.InputCondition.MoveAxisValue();
        }

        condition.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", condition.yAxis.include);

        if (condition.yAxis.include)
        {
            condition.yAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.yAxis.isNonZero);

            if (condition.yAxis.isNonZero)
            {
                condition.yAxis.sign = (State.InputCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.yAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void AddInputConfiguration()
    {
        foreach (var condition in state.InputCollection.Conditions)
        {
            EditorGUILayout.BeginVertical("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(110));

            if (condition.Enum == State.InputCondition.InputEnum.Move)
            {
                EmbedMoveConfiguration(condition);
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                state.InputCollection.RevokeCondition(condition.Enum);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }

    void AddLine(int height = 1)
    {
        var rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    protected void RenderUI()
    {
        state = (State) target;

        EditorGUILayout.LabelField("Conditionality", EditorStyles.boldLabel);

        AddStateDropdown();

        if (state.StateCollection.Conditions == null)
        {
            state.StateCollection.Conditions = new List<State.StateCondition>();
        }

        AddStateConfiguration();
        
        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();
        AddInputDropdown();

        if (state.InputCollection.Conditions == null)
        {
            state.InputCollection.Conditions = new List<State.InputCondition>();
        }
        
        AddInputConfiguration();

        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();
    }
}
#endif