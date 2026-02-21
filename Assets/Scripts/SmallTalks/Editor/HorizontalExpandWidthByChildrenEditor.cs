using SmallTalks.UI.Tools;
using UnityEditor;
using UnityEngine;

namespace SmallTalks.Editor
{
    [CustomEditor(typeof(HorizontalExpandWidthByChildren))]
    public sealed class HorizontalExpandWidthByChildrenEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var component = (HorizontalExpandWidthByChildren)target;
            if (GUILayout.Button("Force Update Width"))
                component.UpdateWidth();
        }
    }
}