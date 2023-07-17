namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Download requirement returned when <see cref="IAssetDownloader.Download(System.Action{DownloadStatus}, string[])"/> method called.
    /// </summary>
    public struct DownloadStatus
    {
        /// <summary>
        /// Downloaded bytes.
        /// </summary>
        public readonly long downloadedBytes;

        /// <summary>
        /// Total bytes to download. 
        /// </summary>
        public readonly long totalBytes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="downloadedBytes">Downloaded bytes.</param>
        /// <param name="totalBytes">Total bytes to download. </param>
        public DownloadStatus(long downloadedBytes, long totalBytes)
        {
            this.downloadedBytes = downloadedBytes;
            this.totalBytes = totalBytes;
        }
    }
}