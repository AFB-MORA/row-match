using System;
using System.Collections.Generic;
using Com.AFBiyik.AssetSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.Global.LevelSystem
{
    /// <summary>
    /// Handles levels.
    /// </summary>
    public class LevelManager : ILevelManager
    {
        // Private Readonly Fields
        private readonly LevelLoader levelLoader;
        private readonly Dictionary<int, LevelModel> loadedLevels; // Cache levels

        // Public Properties
        /// <inheritdoc/>
        public int NumberOfLevels => LevelConstants.NUMBER_OF_LEVELS;

        /// <summary>
        /// Creates level manager
        /// </summary>
        /// <param name="assetLoader"></param>
        public LevelManager(IAssetLoader assetLoader)
        {
            levelLoader = new LevelLoader(assetLoader);
            loadedLevels = new Dictionary<int, LevelModel>();

            // Check levels
            LevelDownloader.CheckLevels();
        }

        /// <inheritdoc/>
        public async UniTask<LevelModel> GetLevel(int level)
        {
            // Check cache 
            if (loadedLevels.ContainsKey(level))
            {
                // Return from cache
                return loadedLevels[level];
            }

            ///
            if (level < 1 || level > LevelConstants.NUMBER_OF_LEVELS)
            {
                throw new ArgumentOutOfRangeException($"Level Number must be between 1 and {LevelConstants.NUMBER_OF_LEVELS} (inclusive).");
            }

            var levelModel = await levelLoader.LoadLevel(level);
            levelModel.HighScore = GetHighScore(level);

            loadedLevels[level] = levelModel;

            return levelModel;
        }

        /// <summary>
        /// Get high score of the level
        /// </summary>
        /// <param name="level">Level number</param>
        /// <returns>High score</returns>
        private int GetHighScore(int level)
        {
            return PlayerPrefs.GetInt($"LevelHighScore_{level}", 0);
        }

        /// <inheritdoc/>
        public void SetHighScore(int level, int score)
        {
            // Save high score
            PlayerPrefs.SetInt($"LevelHighScore_{level}", score);
            // Update cache
            loadedLevels[level].HighScore = score;
        }
    }
}