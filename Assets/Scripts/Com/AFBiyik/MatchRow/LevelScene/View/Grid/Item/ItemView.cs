using System;
using Com.AFBiyik.MatchRow.Global.Util;
using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using Com.AFBiyik.MatchRow.LevelScene.Presenter;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.LevelScene.View
{
    /// <summary>
    /// Grid item view
    /// </summary>
    public class ItemView : MonoBehaviour
    {
        // Constants
        private const float MOVE_TWEEN_TIME = 0.2f;
        private const float SCALE_TWEEN_TIME = 0.4f;

        // Serialize Fields
        [SerializeField]
        private ItemType itemType;
        [SerializeField]
        private SpriteRenderer image;
        [SerializeField]
        private CircleCollider2D circleCollider;
        [SerializeField]
        private SpriteRenderer completedObject;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;
        [Inject]
        private IGamePresenter gamePresenter;

        // Private fields
        private Vector2Int gridPosition;
        private MaterialPropertyBlock imagePropertyBlock;
        private MaterialPropertyBlock completedPropertyBlock;

        // Public Properties
        public ItemType ItemType => itemType;
        public Vector2Int GridPosition => gridPosition;

        private void Awake()
        {
            // Subscribe to game over
            gamePresenter.OnGameOver
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnGameOver);
        }

        /// <summary>
        /// Initializes item
        /// </summary>
        /// <param name="gridPosition"></param>
        public void Initialize(Vector2Int gridPosition)
        {
            // Set property block
            imagePropertyBlock = new MaterialPropertyBlock();
            image.GetPropertyBlock(imagePropertyBlock);
            completedPropertyBlock = new MaterialPropertyBlock();
            completedObject.GetPropertyBlock(completedPropertyBlock);

            // Set grid position
            this.gridPosition = gridPosition;

            // Set object name
            gameObject.name = $"({gridPosition.x}, {gridPosition.y})";

            UpdateSize();
        }

        /// <summary>
        /// Update cell size
        /// </summary>
        public void UpdateSize()
        {
            // Set position
            transform.position = gridPresenter.GridPositionToWorldPosition(gridPosition);

            // Get cell size
            float size2 = gridPresenter.CellSize / 2f;
            // Set image
            image.size = new Vector2(gridPresenter.CellSize, gridPresenter.CellSize);
            image.transform.localPosition = new Vector3(size2, size2, 0);
            // Set completed object
            completedObject.size = new Vector2(gridPresenter.CellSize, gridPresenter.CellSize);
            completedObject.transform.localPosition = new Vector3(size2, size2, 0);
            // Set collider
            circleCollider.radius = size2;
            circleCollider.offset = new Vector2(circleCollider.radius, circleCollider.radius);

            // Disable completed object
            completedObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// Moves item to new grid position.
        /// </summary>
        /// <param name="gridPosition">New grid position</param>
        public async void MoveTo(Vector2Int gridPosition)
        {
            // Add to animating views
            Guid animation = Guid.NewGuid();
            gamePresenter.AnimatingViews.Add(animation);

            // Set grid position
            this.gridPosition = gridPosition;

            // Set position
            var newPosition = gridPresenter.GridPositionToWorldPosition(gridPosition);
            await transform.DOMove(newPosition, MOVE_TWEEN_TIME);

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(animation);
        }

        /// <summary>
        /// Changes item type.
        /// </summary>
        /// <param name="newItemType">New item type</param>
        public void UpdateType(ItemType newItemType)
        {
            if (itemType != ItemType.Completed && newItemType == ItemType.Completed)
            {
                itemType = newItemType;
                ShowCompleted();
            }
        }

        /// <summary>
        /// Shows completed animation
        /// </summary>
        private async void ShowCompleted()
        {
            // Wait for move
            await UniTask.Delay((int)(MOVE_TWEEN_TIME * 1000));
            // Set completed
            completedObject.gameObject.SetActive(true);
            completedObject.color = new Color(1, 1, 1, 0);
            // Animate
            await UniTask.WhenAll(
                completedObject.DOFade(1, SCALE_TWEEN_TIME).ToUniTask(),
                image.transform.DOScale(new Vector3(0, 0, 0), SCALE_TWEEN_TIME).ToUniTask());
            // Disable image
            image.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when game is over
        /// </summary>
        /// <param name="isHighScore"></param>
        private async void OnGameOver(bool isHighScore)
        {
            if (isHighScore)
            {
                return;
            }

            // Add to animating views
            Guid animation = Guid.NewGuid();
            gamePresenter.AnimatingViews.Add(animation);

            // Wait
            await UniTask.Delay((int)(MOVE_TWEEN_TIME * 1000) + 100 * gridPosition.y);

            // Tween
            await NumberTween.TweenFloat(0, 1, 0.4f, (value) =>
            {
                imagePropertyBlock.SetFloat("_EffectAmount", value);
                completedPropertyBlock.SetFloat("_EffectAmount", value);
                image.SetPropertyBlock(imagePropertyBlock);
                completedObject.SetPropertyBlock(completedPropertyBlock);
            });

            await UniTask.Delay(150);

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(animation);
        }
    }
}