using System.Collections;
using Cysharp.Threading.Tasks;

namespace Com.AFBiyik.PopupSystem
{
    public static class PopupExtensions
    {
        /// <summary>
        /// Enables popup.
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async UniTask Show(this IPopup popup, Hashtable args)
        {
            popup.gameObject.transform.SetAsLastSibling();
            popup.gameObject.SetActive(true);

            popup.OnOpened(args);

            if (popup is IAnimatedPopup animatedPopup)
            {
                await animatedPopup.PlayOpenAnimation();
            }
        }

        /// <summary>
        /// Disables popup.
        /// </summary>
        /// <param name="popup"></param>
        /// <returns></returns>
        public static async UniTask Close(this IPopup popup)
        {
            if (popup is IAnimatedPopup animatedPopup)
            {
                await animatedPopup.PlayCloseAnimation();
            }

            popup.gameObject.SetActive(false);
            popup.OnClosed();
        }
    }
}
