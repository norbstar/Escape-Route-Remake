#if (UNITY_EDITOR) 
using Tests.States;

using UnityEditor;

[CustomEditor(typeof(SneakState))]
public class SneakStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
