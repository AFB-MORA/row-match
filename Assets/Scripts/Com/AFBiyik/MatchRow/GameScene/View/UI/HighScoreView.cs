using Com.AFBiyik.MatchRow.GameScene.Presenter;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    /// <summary>
    /// Displays score
    /// </summary>
    public class HighScoreView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private TMP_Text highScoreValue;

        // Dependencies
        [Inject]
        private IGamePresenter gamePresenter;

        private void Awake()
        {
            // Subscrive score value
            gamePresenter.HighScore
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnHighScore);
        }

        /// <summary>
        /// Called when high score changes.
        /// </summary>
        /// <param name="highScore"></param>
        private void OnHighScore(int highScore)
        {
            // Set score
            highScoreValue.text = highScore.ToString();
        }
    }
}
