using System;
using Com.AFBiyik.LanguageSystem;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
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

        // Private Fields
        private LevelModel levelModel;

        /// <summary>
        /// Sets level
        /// </summary>
        /// <param name="level">Level number</param>
        public async void SetLevel(int level)
        {
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
                // Set fields
                levelText.SetValues(level.ToString(), "?");
                highScoreText.gameObject.SetActive(false);
                lockText.SetActive(true);
                playButton.Interactable = false;
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
            // TODO open level
            Debug.Log("Play Level: " + levelModel.LevelNumber);
        }
    }
}
