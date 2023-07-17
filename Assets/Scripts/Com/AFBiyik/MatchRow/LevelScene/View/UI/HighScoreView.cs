using System;
using Com.AFBiyik.MatchRow.LevelScene.VFX;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.LevelScene.View
{
    /// <summary>
    /// Displays score
    /// </summary>
    public class HighScoreView : BaseValueView
    {
        // Private Fields
        [SerializeField]
        private GameObject highScoreEffect;
        [SerializeField]
        private float lifeTime;

        protected override void Awake()
        {
            base.Awake();
            highScoreEffect.SetActive(false);
        }

        protected override void SubscribeValue()
        {
            // Subscrive score value
            gamePresenter.HighScore
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnValueChange);
        }

        protected override async UniTask OnValueChangeAsync(int newValue)
        {
            // Add to animating views
            Guid animation = Guid.NewGuid();
            gamePresenter.AnimatingViews.Add(animation);

            bool initial = isInitial;

            if (!initial)
            {
                // Wait move animation
                await UniTask.Delay((int)(CompletedEffect.SCORE_MOVE_TWEEN_TIME * 1000));
            }

            // Change value
            UniTask baseTask = base.OnValueChangeAsync(newValue);

            // If not initial
            if (!initial)
            {
                await UniTask.WhenAll(ShowEffect(),
                    BaseTask(baseTask, animation)
                );
            }
            else
            {
                await BaseTask(baseTask, animation);
            }
        }

        private async UniTask BaseTask(UniTask baseTask, Guid animation)
        {
            await baseTask;

            await UniTask.Delay((int)(lifeTime * 1000 * 0.3f));

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(animation);
        }

        private async UniTask ShowEffect()
        {
            // Show effect
            highScoreEffect.SetActive(true);

            await UniTask.Delay((int)(lifeTime * 1000));

            // Hide effect
            highScoreEffect.SetActive(false);
        }
    }
}
