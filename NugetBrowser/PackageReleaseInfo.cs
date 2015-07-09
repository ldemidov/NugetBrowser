namespace NugetBrowser
{
    /// <summary>
    /// Search result item for most popular package search
    /// </summary>
    public class PackageReleaseInfo
    {
        /// <summary>
        /// Package Name
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Package version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Number of downloads for this version
        /// </summary>
        public int DownloadCount { get; set; }

    }
}