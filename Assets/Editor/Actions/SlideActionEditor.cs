#if (UNITY_EDITOR) 
using Tests.Actions;

using UnityEditor;

[CustomEditor(typeof(SlideAction))]
public class SlideActionEditor : ActionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RenderUI();
    }
}
#endif
