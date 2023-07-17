using System;
using System.Collections.Generic;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Util;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene.Presenter
{
    public class GamePresenter : IGamePresenter
    {
        // Private Readonly Fields
        private readonly ReactiveProperty<int> score;
        private readonly ReactiveProperty<int> highScore;
        private readonly ReactiveProperty<int> moveCount;
        private readonly ILevelManager levelManager;
        private readonly LevelModel levelModel;
        private readonly Subject<object> onGameOver;

        // Public Properties
        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> Score => score;

        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> HighScore => highScore;

        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> MoveCount => moveCount;

        /// <inheritdoc/>
        public bool IsHighScore { get; private set; }

        /// <inheritdoc/>
        public HashSet<Guid> AnimatingViews { get; set; }

        /// <inheritdoc/>
        public IObservable<object> OnGameOver => onGameOver;

        /// <summary>
        /// Creates grid presenter with level model
        /// </summary>
        /// <param name="levelModel"></param>
        public GamePresenter(LevelModel levelModel, ILevelManager levelManager)
        {
            this.levelModel = levelModel;
            this.levelManager = levelManager;
            score = new ReactiveProperty<int>(0);
            highScore = new ReactiveProperty<int>(levelModel.HighScore);
            moveCount = new ReactiveProperty<int>(levelModel.MoveCount);
            IsHighScore = false;
            AnimatingViews = new HashSet<Guid>();
            onGameOver = new Subject<object>();
        }

        /// <inheritdoc/>
        public void UpdateScore(ItemType itemType, int columns)
        {
            int scoreToAdd = GameConstants.GetScore(itemType);

            score.Value += scoreToAdd * columns;

            // Check High Score
            if (highScore.Value < score.Value)
            {
                IsHighScore = true;
                highScore.Value = score.Value;
                levelManager.SetHighScore(levelModel.LevelNumber, highScore.Value);
            }
        }

        /// <inheritdoc/>
        public bool MakeMove()
        {
            // Decrease move count
            moveCount.Value--;

            // return has moves
            return moveCount.Value > 0;
        }

        /// <inheritdoc/>
        public void GameOver()
        {
            onGameOver.OnNext(null);
        }
    }
}