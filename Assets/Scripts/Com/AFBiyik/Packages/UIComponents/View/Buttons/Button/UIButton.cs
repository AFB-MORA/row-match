using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Simple button
    /// </summary>
    public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        // Serialize Fields
        [SerializeField]
        protected bool interactable = true;
        [SerializeField]
        protected SpriteRenderer targetGraphic;
        [SerializeField]
        protected Color normalColor = Color.white;
        [SerializeField]
        protected Color highlightedColor = Color.white;
        [SerializeField]
        protected Color pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1f);
        [SerializeField]
        protected Color selectedColor = Color.white;
        [SerializeField]
        protected Color disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);

        // Private Fields
        protected SelectionState currentState;

        // Public Fields
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
                DoStateTransition(interactable ? SelectionState.Normal : SelectionState.Disabled);
            }
        }

        public SpriteRenderer TargetGraphic
        {
            get => targetGraphic;
            set
            {
                targetGraphic = value;
                DoStateTransition(interactable ? currentState : SelectionState.Disabled);
            }
        }

        protected virtual void Start()
        {
            // For enabling
        }

#if UNITY_EDITOR
        /// <summary>
        /// Validates serialized fields
        /// </summary>
        /// 
        protected virtual void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += OnValidateCallback;
        }

        protected virtual void OnValidateCallback()
        {
            if (this == null)
            {
                UnityEditor.EditorApplication.delayCall -= OnValidateCallback;
                return; // MissingRefException if managed in the editor - uses the overloaded Unity == operator.
            }

            DoStateTransition(interactable ? SelectionState.Normal : SelectionState.Disabled);
        }
#endif

        /// <summary>
        /// Updates state of the button
        /// </summary>
        /// <param name="state">New state</param>
        protected virtual void DoStateTransition(SelectionState state)
        {
            currentState = state;

            if (targetGraphic == null)
            {
                return;
            }

            if (state == SelectionState.Normal)
            {
                targetGraphic.color = normalColor;
            }
            else if (state == SelectionState.Highlighted)
            {
                targetGraphic.color = highlightedColor;
            }
            else if (state == SelectionState.Pressed)
            {
                targetGraphic.color = pressedColor;
            }
            else if (state == SelectionState.Selected)
            {
                targetGraphic.color = selectedColor;
            }
            else if (state == SelectionState.Disabled)
            {
                targetGraphic.color = disabledColor;
            }
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

            DoStateTransition(SelectionState.Pressed);
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

            DoStateTransition(SelectionState.Normal);
        }

        /// <summary>
        /// Called when pointer enter
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }

            DoStateTransition(SelectionState.Highlighted);
        }

        /// <summary>
        /// Called when pointer exit
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }

            DoStateTransition(SelectionState.Normal);
        }

        /// <summary>
        /// Called when selected
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSelect(BaseEventData eventData)
        {
            if (!interactable)
            {
                return;
            }

            DoStateTransition(SelectionState.Selected);
        }

        /// <summary>
        /// Called when deselected
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDeselect(BaseEventData eventData)
        {
            if (!interactable)
            {
                return;
            }

            DoStateTransition(SelectionState.Normal);
        }
    }
}

