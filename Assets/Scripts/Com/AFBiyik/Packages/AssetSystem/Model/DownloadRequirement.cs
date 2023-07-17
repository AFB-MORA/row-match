namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Download requirement returned when <see cref="IAssetDownloader.IsDownloadRequired(string[])"/> method called.
    /// </summary>
    public struct DownloadRequirement
    {
        /// <summary>
        /// True if download required; otherwise false.
        /// </summary>
        public readonly bool isRequired;

        /// <summary>
        /// Total bytes to download. 
        /// </summary>
        public readonly long totalDownloadBytes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isRequired">True if download required; otherwise false.</param>
        /// <param name="totalDownloadBytes">Total bytes to download.</param>
        public DownloadRequirement(bool isRequired, long totalDownloadBytes)
        {
            this.isRequired = isRequired;
            this.totalDownloadBytes = totalDownloadBytes;
        }
    }
}
