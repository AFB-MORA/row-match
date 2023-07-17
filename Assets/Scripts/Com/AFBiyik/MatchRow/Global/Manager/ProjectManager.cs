using System.Collections.Generic;
using Com.AFBiyik.AudioSystem;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.MatchRow.Global.Popup;
using Com.AFBiyik.MatchRow.Global.Util;
using Com.AFBiyik.PopupSystem;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.SceneManagement;

namespace Com.AFBiyik.MatchRow.Global.Manager
{
    /// <inheritdoc/>
    public class ProjectManager : IProjectManager
    {
        // Private Readonly Properties
        private readonly IPopupController popupController;
        private readonly IMusicController musicController;

        /// <inheritdoc/>
        public LevelModel CurrentLevel { get; private set; }

        /// <summary>
        /// Creates project manager.
        /// </summary>
        public ProjectManager(IPopupController popupController, IMusicController musicController)
        {
            this.musicController = musicController;
            this.popupController = popupController;

            popupController.PreloadPopups(new List<string>() { PopupConstants.LEVELS_POPUP, PopupConstants.GAME_OVER_POPUP });

            musicController.IsLoaded.Subscribe(OnMusicLoaded);
        }

        private void OnMusicLoaded(bool loaded)
        {
            if (loaded)
            {
                musicController.PlayMusic(musics: new Sound[] { new Sound(SoundConstants.MUSIC, volume: 0.1f) });
            }
        }

        /// <inheritdoc/>
        public void PlayLevel(LevelModel levelModel)
        {
            CurrentLevel = levelModel;
            OpenScene(SceneConstants.LEVEL_SCENE);
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