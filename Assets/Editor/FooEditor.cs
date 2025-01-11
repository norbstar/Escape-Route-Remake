#if (UNITY_EDITOR) 
using UnityEditor;

[CustomEditor(typeof(Foo))]
public class FooEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var foo = (Foo) target;
        foo.value = EditorGUILayout.IntField("Value", foo.Value);
        EditorGUILayout.LabelField("Foo", foo.Value.ToString());
    }
}
#endif
