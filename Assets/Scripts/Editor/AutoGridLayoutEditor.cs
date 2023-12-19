using Assets.Scripts.UI.Base;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(AutoGridLayout))]
    public class AutoGridLayoutEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var grid = (AutoGridLayout)target;
            if (GUILayout.Button("AutoSize"))
            {
                grid.AutoCellSize();
            }
        }
    }
}