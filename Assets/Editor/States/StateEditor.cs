#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

using Tests.States;

[CustomEditor(typeof(State))]
public abstract class StateEditor : Editor
{
    private State state;
    private bool showExecutionRules;

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

            condition.boolean = EditorGUILayout.Toggle(condition.boolean);

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
        state.PropertyCollection.Enum = (State.PropertyCondition.InputEnum) EditorGUILayout.EnumPopup("Inputs", state.PropertyCollection.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(95)))
        {
            state.PropertyCollection.AddCondition(state.PropertyCollection.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void EmbedMoveConfiguration(State.MoveCondition condition)
    {
        if (condition.xAxis == null)
        {
            condition.xAxis = new State.MoveCondition.MoveAxisValue();
        }

        condition.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", condition.xAxis.include);

        if (condition.xAxis.include)
        {
            condition.xAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.xAxis.isNonZero);

            if (condition.xAxis.isNonZero)
            {
                condition.xAxis.sign = (State.MoveCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.xAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        if (condition.yAxis == null)
        {
            condition.yAxis = new State.MoveCondition.MoveAxisValue();
        }

        condition.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", condition.yAxis.include);

        if (condition.yAxis.include)
        {
            condition.yAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.yAxis.isNonZero);

            if (condition.yAxis.isNonZero)
            {
                condition.yAxis.sign = (State.MoveCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.yAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void AddInputConfiguration()
    {
        foreach (var condition in state.PropertyCollection.Conditions)
        {
            EditorGUILayout.BeginVertical("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(110));

            switch (condition.Enum)
            {
                case State.PropertyCondition.InputEnum.Move:
                    EmbedMoveConfiguration((State.MoveCondition) condition);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                state.PropertyCollection.RevokeCondition(condition.Enum);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }

    private void AddLine(int height = 1)
    {
        var rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    // private bool CanExecute() => state.Essentials != null ? state.TestConditions() : false;

    protected void RenderUI()
    {
        state = (State) target;
        showExecutionRules = EditorGUILayout.Foldout(showExecutionRules, "Execution Rules");

        if (!showExecutionRules) return;

        AddStateDropdown();
        AddStateConfiguration();
        
        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();

        AddInputDropdown();
        AddInputConfiguration();

        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 75;
        // FontStyle originalFontStyle = EditorStyles.label.fontStyle;
        // EditorStyles.label.fontStyle = FontStyle.Bold;
        // EditorGUI.BeginDisabledGroup(true);
        GUI.enabled = false;
        EditorGUILayout.TextField("Execute", state.CanExecute.ToString(), GUILayout.Width(210));
        // EditorGUI.EndDisabledGroup();
        GUI.enabled = true;
        // EditorStyles.label.fontStyle = originalFontStyle;
        EditorGUIUtility.labelWidth = originalValue;

        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();
    }
}
#endif
