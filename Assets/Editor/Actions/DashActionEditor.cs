#if (UNITY_EDITOR) 
using Tests.Actions;

using UnityEditor;

[CustomEditor(typeof(DashAction))]
public class DashActionEditor : ActionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
