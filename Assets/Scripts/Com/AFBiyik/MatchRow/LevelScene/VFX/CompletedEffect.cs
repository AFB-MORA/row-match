using System;
using Com.AFBiyik.MatchRow.LevelScene.Presenter;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.LevelScene.VFX
{
    /// <summary>
    /// Row completed effect
    /// </summary>
    public class CompletedEffect : MonoBehaviour
    {
        // Constants
        public const float SCORE_MOVE_TWEEN_TIME = 1.2f;

        // Serialize Fields
        [SerializeField]
        private float lifeTime;
        [SerializeField]
        private ParticleSystem[] particleSystems;
        [SerializeField]
        private TMP_Text scoreText;

        // Dependencies
        [Inject]
        private IGamePresenter gamePresenter;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows effect
        /// </summary>
        /// <param name="fromColor">Particle system from color</param>
        /// <param name="toColor">Particle system to color</param>
        /// <param name="position">Effect position</param>
        /// <param name="scorePosition">Score UI position</param>
        /// <param name="score">Score value</param>
        /// <returns></returns>
        public async UniTask Show(Color fromColor, Color toColor, Vector3 position, Vector3 scorePosition, float score)
        {
            // Add to animating views
            Guid animation = Guid.NewGuid();
            gamePresenter.AnimatingViews.Add(animation);

            // Enable game object
            gameObject.SetActive(true);

            // Foreach particle systeö
            foreach (var particleSystem in particleSystems)
            {
                // Get main
                var main = particleSystem.main;
                // Get color
                var startColor = main.startColor;
                // Set colors
                startColor.colorMin = fromColor;
                startColor.colorMax = toColor;
            }

            // Set position
            transform.position = position;
            // Set text
            scoreText.text = score.ToString();
            // Set score scale
            scoreText.transform.localScale = new Vector3(1, 1, 1);
            // Set score position
            scoreText.transform.localPosition = new Vector3(0, 0, 0);

            // Animate score
            await UniTask.WhenAll(
                scoreText.transform.DOMove(scorePosition, SCORE_MOVE_TWEEN_TIME).SetEase(Ease.InOutQuad).ToUniTask(),
                scoreText.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), SCORE_MOVE_TWEEN_TIME).SetEase(Ease.InOutQuad).ToUniTask()
            );

            // Reset score
            scoreText.text = "";

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(animation);

            // Wait for particles
            await UniTask.Delay((int)(lifeTime * 1000));

            // Disable game obejct
            gameObject.SetActive(false);
        }

    }
}