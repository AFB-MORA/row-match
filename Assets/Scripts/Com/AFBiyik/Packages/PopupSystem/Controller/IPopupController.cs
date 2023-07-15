using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Com.AFBiyik.PopupSystem
{
    public interface IPopupController
    {
        /// <summary>
        /// Fired when a popup is closed.
        /// </summary>
        IObservable<PopupData> OnPopupClose { get; }

        /// <summary>
        /// Fired when a popup is opened.
        /// </summary>
        IObservable<PopupData> OnPopupOpen { get; }

        /// <summary>
        /// Closes last opened popup.
        /// </summary>
        /// <param name="args">Arguments passed to new active popup <see cref="IPopup.Refocus(Hashtable)"/></param>
        void Close(Hashtable args = null);

        /// <summary>
        /// Closes all popups
        /// </summary>
        void CloseAll();

        /// <summary>
        /// Closes all popups and opens the popup.
        /// </summary>
        /// <param name="key">Addressable key of the popup</param>
        /// <param name="args">Arguments passed to new active popup <see cref="IPopup.Initialize(Hashtable)"/></param>
        void Open(string key, Hashtable args = null);

        /// <summary>
        /// Opens the popup, on top of other popups.
        /// </summary>
        /// <param name="key">Addressable key of the popup</param>
        /// <param name="args">Arguments passed to new active popup <see cref="IPopup.Initialize(Hashtable)"/></param>
        void OpenTop(string key, Hashtable args = null);

        /// <summary>
        /// Sends message to popup. Popup must be opened.
        /// If more then one popup is opened with key, sends message to all.
        /// </summary>
        /// <param name="key">Addressable key of the popup</param>
        /// <param name="args">Arguments passed to popup <see cref="IPopup.Initialize(Hashtable)"/></param>
        void SendMessage(string key, Hashtable args);

        /// <summary>
        /// Checks is the popup with key is open.
        /// </summary>
        /// <param name="key">Addressable key of the popup</param>
        /// <returns>True if popup is active</returns>
        bool IsPopupActive(string key);

        /// <summary>
        /// Checks is at least 1 popup is open.
        /// </summary>
        /// <returns>True if any popup</returns>
        bool HasPopup();

        /// <summary>
        /// Clears popup chache.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Clears popup stack.
        /// </summary>
        void ClearStack();

        /// <summary>
        /// Clears popup chache and stack.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Preloads popups.
        /// </summary>
        /// <param name="keys">Addressable key of the popups</param>
        UniTask PreloadPopups(IList<string> keys);

        /// <summary>
        /// Preloads popup.
        /// </summary>
        /// <param name="key">Addressable key of the popup</param>
        UniTask PreloadPopup(string key);
    }
}