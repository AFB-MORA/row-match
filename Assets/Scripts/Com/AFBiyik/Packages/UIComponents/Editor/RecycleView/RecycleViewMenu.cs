using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.UIComponents.Editor
{
    /// <summary>
    /// Creates recycle view game object in hierarchy.
    /// </summary>
    public class RecycleViewMenu
    {
        [MenuItem("GameObject/CustomUI/RecycleView")]
        private static void CreateButton()
        {
            // Get parent
            var parent = Selection.activeTransform;

            // Create recycle view
            var recycleViewGo = new GameObject("RecycleView");
            var recycleViewRenderer = recycleViewGo.AddComponent<SpriteRenderer>();
            recycleViewRenderer.enabled = false;
            recycleViewGo.layer = LayerMask.NameToLayer("UI");
            var recycleViewRT = recycleViewGo.AddComponent<RectTransform>();
            var recycleView = recycleViewGo.AddComponent<RecycleView>();
            recycleViewRT.SetParent(parent, false);
            recycleViewRT.localScale = new Vector3(1, 1, 1);
            recycleViewRT.sizeDelta = new Vector2(4, 4);
            recycleViewRT.localPosition = new Vector3(0, 0, 0);
            recycleViewRT.anchorMin = new Vector2(0.5f, 0.5f);
            recycleViewRT.anchorMax = new Vector2(0.5f, 0.5f);
            recycleViewRT.pivot = new Vector2(0.5f, 0.5f);

            // Create viewport
            var recycleViewPortGo = new GameObject("RecycleViewPort");
            recycleViewPortGo.layer = LayerMask.NameToLayer("UI");
            var recycleViewPortRT = recycleViewPortGo.AddComponent<RectTransform>();
            recycleViewPortGo.AddComponent<RecycleViewPort>();
            recycleViewPortRT.SetParent(recycleViewRT, false);
            recycleViewPortRT.localScale = new Vector3(1, 1, 1);
            recycleViewPortRT.localPosition = new Vector3(0, 0, 0);
            recycleViewPortRT.anchorMin = new Vector2(0, 0);
            recycleViewPortRT.anchorMax = new Vector2(1, 1);
            recycleViewPortRT.pivot = new Vector2(0.5f, 0.5f);
            recycleViewPortRT.offsetMin = new Vector2(0, 0);
            recycleViewPortRT.offsetMax = new Vector2(0, 0);

            // Create content
            var contentGo = new GameObject("Content");
            contentGo.layer = LayerMask.NameToLayer("UI");
            var contentRT = contentGo.AddComponent<RectTransform>();
            contentRT.SetParent(recycleViewPortRT, false);
            contentRT.localScale = new Vector3(1, 1, 1);
            contentRT.localPosition = new Vector3(0, 0, 0);
            contentRT.anchorMin = new Vector2(0, 1);
            contentRT.anchorMax = new Vector2(1, 1);
            contentRT.pivot = new Vector2(0.5f, 1);
            contentRT.offsetMin = new Vector2(0, 0);
            contentRT.offsetMax = new Vector2(0, 0);
            contentRT.sizeDelta = new Vector2(0, 4);

            // Set content
            var field = typeof(RecycleView).GetField("content", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(recycleView, contentRT);
            EditorUtility.SetDirty(recycleView);
        }
    }
}
