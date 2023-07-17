using System;
using Com.AFBiyik.AudioSystem;
using Com.AFBiyik.MatchRow.Global.Util;
using Com.AFBiyik.MatchRow.LevelScene.VFX;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Com.AFBiyik.MatchRow.LevelScene.View
{
    /// <summary>
    /// Displays score
    /// </summary>
    public class ScoreView : BaseValueView
    {
        [Inject]
        private ISoundController2d soundController;

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
            Guid animation = Guid.NewGuid();
            gamePresenter.AnimatingViews.Add(animation);

            if (!isInitial)
            {
                // Wait move animation
                await UniTask.Delay((int)(CompletedEffect.SCORE_MOVE_TWEEN_TIME * 1000));

                // Play sound
                soundController.PlaySound(new Sound(SoundConstants.SCORE, volume: 0.03f));
            }

            // Change value
            await base.OnValueChangeAsync(newValue);

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(animation);
        }
    }
}
