using UniRx;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    /// <summary>
    /// Displays score
    /// </summary>
    public class ScoreView : BaseValueView
    {
        protected override void SubscribeValue()
        {
            // Subscribe score value
            gamePresenter.Score
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnValueChange);
        }
    }
}
