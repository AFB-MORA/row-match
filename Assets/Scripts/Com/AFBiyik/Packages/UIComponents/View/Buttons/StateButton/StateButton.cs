using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Update button state with <see cref="ChangeableButtonBase"/>
    /// </summary>
    public class StateButton : UIButton
    {
        // Serialize Fields
        [SerializeField]
        protected bool disableDefaultButtonStates = false;
        [SerializeField]
        protected ChangeableButtonType stateType = ChangeableButtonType.None;
        [SerializeReference]
        protected ChangeableButtonBase changeableButton;

        // Public Properties
        public ChangeableButtonBase ChangeableButton {
            get => changeableButton;
            set => changeableButton = value;
        }

        /// <inheritdoc/>
        protected override void DoStateTransition(SelectionState state) {
            if (!disableDefaultButtonStates) {
                base.DoStateTransition(state);
            }

            SetState(state);
        }

        /// <summary>
        /// Sets state with <see cref="changeableButton"/>
        /// </summary>
        /// <param name="state">New state</param>
        protected virtual void SetState(SelectionState state) {
            changeableButton?.SetSate(state);
        }
    }
}
