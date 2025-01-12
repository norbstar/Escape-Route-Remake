#if (UNITY_EDITOR) 
using Tests.States;

using UnityEditor;

[CustomEditor(typeof(JumpState))]
public class JumpStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        RenderUI();
        base.OnInspectorGUI();
    }
}
#endif
