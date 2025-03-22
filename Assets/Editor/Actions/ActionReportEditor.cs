#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

using Tests.Actions;

[CustomEditor(typeof(ActionReport))]
public class ActionReportEditor : Editor
{
    private ActionReport actionReport;

#if false
    private void AddLine(int height = 1)
    {
        var rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    private void AddExecutingIndicators()
    {
        EditorGUILayout.LabelField("Executing", EditorStyles.boldLabel);

        var defaultColor = GUI.color;
        GUI.color = Color.green;

        foreach (var action in actionReport.Actions)
        {
            if (action.CanAction)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField(action.GetType().Name);
                EditorGUILayout.EndVertical();
            }
        }

        GUI.color = defaultColor;
    }

    private void AddNonExecutingIndicators()
    {
        EditorGUILayout.LabelField("Non Executing", EditorStyles.boldLabel);

        var defaultColor = GUI.color;
        GUI.color = Color.red;

        foreach (var action in actionReport.Actions)
        {
            if (!action.CanAction)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField(action.GetType().Name);
                EditorGUILayout.EndVertical();
            }
        }

        GUI.color = defaultColor;
    }
#endif

    private void AddReport()
    {
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 150;

        foreach (var action in actionReport.Actions)
        {
            EditorGUILayout.BeginVertical("Box");
            GUI.enabled = false;
            var defaultColor = GUI.color;
            GUI.color = action.CanAction ? Color.green : Color.red;
            var status = action.CanAction ? "Active" : "Inactive";
            EditorGUILayout.TextField(action.GetType().Name, status, GUILayout.Width(210));
            GUI.color = defaultColor;
            GUI.enabled = true;
            EditorGUILayout.EndVertical();
        }

        EditorGUIUtility.labelWidth = originalValue;
    }

    public override void OnInspectorGUI()
    {
        actionReport = (ActionReport) target;

        if (actionReport.Actions == null) return;

        // AddExecutingIndicators();

        // EditorGUILayout.Space();
        // AddLine();
        // EditorGUILayout.Space();

        // AddNonExecutingIndicators();
        AddReport();
    }
}
#endif