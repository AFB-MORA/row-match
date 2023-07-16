using DG.Tweening;
using UniRx;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    /// <summary>
    /// Displays move count
    /// </summary>
    public class MoveCountView : BaseValueView
    {
        // Constants
        private const int MOVE_WARNING_COUNT = 5;
        private const float PULSE_TWEEN_TIME = 0.8f;

        // Private Fields
        private bool isPulsing;
        private Tweener pulseAnimation;

        protected override void Awake()
        {
            base.Awake();
            isPulsing = false;
        }

        private void OnDestroy()
        {
            pulseAnimation?.Kill();
            pulseAnimation = null;
        }

        protected override void SubscribeValue()
        {
            // Subscribe move count value
            gamePresenter.MoveCount
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnValueChange);
        }

        protected override void OnValueChange(int newValue)
        {
            base.OnValueChange(newValue);

            if (newValue <= MOVE_WARNING_COUNT && !isPulsing)
            {
                isPulsing = true;
                PulseAnimation();
            }
        }

        private void PulseAnimation()
        {
            pulseAnimation = valueText.DOFade(0.2f, PULSE_TWEEN_TIME)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
