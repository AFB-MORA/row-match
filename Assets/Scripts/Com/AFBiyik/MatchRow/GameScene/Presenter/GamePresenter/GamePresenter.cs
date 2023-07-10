using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Util;
using Com.AFBiyik.MatchRow.LevelSystem;
using UniRx;

namespace Com.AFBiyik.MatchRow.GameScene.Presenter
{
    public class GamePresenter : IGamePresenter
    {
        // Private Fields
        private readonly ReactiveProperty<int> score;
        private readonly ReactiveProperty<int> moveCount;

        // Public Properties
        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> Score => score;
        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<int> MoveCount => moveCount;

        /// <summary>
        /// Creates grid presenter with level model
        /// </summary>
        /// <param name="levelModel"></param>
        public GamePresenter(LevelModel levelModel)
        {
            score = new ReactiveProperty<int>(0);
            moveCount = new ReactiveProperty<int>(levelModel.MoveCount);
        }

        /// <inheritdoc/>
        public void UpdateScore(ItemType itemType)
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

            score.Value += scoreToAdd;
        }
    }
}