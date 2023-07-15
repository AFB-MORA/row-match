using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Mask area for recycle view.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(SpriteMask))]
    public class RecycleViewPort : UIBehaviour
    {
        // Serialize Fields
        [SerializeField]
        protected int pixelsPerUnit = 100;

        // Private Fields
        private RectTransform rectTransform;
        private SpriteMask spriteMask;

        // Protected Properties
        protected RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }

                return rectTransform;
            }

        }

        protected SpriteMask SpriteMask {
            get
            {
                if (spriteMask == null)
                {
                    spriteMask = GetComponent<SpriteMask>();
                }

                return spriteMask;
            }
        }

        /// <summary>
        /// Called first
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            SetBounds();
        }

        /// <summary>
        /// Called when rect transform changes
        /// </summary>
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            SetBounds();
        }

        /// <summary>
        /// Sets bounds for sprite renderer and collider
        /// </summary>
        private void SetBounds()
        {
            var size = RectTransform.rect.size * pixelsPerUnit;
            Texture2D texture = new Texture2D((int)size.x, (int)size.y);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
            sprite.name = "Generated Sprite";
            SpriteMask.sprite = sprite;
        }
    }
}