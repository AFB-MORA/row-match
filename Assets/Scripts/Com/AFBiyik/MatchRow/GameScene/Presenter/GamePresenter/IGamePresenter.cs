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
        /// Game Score
        /// </summary>
        IReadOnlyReactiveProperty<int> HighScore { get; }

        /// <summary>
        /// Move count
        /// </summary>
        IReadOnlyReactiveProperty<int> MoveCount { get; }

        /// <summary>
        /// Updates score
        /// </summary>
        /// <param name="itemType">Row item type</param>
        /// <param name="columns">Number of columns</param>
        void UpdateScore(ItemType itemType, int columns);

        /// <summary>
        /// Decreases move count.
        /// </summary>
        /// <returns>True if has move; otherwise false.</returns>
        bool MakeMove();
    }
}