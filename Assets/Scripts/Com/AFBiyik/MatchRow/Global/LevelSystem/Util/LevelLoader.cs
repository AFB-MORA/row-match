using System.Collections.Generic;
using System.IO;
using Com.AFBiyik.AssetSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.Global.LevelSystem
{
    /// <summary>
    /// Loads level from project or persistent data path.
    /// </summary>
    internal class LevelLoader
    {
        // Private Readonly Fields
        private readonly IAssetLoader assetLoader;

        /// <summary>
        /// Create level loader.
        /// </summary>
        /// <param name="assetLoader">To load levels from project</param>
        public LevelLoader(IAssetLoader assetLoader)
        {
            this.assetLoader = assetLoader;
        }

        /// <summary>
        /// Loads level from project or persistent data path.
        /// </summary>
        /// <param name="level">Level number</param>
        /// <returns>Loaded levels</returns>
        /// <exception cref="FileNotFoundException">If file not found</exception>
        public async UniTask<LevelModel> LoadLevel(int level)
        {
            // Get level values
            var levelDict = await GetLevelDataValues(level);

            // Create level
            LevelModel levelModel = new LevelModel();

            // Assign level values
            foreach (var pair in levelDict)
            {
                if (pair.Key == "level_number")
                {
                    levelModel.LevelNumber = int.Parse(pair.Value);
                }
                else if (pair.Key == "grid_width")
                {
                    levelModel.GridWidth = int.Parse(pair.Value);
                }
                else if (pair.Key == "grid_height")
                {
                    levelModel.GridHeight = int.Parse(pair.Value);
                }
                else if (pair.Key == "move_count")
                {
                    levelModel.MoveCount = int.Parse(pair.Value);
                }
                else if (pair.Key == "grid")
                {
                    levelModel.Grid = pair.Value.Split(",");
                }
            }

            return levelModel;
        }

        /// <summary>
        /// Laods and splits data from level data text.
        /// </summary>
        /// <param name="level">Level number</param>
        /// <returns>Key value pairs</returns>
        /// <exception cref="FileNotFoundException">If file not found</exception>
        private async UniTask<Dictionary<string, string>> GetLevelDataValues(int level)
        {
            // Get level text
            string text = await LoadLevelText(level);

            // Create dictionary
            var levelData = new Dictionary<string, string>();

            // Split text
            var rows = text.Split("\n");

            // For each row
            foreach (var row in rows)
            {
                // Split key value pairs
                var pair = row.Split(":");
                // Set value to key
                levelData[pair[0]] = pair[1].Trim();
            }

            return levelData;
        }

        /// <summary>
        /// Laods level text data.
        /// </summary>
        /// <param name="level">Level number</param>
        /// <returns>Loaded level data</returns>
        /// <exception cref="FileNotFoundException">If file not found</exception>
        private async UniTask<string> LoadLevelText(int level)
        {
            string text;

            // If pre loaded levels
            if (level <= LevelConstants.PRELOADED_LEVELS)
            {
                // Load from addressables
                var textAsset = await assetLoader.LoadAsset<TextAsset>($"RM_A{level}");
                // Read text
                text = textAsset.text;
            }
            else
            {
                // Load from file
                var path = LevelConstants.GetLevelPath(level);

                // Check file exists
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("File not found at path: " + path);
                }

                // Read text
                text = await File.ReadAllTextAsync(path);
            }

            return text;
        }
    }
}
