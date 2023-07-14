using System;
using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Base class for changeable buttons
    /// </summary>
    [Serializable]
    public abstract class ChangeableButtonBase
    {
        /// <summary>
        /// Selectable ui element
        /// </summary>
        [HideInInspector]
        public UIButton selectable;

        /// <summary>
        /// Updates button with state
        /// </summary>
        /// <param name="state">New state</param>
        public abstract void SetSate(SelectionState state);
    }
}
