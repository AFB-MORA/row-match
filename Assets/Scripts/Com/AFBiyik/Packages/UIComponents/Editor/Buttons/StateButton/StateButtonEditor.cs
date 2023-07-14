using UnityEditor;

namespace Com.AFBiyik.UIComponents
{
    [CustomEditor(typeof(StateButton))]
    [CanEditMultipleObjects]
    public class StateButtonEditor : UIButtonEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("disableDefaultButtonStates"));
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();

            StateButton button = (StateButton)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("stateType"));

            if (serializedObject.FindProperty("stateType").intValue == (int)ChangeableButtonType.None)
            {
                if (button.ChangeableButton != null)
                {
                    button.ChangeableButton = null;
                    EditorUtility.SetDirty(button);
                }
            }
            else if (serializedObject.FindProperty("stateType").intValue == (int)ChangeableButtonType.Sprite)
            {
                if (button.ChangeableButton is ChangeableButtonSprite changeableButtonSprite)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("changeableButton"));
                }
                else
                {
                    button.ChangeableButton = new ChangeableButtonSprite();
                    button.ChangeableButton.selectable = button;
                    EditorUtility.SetDirty(button);
                }
            }
            else if (serializedObject.FindProperty("stateType").intValue == (int)ChangeableButtonType.GameObject)
            {
                if (button.ChangeableButton is ChangeableButtonGameObject)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("changeableButton"));
                }
                else
                {
                    button.ChangeableButton = new ChangeableButtonGameObject();
                    button.ChangeableButton.selectable = button;
                    EditorUtility.SetDirty(button);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
