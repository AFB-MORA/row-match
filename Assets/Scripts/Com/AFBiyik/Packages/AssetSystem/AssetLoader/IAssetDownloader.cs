using Cysharp.Threading.Tasks;
using System;

namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Asset download system.
    /// <example>
    /// <code>
    /// [Inject]
    /// private IAssetDownloader assetDownloader;
    /// 
    /// public async void DownloadAssets() {
    ///     DownloadRequirement requirement = await assetDownloader.IsDownloadRequired("AddressableLabel");
    ///     if (requirement.isRequired) {
    ///         Debug.Log("Reqired Bytes: " + requirement.totalDownloadBytes);
    ///         bool success = await assetDownloader.Download(SetDownloadStatus, "AddressableLabel");
    ///     }
    /// }
    /// 
    /// private void SetDownloadStatus(DownloadStatus status) {
    ///     Debug.Log("Downloaded: " + status.downloadedBytes + "/" + status.totalBytes);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public interface IAssetDownloader
    {
        /// <summary>
        /// Checks is download reqired with intersecting addressable labels.
        /// </summary>
        /// <param name="labels">Intersecting addressable labels</param>
        /// <returns><see cref="DownloadRequirement"/></returns>
        UniTask<DownloadRequirement> IsDownloadRequired(params string[] labels);

        /// <summary>
        /// Downloads assets with intersecting addressable labels.
        /// </summary>
        /// <param name="process">Action called while downloading</param>
        /// <param name="labels">Intersecting addressable labels</param>
        /// <returns>True if successful; otherwise false</returns>
        UniTask<bool> Download(Action<DownloadStatus> process = null, params string[] labels);
    }
}
