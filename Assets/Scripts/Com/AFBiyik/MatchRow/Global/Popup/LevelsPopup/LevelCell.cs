using System;
using Com.AFBiyik.AudioSystem;
using Com.AFBiyik.LanguageSystem;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.MatchRow.Global.Manager;
using Com.AFBiyik.MatchRow.Global.Util;
using Com.AFBiyik.UIComponents;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.Global.Popup
{
    /// <summary>
    /// Level cell for <see cref="RecycleView"/> in <see cref="LevelsPopup"/>
    /// </summary>
    public class LevelCell : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private LanguageBase levelText;
        [SerializeField]
        private LanguageBase highScoreText;
        [SerializeField]
        private StateButton playButton;
        [SerializeField]
        private GameObject lockText;

        // Dependencies
        [Inject]
        private ILevelManager levelManager;
        [Inject]
        private IProjectManager projectManager;
        [Inject]
        private ISoundController2d soundController;

        // Private Fields
        private LevelModel levelModel;

        /// <summary>
        /// Sets level
        /// </summary>
        /// <param name="level">Level number</param>
        public async void SetLevel(int level)
        {
            // Set fields
            levelText.SetValues(level.ToString(), "    ");
            highScoreText.gameObject.SetActive(false);
            lockText.SetActive(true);
            playButton.Interactable = false;

            // Get level model
            levelModel = null;
            try
            {
                levelModel = await levelManager.GetLevel(level);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }

            // If error
            if (levelModel == null)
            {
                return;
            }

            // Set fields
            levelText.SetValues(level.ToString(), levelModel.MoveCount.ToString());
            highScoreText.gameObject.SetActive(!levelModel.IsLocked);
            lockText.SetActive(levelModel.IsLocked);
            playButton.Interactable = !levelModel.IsLocked;

            if (!levelModel.IsLocked)
            {
                highScoreText.SetValues(levelModel.HighScore.ToString());
            }
        }

        /// <summary>
        /// Called with play button
        /// </summary>
        public void PlayClick()
        {
            // Play sound
            soundController.PlaySound(new Sound(SoundConstants.CLICK, volume: 0.2f));

            // Open level
            projectManager.PlayLevel(levelModel);
        }
    }
}
