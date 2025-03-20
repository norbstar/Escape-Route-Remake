#if (UNITY_EDITOR) 
using Tests.Actions;

using UnityEditor;

[CustomEditor(typeof(JumpAction))]
public class JumpActionEditor : ActionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
