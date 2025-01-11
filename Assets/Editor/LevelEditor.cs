#if (UNITY_EDITOR) 
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var level = (Level) target;
        level.experience = EditorGUILayout.IntField("Experience", level.experience);
        EditorGUILayout.LabelField("Level", level.ExperienceLevel.ToString());
    }
}
#endif
