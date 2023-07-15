using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.MatchRow.Global.Util;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Com.AFBiyik.MatchRow.Global.Manager
{
    /// <inheritdoc/>
    public class ProjectManager : IProjectManager
    {
        /// <inheritdoc/>
        public LevelModel CurrentLevel { get; private set; }

        /// <summary>
        /// Creates project manager.
        /// </summary>
        public ProjectManager()
        {

        }

        /// <inheritdoc/>
        public void PlayLevel(LevelModel levelModel)
        {
            CurrentLevel = levelModel;
            OpenScene(SceneConstants.GAME_SCENE);
        }

        /// <summary>
        /// Opens scene with name
        /// </summary>
        /// <param name="scene">Scene name</param>
        private async void OpenScene(string scene)
        {
            await SceneManager.LoadSceneAsync(scene);
        }
    }
}