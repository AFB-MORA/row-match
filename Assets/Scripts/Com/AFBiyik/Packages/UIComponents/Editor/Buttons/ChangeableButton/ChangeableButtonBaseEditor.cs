using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    [CustomPropertyDrawer(typeof(ChangeableButtonBase))]
    public class ChangeableButtonBaseEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
        }
    }
}
