using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using UniRx;

namespace Com.AFBiyik.MatchRow.GameScene.Presenter
{
    public interface IGamePresenter
    {
        /// <summary>
        /// Game Score
        /// </summary>
        IReadOnlyReactiveProperty<int> Score { get; }

        /// <summary>
        /// Move count
        /// </summary>
        IReadOnlyReactiveProperty<int> MoveCount { get; }

        /// <summary>
        /// Updates score
        /// </summary>
        /// <param name="itemType">Row item type</param>
        void UpdateScore(ItemType itemType);
    }
}
