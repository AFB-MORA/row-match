using Cysharp.Threading.Tasks;

namespace Com.AFBiyik.MatchRow.Global.LevelSystem
{
    /// <summary>
    /// Handles levels.
    /// </summary>
    public interface ILevelManager
    {
        /// <summary>
        /// Number of levels
        /// </summary>
        int NumberOfLevels { get; }

        /// <summary>
        /// Get level
        /// </summary>
        /// <param name="level">Level number</param>
        /// <returns>Level model</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">If level is not between 1 and max number of levels (inclusive). </exception>
        UniTask<LevelModel> GetLevel(int level);

        /// <summary>
        /// Update high score for level
        /// </summary>
        /// <param name="level">Level number</param>
        /// <param name="score">High score</param>
        void SetHighScore(int level, int score);
    }
}