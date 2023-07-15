using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.MatchRow.Global.Popup;
using Com.AFBiyik.MatchRow.Global.Util;
using Com.AFBiyik.PopupSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Com.AFBiyik.MatchRow.Global.Manager
{
    /// <inheritdoc/>
    public class ProjectManager : IProjectManager
    {
        // Private Readonly Properties
        private readonly IPopupController popupController;

        /// <inheritdoc/>
        public LevelModel CurrentLevel { get; private set; }

        /// <summary>
        /// Creates project manager.
        /// </summary>
        public ProjectManager(IPopupController popupController)
        {
            this.popupController = popupController;
        }

        /// <inheritdoc/>
        public void PlayLevel(LevelModel levelModel)
        {
            CurrentLevel = levelModel;
            OpenScene(SceneConstants.GAME_SCENE);
        }

        /// <inheritdoc/>
        public async void OpenMainMenu()
        {
            OpenScene(SceneConstants.MAIN_SCENE);
            await UniTask.Delay(50);
            popupController.Open(PopupConstants.LEVELS_POPUP);
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