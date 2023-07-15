using System;
using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Changes gameobject depends on <see cref="SelectionState"/>
    /// </summary>
    [Serializable]
    public class ChangeableButtonGameObject : ChangeableButtonBase
    {
        // Public Fields
        public GameObject normal;
        public GameObject highlighted;
        public GameObject pressed;
        public GameObject selected;
        public GameObject disabled;

        /// <summary>
        /// Disables all gameobjects
        /// </summary>
        private void DisableAll()
        {
            if (normal) normal.SetActive(false);
            if (highlighted) highlighted.SetActive(false);
            if (pressed) pressed.SetActive(false);
            if (selected) selected.SetActive(false);
            if (disabled) disabled.SetActive(false);
        }

        /// <inheritdoc/>
        public override void SetSate(SelectionState state)
        {
            if (state == SelectionState.Normal && normal)
            {
                DisableAll();
                normal.SetActive(true);
            }
            else if (state == SelectionState.Highlighted && highlighted)
            {
                DisableAll();
                highlighted.SetActive(true);
            }
            else if (state == SelectionState.Pressed && pressed)
            {
                DisableAll();
                pressed.SetActive(true);
            }
            else if (state == SelectionState.Selected && selected)
            {
                DisableAll();
                selected.SetActive(true);
            }
            else if (state == SelectionState.Disabled && disabled)
            {
                DisableAll();
                disabled.SetActive(true);
            }
        }
    }
}