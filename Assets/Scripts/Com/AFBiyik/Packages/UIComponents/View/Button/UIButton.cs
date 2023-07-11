using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Simple button
    /// </summary>
    public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        // Serialize Fields
        [SerializeField]
        protected bool interactable = true;
        [SerializeField]
        protected SpriteRenderer image;
        [SerializeField]
        protected Color pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1f);
        [SerializeField]
        protected Color normalColor = Color.white;
        [SerializeField]
        protected Color disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);

        /// <summary>
        /// Called when button clicked
        /// </summary>
        public UnityEvent onClick;

        public bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
                DisabledStateChanged();
            }
        }

        /// <summary>
        /// Validates serialized fields
        /// </summary>
        protected virtual void OnValidate()
        {
            DisabledStateChanged();
        }

        /// <summary>
        /// Updates disabled state
        /// </summary>
        protected virtual void DisabledStateChanged()
        {
            if (image == null)
            {
                return;
            }

            image.color = interactable ? normalColor : disabledColor;
        }

        /// <summary>
        /// Called when clicked
        /// </summary>
        /// <param name="pointerEventData"></param>
        public virtual void OnPointerClick(PointerEventData pointerEventData)
        {
            if (!interactable)
            {
                return;
            }

            onClick?.Invoke();
        }

        /// <summary>
        /// Called when pointer down
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }

            if (image == null)
            {
                return;
            }

            image.color = pressedColor;
        }

        /// <summary>
        /// Called when pointer up
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }

            if (image == null)
            {
                return;
            }

            image.color = normalColor;
        }
    }
}

