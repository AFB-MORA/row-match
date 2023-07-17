using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents.View
{
    /// <summary>
    /// Fits sprite renderer into recttransform.
    /// Respecting aspect ratio.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class FitSpriteRenderer : UIBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            SetSize();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            SetSize();
        }

        /// <summary>
        /// Sets sprite renderer size
        /// </summary>
        private void SetSize()
        {
            // Get sprite renderer
            var spriteRenderer = GetComponent<SpriteRenderer>();
            // Get recttransform
            var rectTransform = (RectTransform)transform;

            // Get sprite size
            var spriteRect = spriteRenderer.sprite.rect;
            float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
            Vector2 spriteSize = spriteRect.size / pixelsPerUnit;

            // Get transform size
            Vector2 transformSize = rectTransform.rect.size;

            // Calculate ratio
            float ratio = transformSize.x / spriteSize.x;
            // Calculate width
            float width = spriteSize.x * ratio;
            // Calculate height
            float height = spriteSize.y * ratio;

            // If not fit, fit vertical
            if (height > transformSize.y)
            {
                // Calculate ration
                ratio = transformSize.y / spriteSize.y;
                // Calculate width
                width = spriteSize.x * ratio;
                // Calculate height
                height = spriteSize.y * ratio;
            }

            // Hack spriteRenderer.size changes scale
            rectTransform.localScale = new Vector3(1, 1, 1);

            // Set size
            spriteRenderer.size = new Vector2(width, height);

            // Hack spriteRenderer.size changes scale
            rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }
}