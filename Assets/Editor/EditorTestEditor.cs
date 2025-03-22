#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

using Tests;

[CustomEditor(typeof(EditorTest))]
public class EditorTestEditor : Editor
{
    private EditorTest editorTest;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }

    protected void RenderUI()
    {
        editorTest = (EditorTest) target;

        EditorGUILayout.Space();

        EditorGUILayout.TextField("Custom Editor", EditorStyles.boldLabel, GUILayout.Width(210));
        
        EditorGUILayout.Space();

        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 150f;

        editorTest.FloatValue = float.Parse(EditorGUILayout.TextField("Float Value", $"{editorTest.FloatValue}".ToString(), GUILayout.Width(250)));
        editorTest.EnumValue = (EnumCondition.FooEnum) EditorGUILayout.EnumPopup("Enum Value", editorTest.EnumValue, GUILayout.Width(250));

        EditorGUILayout.Space();

        editorTest.EnumCondition.FloatValue = float.Parse(EditorGUILayout.TextField("Instance Float Value", $"{editorTest.EnumCondition.FloatValue}".ToString(), GUILayout.Width(250)));
        editorTest.EnumCondition.EnumValue = (EnumCondition.FooEnum) EditorGUILayout.EnumPopup("Instance Enum Value", editorTest.EnumCondition.EnumValue, GUILayout.Width(250));
    
        EditorGUIUtility.labelWidth = originalValue;
    }
}
#endif