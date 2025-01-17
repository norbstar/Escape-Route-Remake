#if (UNITY_EDITOR) 
using Tests.States;

using UnityEditor;

[CustomEditor(typeof(DashState))]
public class DashStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
