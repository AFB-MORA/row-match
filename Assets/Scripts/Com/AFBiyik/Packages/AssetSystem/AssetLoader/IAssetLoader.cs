using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Asset loader to load assets.
    /// <example>
    /// <code>
    /// [Inject]
    /// private IAssetLoader assetLoader;
    /// 
    /// public async void Load() {
    ///     Sprite sprite = await assetLoader.LoadSprite("Atlas", "Sprite");
    ///     
    ///     TextAsset gameDataJson = await assetLoader.LoadAsset&lt;TextAsset&gt;("GameData");
    ///     
    ///     IList&lt;TextAsset&gt; levelDataJson = await assetLoader.LoadAssets&lt;TextAsset&gt;("LevelData");
    ///     
    ///     FooPrefab foo = await assetLoader.LoadPrefab&lt;FooPrefab&gt;("FooPrefab");
    ///     
    ///     BarPrefab bar = await assetLoader.LoadPrefab&lt;BarPrefab&gt;("BarPrefab");
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Loads sprite in the atlas with key.
        /// </summary>
        /// <param name="atlas">Atlas key</param>
        /// <param name="key">Sprite name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Sprite</returns>
        UniTask<Sprite> LoadSprite(string atlas, string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Loads an asset with key.
        /// </summary>
        /// <typeparam name="T">Asset type</typeparam>
        /// <param name="key">Asset key</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Asset</returns>
        UniTask<T> LoadAsset<T>(string key, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Loads multiple assets with label.
        /// </summary>
        /// <typeparam name="T">Asset type</typeparam>
        /// <param name="label">Label of assets</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Assets</returns>
        UniTask<IList<T>> LoadAssets<T>(string label, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Loads prefab with key.
        /// </summary>
        /// <typeparam name="T">Prefab type</typeparam>
        /// <param name="key">Prefab key/param>
        /// <param name="cancellationToken"></param>
        /// <returns>Prefab</returns>
        UniTask<T> LoadPrefab<T>(string key, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Loads prefab with key.
        /// </summary>
        /// <param name="key">Prefab key</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Prefab</returns>
        UniTask<GameObject> LoadPrefab(string key, CancellationToken cancellationToken = default);
    }
}