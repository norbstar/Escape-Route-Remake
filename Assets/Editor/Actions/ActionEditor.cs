#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

using Tests.Actions;

[CustomEditor(typeof(Action))]
public abstract class ActionEditor : Editor
{
    private Action action;
    private bool showPrerequisites = true;

    private void AddLine(int height = 1)
    {
        var rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    private void AddBinaryDropdown()
    {
        EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        action.BinaryConditions.Enum = (BinaryCondition.BinaryEnum) EditorGUILayout.EnumPopup("Binaries", action.BinaryConditions.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
        {
            action.BinaryConditions.AddCondition(action.BinaryConditions.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddBinaryConfiguration()
    {
        float originalFieldWidth = EditorGUIUtility.fieldWidth;
        float originalLabelWidth = EditorGUIUtility.labelWidth;

        foreach (var condition in action.BinaryConditions.Conditions)
        {
            EditorGUILayout.BeginHorizontal("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(160));

            EditorGUIUtility.fieldWidth = 0;
            condition.Boolean = EditorGUILayout.Toggle(condition.Boolean, GUILayout.Width(25));
            EditorGUIUtility.fieldWidth = originalFieldWidth;

            EditorGUIUtility.labelWidth = 0;
            GUI.enabled = false;
            var defaultColor = GUI.color;
            GUI.color = action.CanAction ? Color.green : Color.red;
            var status = action.CanAction ? "Active" : "Inactive";
            EditorGUILayout.TextField("", status, GUILayout.Width(60));
            GUI.color = defaultColor;
            GUI.enabled = true;
            EditorGUIUtility.labelWidth = originalLabelWidth;

            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                action.BinaryConditions.RevokeCondition(condition.Enum);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void AddPropertyDropdown()
    {
        EditorGUILayout.BeginHorizontal("Box");

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 70;
        action.PropertyConditions.Enum = (PropertyCondition.PropertyEnum) EditorGUILayout.EnumPopup("Properties", action.PropertyConditions.Enum, GUILayout.Width(210));
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
        {
            action.PropertyConditions.AddCondition(action.PropertyConditions.Enum);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void EmbedVelocityConfiguration(VelocityCondition condition)
    {
        if (condition.xAxis == null)
        {
            condition.xAxis = new VelocityCondition.AxisValue();
        }

        condition.xAxis.include = EditorGUILayout.BeginToggleGroup("X Axis", condition.xAxis.include);

        if (condition.xAxis.include)
        {
            condition.xAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.xAxis.isNonZero);

            if (condition.xAxis.isNonZero)
            {
                condition.xAxis.sign = (VelocityCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.xAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        if (condition.yAxis == null)
        {
            condition.yAxis = new VelocityCondition.AxisValue();
        }

        condition.yAxis.include = EditorGUILayout.BeginToggleGroup("Y Axis", condition.yAxis.include);

        if (condition.yAxis.include)
        {
            condition.yAxis.isNonZero = EditorGUILayout.Toggle("Is Non Zero", condition.yAxis.isNonZero);

            if (condition.yAxis.isNonZero)
            {
                condition.yAxis.sign = (VelocityCondition.SignEnum) EditorGUILayout.EnumPopup("Sign", condition.yAxis.sign, GUILayout.Width(225));
            }
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void AddPropertyConfiguration()
    {
        float originalFieldWidth = EditorGUIUtility.fieldWidth;
        float originalLabelWidth = EditorGUIUtility.labelWidth;

        foreach (var condition in action.PropertyConditions.Conditions)
        {
            EditorGUILayout.BeginVertical("Box");

            var label = condition.Enum.ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(100));

            switch (condition.Enum)
            {
                case PropertyCondition.PropertyEnum.Velocity:
                    EmbedVelocityConfiguration((VelocityCondition) condition);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
            {
                action.PropertyConditions.RevokeCondition(condition.Enum);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
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
        EditorGUIUtility.labelWidth = 50;
        
        FontStyle originalFontStyle = EditorStyles.label.fontStyle;
        EditorStyles.label.fontStyle = FontStyle.Bold;
        
        // EditorGUI.BeginDisabledGroup(true);
        GUI.enabled = false;
        var defaultColor = GUI.color;
        GUI.color = action.CanAction ? Color.green : Color.red;
        var status = action.CanAction ? "Active" : "Inactive";
        EditorGUILayout.TextField("Status", status, GUILayout.Width(110));
        GUI.color = defaultColor;
        GUI.enabled = true;
        // EditorGUI.EndDisabledGroup();
        
        EditorStyles.label.fontStyle = originalFontStyle;
        EditorGUIUtility.labelWidth = originalValue;
    }
}
#endif
