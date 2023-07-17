using System.Collections;
using Com.AFBiyik.AudioSystem;
using Com.AFBiyik.MatchRow.Global.Manager;
using Com.AFBiyik.MatchRow.Global.Util;
using Com.AFBiyik.PopupSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.Global.Popup
{
    /// <summary>
    /// Popup that is showed when game over.
    /// <br/>
    /// Parameters:
    /// <br/>
    /// isHighScore: book
    /// <br/>
    /// highScore: int
    /// <br/>
    /// score: int
    /// </summary>
    public class GameOverPopup : BasePopup
    {
        // Serialize Field
        [SerializeField]
        private TMP_Text highScoreValue;
        [SerializeField]
        private TMP_Text scoreValue;
        [SerializeField]
        private GameObject celebrationView;
        [SerializeField]
        private GameObject normalView;

        // Dependencies
        [Inject]
        private IProjectManager projectManager;
        [Inject]
        private ISoundController2d soundController;

        // Private Fields
        private IFxSource starSource;
        private IFxSource celebrationSource;

        /// <inheritdoc/>
        public override async void OnOpened(Hashtable args)
        {
            base.OnOpened(args);

            starSource = null;
            celebrationSource = null;

            // Get is high score
            bool isHighScore = (bool)args["isHighScore"];
            // Get high score
            int highScore = (int)args["highScore"];

            // Enable views
            celebrationView.SetActive(isHighScore);
            normalView.SetActive(!isHighScore);

            // Set high score text
            highScoreValue.text = highScore.ToString();

            // If not high score
            if (!isHighScore)
            {
                // Set score
                int score = (int)args["score"];
                scoreValue.text = score.ToString();
            }
            else
            {
                starSource = await soundController.PlayAndGetSource(new Sound(SoundConstants.STAR, loop: true, volume: 0.1f));
                celebrationSource = await soundController.PlayAndGetSource(new Sound(SoundConstants.CELEBRATION, volume: 0.1f));
            }
        }

        /// <inheritdoc/>
        public override void OnClosed()
        {
            base.OnClosed();

            starSource?.Stop();
            starSource = null;

            if (celebrationSource != null && celebrationSource.Source.isPlaying && celebrationSource.Source.clip.name == SoundConstants.CELEBRATION)
            {
                celebrationSource.Stop();
            }
            celebrationSource = null;
        }

        /// <summary>
        /// Opens main menu
        /// </summary>
        public void OpenMainMenu()
        {
            // Close popup
            ClosePopup();
            // Open main scene
            projectManager.OpenMainMenu();
        }
    }
}
