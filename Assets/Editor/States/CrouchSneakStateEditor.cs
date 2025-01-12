#if (UNITY_EDITOR) 
using Tests.States;

using UnityEditor;

[CustomEditor(typeof(CrouchSneakState))]
public class CrouchSneakStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        RenderUI();
        base.OnInspectorGUI();
    }
}
#endif
