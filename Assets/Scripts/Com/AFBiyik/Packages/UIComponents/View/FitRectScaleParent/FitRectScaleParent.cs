using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents.View
{
    /// <summary>
    /// Fits sprite renderer into recttransform.
    /// Respecting aspect ratio.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class FitRectScaleParent : UIBehaviour
    {
        // Private Fields
        private FitRectScaleParentChangeEvent fitRectScaleParentChangeEvent;

        protected override void Awake()
        {
            base.Awake();


            if (transform.parent == null || transform.parent is not RectTransform)
            {
                return;
            }

            fitRectScaleParentChangeEvent = transform.parent.gameObject.AddComponent<FitRectScaleParentChangeEvent>();
            fitRectScaleParentChangeEvent.onRectChange.AddListener(SetSize);

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
            if (fitRectScaleParentChangeEvent == null)
            {
                return;
            }

            // Get rect transform
            RectTransform rectTransform = (RectTransform)transform;

            // Get parent rect transform
            RectTransform parentTransform = (RectTransform)rectTransform.parent;

            // Get sprite size
            Vector2 selfSize = rectTransform.rect.size;

            // Get transform size
            Vector2 parentSize = parentTransform.rect.size;

            // Calculate ratio
            float ratio = parentSize.x / selfSize.x;
            // Calculate height
            float height = selfSize.y * ratio;

            // If not fit, fit vertical
            if (height > parentSize.y)
            {
                // Calculate ration
                ratio = parentSize.y / selfSize.y;
            }

            // Set scale
            rectTransform.localScale = new Vector3(ratio, ratio, 1);
        }
    }
}