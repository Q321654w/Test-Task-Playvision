using UnityEditor;
using UnityEngine;

namespace DiceThrowers.Editor
{
    [CustomEditor(typeof(DiceThrower))]
    public class DiceThrowerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var diceThrower = (DiceThrower)target;
            
            GUILayout.Space(15);
            diceThrower.UseGivenNumbers = EditorGUILayout.Toggle("Use given numbers", diceThrower.UseGivenNumbers);
                             
            var stringListProperty = serializedObject.FindProperty(nameof(diceThrower.GivenNumbers));
            if (diceThrower.UseGivenNumbers)
            {
                EditorGUILayout.PropertyField(stringListProperty);
                serializedObject.ApplyModifiedProperties();
            }

            
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            GUILayout.Space(10);
            
            
            var shouldThrow = GUILayout.Button("Throw", GUILayout.Height(30));
            if (shouldThrow)
                diceThrower.ThrowDices();
            
            
            var shouldClearPrevious = GUILayout.Button("Clear previous", GUILayout.Height(30));
            if (shouldClearPrevious)
                diceThrower.ClearPreviousDices();
            
            
            var shouldClearAll = GUILayout.Button("Clear all", GUILayout.Height(30));
            if (shouldClearAll)
                diceThrower.ClearAllDices();
            
            EditorGUI.EndDisabledGroup();
        }
    }
}