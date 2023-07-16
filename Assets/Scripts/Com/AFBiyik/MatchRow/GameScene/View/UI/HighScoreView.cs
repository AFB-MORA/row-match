using Com.AFBiyik.MatchRow.GameScene.VFX;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene.View
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
            gamePresenter.AnimatingViews.Add(gameObject);

            // Wait move animation
            await UniTask.Delay((int)(CompletedEffect.SCORE_MOVE_TWEEN_TIME * 1000));

            bool initial = isInitial;

            // Change value
            UniTask baseTask = base.OnValueChangeAsync(newValue);

            // If not initial
            if (!initial)
            {
                // Show effect
                highScoreEffect.SetActive(true);
                await UniTask.WhenAll(baseTask,
                    UniTask.Delay((int)(lifeTime * 1000))
                    );
                highScoreEffect.SetActive(false);
            }
            else
            {
                await baseTask;
            }

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(gameObject);
        }
    }
}
