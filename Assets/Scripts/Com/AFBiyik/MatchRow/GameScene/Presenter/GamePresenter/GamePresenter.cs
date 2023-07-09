using Com.AFBiyik.MatchRow.LevelSystem;
using UniRx;

namespace Com.AFBiyik.MatchRow.GameScene
{
    public class GamePresenter
    {
        // Private Fields
        private readonly ReactiveProperty<int> score;

        // Public Properties
        public LevelModel LevelModel { get; private set; }
        public IReadOnlyReactiveProperty<int> Score => score;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelModel"></param>
        public GamePresenter(LevelModel levelModel)
        {
            LevelModel = levelModel;
            score = new ReactiveProperty<int>(0);
        }
    }
}