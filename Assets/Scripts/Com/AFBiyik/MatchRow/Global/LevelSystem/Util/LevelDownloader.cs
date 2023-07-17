using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Com.AFBiyik.MatchRow.Global.LevelSystem
{
    /// <summary>
    /// Downloads level data
    /// </summary>
    internal class LevelDownloader
    {
        /// <summary>
        /// Check is download required
        /// </summary>
        public static void CheckLevels()
        {
            // If all levels is not levels downloaded
            if (!IsAllLevelsDownloaded())
            {
                // Download levels
                DownloadLevels();
            }
        }

        /// <summary>
        /// Checks internet connection
        /// </summary>
        /// <returns></returns>
        private static bool HasConnection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        /// <summary>
        /// Checks is all levels downloaded
        /// </summary>
        /// <returns></returns>
        private static bool IsAllLevelsDownloaded()
        {
            bool isAllLevelsDownloaded = true;

            // For each levels that needs to be downloaded
            for (int i = LevelConstants.PRELOADED_LEVELS; i < LevelConstants.NUMBER_OF_LEVELS; i++)
            {
                // Check is level downloaded
                isAllLevelsDownloaded = isAllLevelsDownloaded && IsLevelDownloaded(i);

                // Break if download is required
                if (!isAllLevelsDownloaded)
                {
                    break;
                }
            }

            return isAllLevelsDownloaded;
        }

        /// <summary>
        /// Checks is level downloaded
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static bool IsLevelDownloaded(int level)
        {
            // Get file path
            string path = LevelConstants.GetLevelPath(level);
            // Check is file exists
            return File.Exists(path);
        }

        /// <summary>
        /// Download levels
        /// </summary>
        private static void DownloadLevels()
        {
            // If has not connection
            if (!HasConnection())
            {
                // Try download
                TryDownloadAgain();
                return;
            }

            // For each levels that needs to be downloaded
            for (int i = LevelConstants.PRELOADED_LEVELS + 1; i <= LevelConstants.NUMBER_OF_LEVELS; i++)
            {
                // Download
                DownloadLevel(i);
            }
        }

        /// <summary>
        /// Try download again
        /// </summary>
        private static async void TryDownloadAgain()
        {
            // If no connection
            while (!HasConnection())
            {
                // Delay 500 ms
                await UniTask.Delay(500);
            }

            // Download
            DownloadLevels();
        }

        /// <summary>
        /// Downloads level
        /// </summary>
        /// <param name="level"></param>
        private static async void DownloadLevel(int level)
        {
            // If level is not downloaded
            if (!IsLevelDownloaded(level))
            {
                // Get url
                string url = LevelConstants.GetLevelUrl(level);
                // Get path
                string path = LevelConstants.GetLevelPath(level);

                // Create request
                UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
                // File download handler
                request.downloadHandler = new DownloadHandlerFile(path);

                // Send request
                await request.SendWebRequest();

                // If error
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(request.error);
                    // Delay 1000 ms
                    await UniTask.Delay(1000);
                    // Try download
                    TryDownloadAgain();
                }
            }
        }
    }
}
