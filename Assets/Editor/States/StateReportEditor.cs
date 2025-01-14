#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

using Tests.States;

[CustomEditor(typeof(StateReport))]
public class StateReportEditor : Editor
{
    private StateReport stateReport;

    private void AddExecutingIndicators()
    {
        EditorGUILayout.LabelField("Executing", EditorStyles.boldLabel);

        var defaultColor = GUI.color;
        GUI.color = Color.green;

        foreach (var state in stateReport.States)
        {
            if (state.CanExecute)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField(state.GetType().Name);
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

        foreach (var state in stateReport.States)
        {
            if (!state.CanExecute)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField(state.GetType().Name);
                EditorGUILayout.EndVertical();
            }
        }

        GUI.color = defaultColor;
    }

    private void AddExecutionIndicators()
    {
        EditorGUILayout.LabelField("Execution States", EditorStyles.boldLabel);

        foreach (var state in stateReport.States)
        {
            EditorGUILayout.BeginVertical("Box");
            GUI.enabled = false;
            var defaultColor = GUI.color;
            GUI.color = state.CanExecute ? Color.green : Color.red;
            EditorGUILayout.TextField(state.GetType().Name, state.CanExecute.ToString(), GUILayout.Width(210));
            GUI.color = defaultColor;
            GUI.enabled = true;
            EditorGUILayout.EndVertical();
        }
    }

    private void AddLine(int height = 1)
    {
        var rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    public override void OnInspectorGUI()
    {
        stateReport = (StateReport) target;

        if (stateReport.States == null) return;

        // AddExecutingIndicators();

        // EditorGUILayout.Space();
        // AddLine();
        // EditorGUILayout.Space();

        // AddNonExecutingIndicators();
        AddExecutionIndicators();
    }
}
#endif
