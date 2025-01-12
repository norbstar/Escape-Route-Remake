#if (UNITY_EDITOR) 
using Tests.States;

using UnityEditor;

[CustomEditor(typeof(GrabState))]
public class GrabStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        RenderUI();
        base.OnInspectorGUI();
    }
}
#endif
