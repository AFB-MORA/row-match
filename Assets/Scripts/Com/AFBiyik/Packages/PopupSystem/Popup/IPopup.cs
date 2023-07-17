using System.Collections;
using UnityEngine;

namespace Com.AFBiyik.PopupSystem
{
    /// <summary>
    /// Base popup interface for all popups
    /// </summary>
    public interface IPopup
    {
        /// <summary>
        /// Popup Key
        /// </summary>
        string PopupKey { get; }

        GameObject gameObject { get; }

        /// <summary>
        /// Called when popup is opened.
        /// </summary>
        /// <param name="args">Arguments passed to popup</param>
        void OnOpened(Hashtable args);

        /// <summary>
        /// Called when popup is closed.
        /// </summary>
        void OnClosed();

        /// <summary>
        /// Called when another popup is opened on top of this popup.
        /// </summary>
        void OnUnfocus();

        /// <summary>
        /// Called when the popup on top of this popup is closed.
        /// </summary>
        /// <param name="args">Arguments passed to popup</param>
        void OnRefocus(Hashtable args);

        /// <summary>
        /// Called when a message is sent.
        /// </summary>
        /// <param name="args"></param>
        void ProcessMessage(Hashtable args);
    }
}