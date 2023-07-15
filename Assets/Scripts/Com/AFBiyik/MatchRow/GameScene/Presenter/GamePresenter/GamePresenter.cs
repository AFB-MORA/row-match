using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Util;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using UniRx;

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

        // Public Properties
        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> Score => score;

        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> HighScore => highScore;

        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> MoveCount => moveCount;

        /// <inheritdoc/>
        public bool IsHighScore { get; private set; }

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
        }

        /// <inheritdoc/>
        public void UpdateScore(ItemType itemType, int columns)
        {
            int scoreToAdd = 0;
            switch (itemType)
            {
                case ItemType.Red:
                    scoreToAdd = GameConstants.RED_SCORE;
                    break;
                case ItemType.Green:
                    scoreToAdd = GameConstants.GREEN_SCORE;
                    break;
                case ItemType.Blue:
                    scoreToAdd = GameConstants.BLUE_SCORE;
                    break;
                case ItemType.Yellow:
                    scoreToAdd = GameConstants.YELLOW_SCORE;
                    break;
            }

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
    }
}