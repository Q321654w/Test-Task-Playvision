using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DiceThrower))]
public class DiceThrowerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var test = (DiceThrower)target;

        var requestedThrowButtonRect = EditorGUILayout.GetControlRect();
        var throwButtonRect = new Rect(requestedThrowButtonRect.x, requestedThrowButtonRect.y, requestedThrowButtonRect.width, 25);
        var shouldThrow = GUI.Button(throwButtonRect, "Throw");
        
        if (shouldThrow)
            test.ThrowDices();
    }
}