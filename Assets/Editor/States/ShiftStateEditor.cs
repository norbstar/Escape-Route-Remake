#if (UNITY_EDITOR) 
using Tests.States;

using UnityEditor;

[CustomEditor(typeof(ShiftState))]
public class ShiftStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        RenderUI();
        base.OnInspectorGUI();
    }
}
#endif
