using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Stretch view for rect transform. Fills the screen.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class StretchView : BoundsView
    {
        protected virtual void Start()
        {
            // Set transform
            SetRectTransform();
        }

        protected override void Update()
        {
            if (currectAspect != Camera.main.aspect)
            {
                // Update transform.
                rect = GetRect();
                SetRectTransform();
                onBoundsChange?.Invoke();
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            // No need to draw gizmoz
        }
#endif

        protected virtual void SetRectTransform()
        {
            // Get rect transform
            var rt = GetComponent<RectTransform>();

            // Set size
            rt.sizeDelta = Rect.size;

            // Set position
            Vector3 offset = Rect.position;
            rt.localPosition = offset - rt.localPosition;

            // Set default anchors and scale
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localScale = new Vector3(1, 1, 1);

            // Grive rect transform i.e. hide fields
            DrivenRectTransformTracker dt = new DrivenRectTransformTracker();
            dt.Clear();

            // Drive the transform
            dt.Add(this, rt, DrivenTransformProperties.AnchoredPositionX |
                DrivenTransformProperties.AnchoredPositionY |
                DrivenTransformProperties.Rotation |
                DrivenTransformProperties.Scale |
                DrivenTransformProperties.Anchors |
                DrivenTransformProperties.Pivot |
                DrivenTransformProperties.SizeDelta);
        }
    }
}
