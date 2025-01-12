#if (UNITY_EDITOR) 
using Tests.States;

using UnityEditor;

[CustomEditor(typeof(TraverseState))]
public class TraverseStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        RenderUI();
        base.OnInspectorGUI();
    }
}
#endif
