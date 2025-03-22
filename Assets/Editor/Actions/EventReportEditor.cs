#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

using Tests.Events;

[CustomEditor(typeof(EventReport))]
public class EventReportEditor : Editor
{
    private EventReport eventReport;

    private void AddReport()
    {
        EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 150;

        foreach (var action in eventReport.Events)
        {
            EditorGUILayout.BeginVertical("Box");
            GUI.enabled = false;
            var defaultColor = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.TextField(action.GetType().Name, "Active", GUILayout.Width(210));
            GUI.color = defaultColor;
            GUI.enabled = true;
            EditorGUILayout.EndVertical();
        }

        EditorGUIUtility.labelWidth = originalValue;
    }

    public override void OnInspectorGUI()
    {
        eventReport = (EventReport) target;

        if (eventReport.Events == null) return;

        AddReport();
    }
}
#endif