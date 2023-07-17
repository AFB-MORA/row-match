using System;
using System.Collections.Generic;
using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using UniRx;

namespace Com.AFBiyik.MatchRow.LevelScene.Presenter
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
        /// Game over event
        /// </summary>
        IObservable<bool> OnGameOver { get; }

        /// <summary>
        /// True if high score changed; otherwise false
        /// </summary>
        bool IsHighScore { get; }

        /// <summary>
        /// Animating Views
        /// </summary>
        HashSet<Guid> AnimatingViews { get; set; }

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

        /// <summary>
        /// Sets game over
        /// </summary>
        void GameOver();
    }
}
