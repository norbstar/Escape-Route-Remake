#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

using Tests.States;

[CustomEditor(typeof(StateReport))]
public class StateReportEditor : Editor
{
    private StateReport stateReport;

    private void AddActiveList()
    {
        EditorGUILayout.LabelField("Active States", EditorStyles.boldLabel);

        foreach (var state in stateReport.States)
        {
            if (state.CanExecute)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField(state.GetType().ToString());
                EditorGUILayout.EndVertical();
            }
        }
    }

    private void AddInactiveList()
    {
        EditorGUILayout.LabelField("Inactive States", EditorStyles.boldLabel);

        foreach (var state in stateReport.States)
        {
            if (!state.CanExecute)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField(state.GetType().ToString());
                EditorGUILayout.EndVertical();
            }
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

        AddActiveList();

        EditorGUILayout.Space();
        AddLine();
        EditorGUILayout.Space();

        AddInactiveList();
    }
}
#endif
