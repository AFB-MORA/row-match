using Com.AFBiyik.MatchRow.Global.LevelSystem;

namespace Com.AFBiyik.MatchRow.Global.Manager
{
    /// <summary>
    /// Global project manager
    /// </summary>
    public interface IProjectManager
    {
        /// <summary>
        /// Current playing level
        /// </summary>
        LevelModel CurrentLevel { get; }

        /// <summary>
        /// Sets current level and opens level scene.
        /// </summary>
        /// <param name="levelModel">Current level model</param>
        void PlayLevel(LevelModel levelModel);

        /// <summary>
        /// Opens main scene
        /// </summary>
        void OpenMainMenu();
    }
}
