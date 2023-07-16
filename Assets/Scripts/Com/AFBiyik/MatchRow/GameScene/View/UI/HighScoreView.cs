using UniRx;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    /// <summary>
    /// Displays score
    /// </summary>
    public class HighScoreView : BaseValueView
    {
        protected override void SubscribeValue()
        {
            // Subscrive score value
            gamePresenter.HighScore
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnValueChange);
        }
    }
}
