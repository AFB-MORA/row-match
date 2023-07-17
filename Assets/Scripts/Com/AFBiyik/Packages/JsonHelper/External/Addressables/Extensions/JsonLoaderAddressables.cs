using System;
using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using UnityEngine;

namespace Com.AFBiyik.JsonHelper
{
    /// <summary>
    /// Json addressable load functions.
    ///
    /// <example>
    ///
    /// <code>
    /// FooObject foo = await JsonLoaderAddressables.LoadFromAddressables&lt;FooObject&gt;("foojson");
    /// </code>
    ///
    /// </example>
    ///
    /// </summary>
    public static class JsonLoaderAddressables
    {
        private static IAssetLoader assetLoader;

        /// <summary>
        /// Sets asset loader.
        /// </summary>
        /// <param name="loader"></param>
        public static void SetAssetLoader(IAssetLoader loader)
        {
            assetLoader = loader;
        }

        /// <summary>
        /// Deserializes json file into T from addressables.
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="key">Text asset key</param>
        /// <returns>Deserialized object</returns>
        public static async UniTask<T> LoadFromAddressables<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Invalid key value: " + key);
            }

            if (assetLoader == null)
            {
                throw new NullReferenceException("null IAssetLoader reference. Initialize IAssetLoader with SetAssetLoader function.");
            }

            TextAsset text = await assetLoader.LoadAsset<TextAsset>(key);
            return text.ParseJson<T>();
        }
    }
}