using System;
using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Changes sprite depends on <see cref="SelectionState"/>
    /// </summary>
    [Serializable]
    public class ChangeableButtonSprite : ChangeableButtonBase
    {
        // Public Fields
        public Sprite normal;
        public Sprite highlighted;
        public Sprite pressed;
        public Sprite selected;
        public Sprite disabled;

        /// <inheritdoc/>
        public override void SetSate(SelectionState state)
        {
            var targetGraphic = selectable.TargetGraphic;

            if (targetGraphic == null)
            {
                Debug.LogWarning("No Target Graphic: " + selectable.gameObject.name);
                return;
            }

            if (state == SelectionState.Normal && normal)
            {
                targetGraphic.sprite = normal;
            }
            else if (state == SelectionState.Highlighted && highlighted)
            {
                targetGraphic.sprite = highlighted;
            }
            else if (state == SelectionState.Pressed && pressed)
            {
                targetGraphic.sprite = pressed;
            }
            else if (state == SelectionState.Selected && selected)
            {
                targetGraphic.sprite = selected;
            }
            else if (state == SelectionState.Disabled && disabled)
            {
                targetGraphic.sprite = disabled;
            }
        }
    }
}