#if (UNITY_EDITOR) 
using Tests.Actions;

using UnityEditor;

[CustomEditor(typeof(VariableJumpAction))]
public class VariableJumpActionEditor : ActionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
