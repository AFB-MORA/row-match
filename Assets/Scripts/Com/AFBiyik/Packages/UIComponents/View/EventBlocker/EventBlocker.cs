using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Blocks input events from lover layers
    /// </summary>
    public class EventBlocker : MonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IBeginDragHandler,
        IEndDragHandler,
        IDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerMoveHandler,
        IScrollHandler
    {
        public void OnBeginDrag(PointerEventData eventData) { }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData) { }

        public void OnPointerClick(PointerEventData eventData) { }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerEnter(PointerEventData eventData) { }

        public void OnPointerExit(PointerEventData eventData) { }

        public void OnPointerMove(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) { }

        public void OnScroll(PointerEventData eventData) { }
    }
}
