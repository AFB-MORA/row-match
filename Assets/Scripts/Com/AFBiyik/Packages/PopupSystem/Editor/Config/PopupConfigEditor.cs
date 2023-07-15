using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Com.AFBiyik.PopupSystem.Editor
{
    [CustomEditor(typeof(PopupConfig))]
    public class PopupConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            PopupConfig popupConfig = (PopupConfig)target;

            var popupCanvasProperty = serializedObject.FindProperty("popupCanvas");

            EditorGUILayout.PropertyField(popupCanvasProperty);


            if (GUILayout.Button("Generate Popup Canvas"))
            {
                popupCanvasProperty.objectReferenceValue = CreatePopupCanvas();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public virtual GameObject CreatePopupCanvas()
        {
            if (!AssetDatabase.IsValidFolder("Assets/PopupSystem"))
            {
                AssetDatabase.CreateFolder("Assets", "PopupSystem");
            }

            string path = "Assets/PopupSystem/PopupCanvas.prefab";

            if (!File.Exists(path))
            {
                GameObject go = new GameObject();
                var canvas = go.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 100;
                var scaler = go.AddComponent<CanvasScaler>();
                var raycaster = go.AddComponent<GraphicRaycaster>();

                PrefabUtility.SaveAsPrefabAsset(go, path);

                DestroyImmediate(go);
            }

            var popupCanvas = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return popupCanvas;
        }
    }
}

