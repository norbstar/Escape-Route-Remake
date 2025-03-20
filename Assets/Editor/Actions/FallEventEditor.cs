#if (UNITY_EDITOR) 
using Tests.Events;

using UnityEditor;

[CustomEditor(typeof(FallEvent))]
public class FallEventEditor : ActionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
