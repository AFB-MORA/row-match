using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    [CustomPropertyDrawer(typeof(ChangeableButtonSprite))]
    public class ChangeableButtonSpriteEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            // Draw label
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.indentLevel += 1;

            var height = EditorGUIUtility.singleLineHeight;
            var width = position.width;

            var p0 = new Rect(position.x, position.y, position.width, position.height);

            var p1 = new Rect(p0.x, p0.y + height, width, height);
            EditorGUI.PropertyField(p1, property.FindPropertyRelative("normal"));

            var p2 = new Rect(p0.x, p1.y + height, width, height);
            EditorGUI.PropertyField(p2, property.FindPropertyRelative("highlighted"));

            var p3 = new Rect(p0.x, p2.y + height, width, height);
            EditorGUI.PropertyField(p3, property.FindPropertyRelative("pressed"));

            var p4 = new Rect(p0.x, p3.y + height, width, height);
            EditorGUI.PropertyField(p4, property.FindPropertyRelative("selected"));

            var p5 = new Rect(p0.x, p4.y + height, width, height);
            EditorGUI.PropertyField(p5, property.FindPropertyRelative("disabled"));

            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 6;
        }
    }
}
