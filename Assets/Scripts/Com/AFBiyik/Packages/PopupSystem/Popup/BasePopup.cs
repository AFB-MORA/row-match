using System.Collections;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.PopupSystem
{
    /// <summary>
    /// Base popup class for all popups
    /// </summary>
    public abstract class BasePopup : MonoBehaviour, IPopup
    {
        /// <summary>
        /// Popup controller
        /// </summary>
        [Inject]
        protected IPopupController popupController;

        /// <inheritdoc/>
        public virtual string PopupKey { get => this.GetType().Name; }

        /// <inheritdoc/>
        public virtual void OnOpened(Hashtable args)
        {
        }

        /// <inheritdoc/>
        public virtual void OnClosed()
        {

        }

        /// <inheritdoc/>
        public virtual void OnUnfocus()
        {

        }

        /// <inheritdoc/>
        public virtual void OnRefocus(Hashtable args)
        {

        }

        /// <inheritdoc/>
        public virtual void ProcessMessage(Hashtable args)
        {
        }

        /// <summary>
        /// Closes active popup.
        /// </summary>
        protected virtual void ClosePopup(Hashtable args = null)
        {
            popupController.Close(args);
        }

    }
}
