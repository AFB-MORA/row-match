using UniRx;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    /// <summary>
    /// Displays move count
    /// </summary>
    public class MoveCountView : BaseValueView
    {
        protected override void SubscribeValue()
        {
            // Subscribe move count value
            gamePresenter.MoveCount
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnValueChange);
        }
    }
}
