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
    public class ScoreView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private TMP_Text scoreValue;

        // Dependencies
        [Inject]
        private IGamePresenter gamePresenter;

        private void Awake()
        {
            // Subscrive score value
            gamePresenter.Score
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnScore);
        }

        /// <summary>
        /// Called when score changes.
        /// </summary>
        /// <param name="score"></param>
        private void OnScore(int score)
        {
            // Set score
            scoreValue.text = score.ToString();
        }
    }
}
