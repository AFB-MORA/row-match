using Com.AFBiyik.MatchRow.GameScene.VFX;
using Cysharp.Threading.Tasks;
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

        protected override async UniTask OnValueChangeAsync(int newValue)
        {
            // Add to animating views
            gamePresenter.AnimatingViews.Add(gameObject);

            // Wait move animation
            await UniTask.Delay((int)(CompletedEffect.SCORE_MOVE_TWEEN_TIME * 1000));
            // Change value
            await base.OnValueChangeAsync(newValue);

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(gameObject);
        }
    }
}
