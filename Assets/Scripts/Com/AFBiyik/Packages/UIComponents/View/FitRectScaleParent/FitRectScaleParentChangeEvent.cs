using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents.View
{
    public class FitRectScaleParentChangeEvent : UIBehaviour
    {
        // Public Fields
        public UnityEvent onRectChange = new UnityEvent();

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            onRectChange?.Invoke();
        }
    }
}
