using System;
using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Changes scale depends on <see cref="SelectionState"/>
    /// </summary>
    [Serializable]
    public class ChangeableButtonScale : ChangeableButtonBase
    {
        // Public Fields
        public Vector3 normal = new Vector3(1, 1, 1);
        public Vector3 highlighted = new Vector3(1, 1, 1);
        public Vector3 pressed = new Vector3(1, 1, 1);
        public Vector3 selected = new Vector3(1, 1, 1);
        public Vector3 disabled = new Vector3(1, 1, 1);

        /// <inheritdoc/>
        public override void SetSate(SelectionState state)
        {
            if (state == SelectionState.Normal)
            {
                selectable.transform.localScale = normal;
            }
            else if (state == SelectionState.Highlighted)
            {
                selectable.transform.localScale = highlighted;
            }
            else if (state == SelectionState.Pressed)
            {
                selectable.transform.localScale = pressed;
            }
            else if (state == SelectionState.Selected)
            {
                selectable.transform.localScale = selected;
            }
            else if (state == SelectionState.Disabled)
            {
                selectable.transform.localScale = disabled;
            }
        }
    }
}