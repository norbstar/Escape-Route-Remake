#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

using Tests.Actions;

[CustomEditor(typeof(Action))]
public abstract class ActionEditor : Editor
{
    private Action action;
    private bool showPrerequisites;

    private void AddBinaryDropdown()
    {
        EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        action.BinaryCollection.Enum = (Action.BinaryCondition.BinaryEnum) EditorGUILayout.EnumPopup("Binaries", action.BinaryCollection.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(95)))
        {
            action.BinaryCollection.AddCondition(action.BinaryCollection.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddBinaryConfiguration()
    {
        foreach (var condition in action.BinaryCollection.Conditions)
        {
            EditorGUILayout.BeginHorizontal("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(122));

            condition.boolean = EditorGUILayout.Toggle(condition.boolean);

            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                action.BinaryCollection.RevokeCondition(condition.Enum);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void AddPropertyDropdown()
    {
        var rect = EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        action.PropertyCollection.Enum = (Action.PropertyCondition.PropertyEnum) EditorGUILayout.EnumPopup("Properties", action.PropertyCollection.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add Condition", EditorStyles.miniButtonLeft, GUILayout.Width(95)))
        {
            action.PropertyCollection.AddCondition(action.PropertyCollection.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void EmbedVelocityConfiguration(Action.VelocityCondition condition)
    {
        if (condition.xAxis == null)
        {
            condition.xAxis = new Action.VelocityCondition.AxisValue();
        }

        condition.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", condition.xAxis.include);

        if (condition.xAxis.include)
        {
            condition.xAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.xAxis.isNonZero);

            if (condition.xAxis.isNonZero)
            {
                condition.xAxis.sign = (Action.VelocityCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.xAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        if (condition.yAxis == null)
        {
            condition.yAxis = new Action.VelocityCondition.AxisValue();
        }

        condition.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", condition.yAxis.include);

        if (condition.yAxis.include)
        {
            condition.yAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.yAxis.isNonZero);

            if (condition.yAxis.isNonZero)
            {
                condition.yAxis.sign = (Action.VelocityCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.yAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void AddPropertyConfiguration()
    {
        foreach (var condition in action.PropertyCollection.Conditions)
        {
            EditorGUILayout.BeginVertical("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(110));

            switch (condition.Enum)
            {
                case Action.PropertyCondition.PropertyEnum.Velocity:
                    EmbedVelocityConfiguration((Action.VelocityCondition) condition);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                action.PropertyCollection.RevokeCondition(condition.Enum);
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
        action = (Action) target;
        
        EditorGUILayout.Space();
        
        showPrerequisites = EditorGUILayout.Foldout(showPrerequisites, "Prerequisites");

        if (!showPrerequisites) return;

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
        GUI.color = action.CanExecute ? Color.green : Color.red;
        var status = action.CanExecute ? "Active" : "Inactive";
        EditorGUILayout.TextField("Status", status, GUILayout.Width(210));
        GUI.color = defaultColor;
        GUI.enabled = true;
        // EditorGUI.EndDisabledGroup();
        // EditorStyles.label.fontStyle = originalFontStyle;
        EditorGUIUtility.labelWidth = originalValue;
    }
}
#endif
