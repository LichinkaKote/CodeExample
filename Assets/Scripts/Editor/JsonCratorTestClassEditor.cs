using Assets.Scripts.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(JSONCreatorTestClass))]
    public class JsonCratorTestClassEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var test = (JSONCreatorTestClass)target;
            if (GUILayout.Button("Create"))
            {
                test.Create();
            }
        }
    }
}