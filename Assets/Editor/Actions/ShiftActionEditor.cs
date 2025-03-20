#if (UNITY_EDITOR) 
using Tests.Actions;

using UnityEditor;

[CustomEditor(typeof(ShiftAction))]
public class ShiftActionEditor : ActionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
