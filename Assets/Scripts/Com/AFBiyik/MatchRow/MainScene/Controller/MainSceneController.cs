using Com.AFBiyik.MatchRow.Global.Popup;
using Com.AFBiyik.PopupSystem;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.MainScene
{
    public class MainSceneController : MonoBehaviour
    {
        // Dependencies
        [Inject]
        private IPopupController popupController;

        /// <summary>
        /// Called when levels button clicked
        /// </summary>
        public void OnLevelsClick()
        {
            popupController.Open(PopupConstants.LEVELS_POPUP);
        }
    }
}
