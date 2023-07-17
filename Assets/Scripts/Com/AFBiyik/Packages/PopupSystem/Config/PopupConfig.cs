using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using UnityEngine;

namespace Com.AFBiyik.PopupSystem
{
    public class PopupConfig : ScriptableObject
    {
        [SerializeField]
        private GameObject popupCanvas;

        /// <summary>
        /// Popup canvas to load
        /// </summary>
        public GameObject PopupCanvas => popupCanvas;

        /// <summary>
        /// Loads popup config
        /// </summary>
        /// <param name="assetLoader"></param>
        /// <returns></returns>
        public static async UniTask<PopupConfig> Load(IAssetLoader assetLoader)
        {
            var tmp = await assetLoader.LoadAsset<PopupConfig>("PopupConfig");
            return Instantiate(tmp);
        }
    }
}
