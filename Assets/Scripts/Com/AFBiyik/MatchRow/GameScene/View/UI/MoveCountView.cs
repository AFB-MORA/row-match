using Com.AFBiyik.MatchRow.GameScene.Presenter;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    /// <summary>
    /// Displays move count
    /// </summary>
    public class MoveCountView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private TMP_Text moveCountValue;

        // Dependencies
        [Inject]
        private IGamePresenter gamePresenter;

        private void Awake()
        {
            // Subscrive move count value
            gamePresenter.MoveCount
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnMoveCount);
        }

        /// <summary>
        /// Called when move count changes.
        /// </summary>
        /// <param name="moveCount"></param>
        private void OnMoveCount(int moveCount)
        {
            // Set move count
            moveCountValue.text = moveCount.ToString();
        }
    }
}
