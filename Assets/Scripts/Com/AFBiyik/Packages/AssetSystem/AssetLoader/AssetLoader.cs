using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Asset loader to load and download assets.
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
    public class AssetLoader : IAssetInitializer, IAssetDownloader, IAssetLoader
    {
        /// <summary>
        /// </summary>
        /// <param name="transformAddressableId">Optional. To change addressable id.</param>
        public AssetLoader(Func<IResourceLocation, string> transformAddressableId = null)
        {
            if (transformAddressableId != null)
            {
                Addressables.ResourceManager.InternalIdTransformFunc += transformAddressableId;
            }
        }

        // Initialize
        /// <inheritdoc/>
        public virtual async UniTask Initialize()
        {
            await Addressables.InitializeAsync();
            await UpdateCatalogs();
        }

        protected virtual async UniTask UpdateCatalogs()
        {
            List<string> catalogsToUpdate = new List<string>();
            AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates(false);

            var list = await checkForUpdateHandle;
            Addressables.Release(checkForUpdateHandle);

            catalogsToUpdate.AddRange(list);

            if (catalogsToUpdate.Count > 0)
            {
                AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, false);
                await updateHandle;
                Addressables.Release(updateHandle);
            }
        }

        // Download
        /// <inheritdoc/>
        public virtual async UniTask<DownloadRequirement> IsDownloadRequired(params string[] labels)
        {
            long totalDownloadBytes = 0;

            var locationsHandle = Addressables.LoadResourceLocationsAsync(labels.ToList(), Addressables.MergeMode.Intersection);
            var locations = await locationsHandle;
            Addressables.Release(locationsHandle);

            var downloadSizeHandle = Addressables.GetDownloadSizeAsync(locations);

            totalDownloadBytes = await downloadSizeHandle;

            bool required = totalDownloadBytes > 0;

            Addressables.Release(downloadSizeHandle);

            return new DownloadRequirement(required, totalDownloadBytes);
        }

        /// <inheritdoc/>
        public virtual async UniTask<bool> Download(Action<DownloadStatus> process = null, params string[] labels)
        {

            bool success = true;

            var downloadRequirement = await IsDownloadRequired(labels);
            long totalDownloadBytes = downloadRequirement.totalDownloadBytes;
            bool required = downloadRequirement.isRequired;

            while (required)
            {
                success &= await DownloadAssets(totalDownloadBytes, process, labels);

                if (success)
                {
                    downloadRequirement = await IsDownloadRequired(labels);
                    totalDownloadBytes = downloadRequirement.totalDownloadBytes;
                    required = downloadRequirement.isRequired;
                }
                else
                {
                    required = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Downloads assets with intersecting addressable labels.
        /// </summary>
        /// <param name="totalDownloadBytes">Bytes to donwload</param>
        /// <param name="process">Action called while downloading</param>
        /// <param name="labels">Intersecting addressable labels</param>
        /// <returns>True if successful; otherwise false</returns>
        protected virtual async UniTask<bool> DownloadAssets(long totalDownloadBytes, Action<DownloadStatus> process = null, params string[] labels)
        {
            long downloadedBytes = 0;

            if (totalDownloadBytes > 0)
            {
                downloadedBytes = 0;

                var locationsHandle = Addressables.LoadResourceLocationsAsync(labels.ToList(), Addressables.MergeMode.Intersection);
                var locations = await locationsHandle;
                Addressables.Release(locationsHandle);

                var downloadHandle = Addressables.DownloadDependenciesAsync(locations);

                Observable.EveryGameObjectUpdate()
                    .TakeWhile(_ => downloadHandle.Status == AsyncOperationStatus.None)
                    .Subscribe(_ =>
                    {
                        downloadedBytes = downloadHandle.GetDownloadStatus().DownloadedBytes;
                        process?.Invoke(new DownloadStatus(downloadedBytes, totalDownloadBytes));
                    });

                await downloadHandle;

                if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError("Download Error: " + downloadHandle.Status + "\nException: " + downloadHandle.OperationException?.Message + "\n" + downloadHandle.OperationException?.StackTrace);
                }

                bool success = downloadHandle.Status == AsyncOperationStatus.Succeeded;

                Addressables.Release(downloadHandle);

                return success;
            }

            return true;
        }

        // Load
        /// <inheritdoc/>
        public virtual async UniTask<Sprite> LoadSprite(string atlas, string key, CancellationToken cancellationToken = default)
        {
            string address = atlas + "[" + key + "]";

            Sprite asset = await Addressables.LoadAssetAsync<Sprite>(address).WithCancellation(cancellationToken);
            return asset;
        }

        /// <inheritdoc/>
        public async UniTask<T> LoadAsset<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            T asset = await Addressables.LoadAssetAsync<T>(key).WithCancellation(cancellationToken);
            return asset;
        }

        /// <inheritdoc/>
        public virtual async UniTask<IList<T>> LoadAssets<T>(string label, CancellationToken cancellationToken = default) where T : class
        {
            IList<T> assets = await Addressables.LoadAssetsAsync<T>(label, null);
            return assets;
        }

        /// <inheritdoc/>
        public virtual async UniTask<T> LoadPrefab<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            GameObject asset = await Addressables.LoadAssetAsync<GameObject>(key).WithCancellation(cancellationToken);

            if (asset != null)
            {
                T component = asset.GetComponent<T>();
                return component;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public virtual async UniTask<GameObject> LoadPrefab(string key, CancellationToken cancellationToken = default)
        {
            GameObject asset = await Addressables.LoadAssetAsync<GameObject>(key).WithCancellation(cancellationToken);
            return asset;
        }
    }
}
