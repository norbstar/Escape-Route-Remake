#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

using Tests.States;

[CustomEditor(typeof(State))]
public abstract class StateEditor : Editor
{
    private State state;
    private bool showExclusionRules;

    private void AddBinaryDropdown()
    {
        var rect = EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        state.BinaryCollection.Enum = (State.BinaryCondition.BinaryEnum) EditorGUILayout.EnumPopup("Binaries", state.BinaryCollection.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(95)))
        {
            state.BinaryCollection.AddCondition(state.BinaryCollection.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddBinaryConfiguration()
    {
        foreach (var condition in state.BinaryCollection.Conditions)
        {
            EditorGUILayout.BeginHorizontal("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(122));

            condition.boolean = EditorGUILayout.Toggle(condition.boolean);

            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                state.BinaryCollection.RevokeCondition(condition.Enum);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void AddPropertyDropdown()
    {
        var rect = EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        state.PropertyCollection.Enum = (State.PropertyCondition.PropertyEnum) EditorGUILayout.EnumPopup("Properties", state.PropertyCollection.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(95)))
        {
            state.PropertyCollection.AddCondition(state.PropertyCollection.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void EmbedVelocityConfiguration(State.VelocityCondition condition)
    {
        if (condition.xAxis == null)
        {
            condition.xAxis = new State.VelocityCondition.AxisValue();
        }

        condition.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", condition.xAxis.include);

        if (condition.xAxis.include)
        {
            condition.xAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.xAxis.isNonZero);

            if (condition.xAxis.isNonZero)
            {
                condition.xAxis.sign = (State.VelocityCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.xAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        if (condition.yAxis == null)
        {
            condition.yAxis = new State.VelocityCondition.AxisValue();
        }

        condition.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", condition.yAxis.include);

        if (condition.yAxis.include)
        {
            condition.yAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.yAxis.isNonZero);

            if (condition.yAxis.isNonZero)
            {
                condition.yAxis.sign = (State.VelocityCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.yAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void AddPropertyConfiguration()
    {
        foreach (var condition in state.PropertyCollection.Conditions)
        {
            EditorGUILayout.BeginVertical("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(110));

            switch (condition.Enum)
            {
                case State.PropertyCondition.PropertyEnum.Velocity:
                    EmbedVelocityConfiguration((State.VelocityCondition) condition);
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
        
        EditorGUILayout.Space();
        
        showExclusionRules = EditorGUILayout.Foldout(showExclusionRules, "Exclusion Rules");

        if (!showExclusionRules) return;

        EditorGUILayout.Space();

        AddBinaryDropdown();
        AddBinaryConfiguration();
        
        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();

        AddPropertyDropdown();
        AddPropertyConfiguration();

        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 75;
        // FontStyle originalFontStyle = EditorStyles.label.fontStyle;
        // EditorStyles.label.fontStyle = FontStyle.Bold;
        // EditorGUI.BeginDisabledGroup(true);
        GUI.enabled = false;
        var defaultColor = GUI.color;
        GUI.color = state.CanExecute ? Color.green : Color.red;
        var status = state.CanExecute ? "Active" : "Inactive";
        EditorGUILayout.TextField("Status", status, GUILayout.Width(210));
        GUI.color = defaultColor;
        GUI.enabled = true;
        // EditorGUI.EndDisabledGroup();
        // EditorStyles.label.fontStyle = originalFontStyle;
        EditorGUIUtility.labelWidth = originalValue;
    }
}
#endif
