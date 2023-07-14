using Com.AFBiyik.AssetSystem;

namespace Com.AFBiyik.JsonHelper
{
    public class JsonAssetLoaderInitializer
    {
        /// <summary>
        /// Initializes <see cref="JsonLoaderAddressables"/>.
        /// </summary>
        /// <param name="assetLoader"></param>
        public JsonAssetLoaderInitializer(IAssetLoader assetLoader)
        {
            JsonLoaderAddressables.SetAssetLoader(assetLoader);
        }
    }
}
