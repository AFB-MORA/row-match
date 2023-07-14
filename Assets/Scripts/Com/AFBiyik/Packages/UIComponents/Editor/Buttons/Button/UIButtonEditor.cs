using UnityEditor;

namespace Com.AFBiyik.UIComponents
{
    [CustomEditor(typeof(UIButton))]
    [CanEditMultipleObjects]
    public class UIButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("interactable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetGraphic"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("normalColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("highlightedColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pressedColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("disabledColor"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
