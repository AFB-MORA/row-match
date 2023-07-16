using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.View
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

        // Private fields
        private Vector2Int gridPosition;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;
        [Inject]
        private IGamePresenter gamePresenter;

        // Public Properties
        public ItemType ItemType => itemType;
        public Vector2Int GridPosition => gridPosition;

        /// <summary>
        /// Initializes item
        /// </summary>
        /// <param name="gridPosition"></param>
        public void Initialize(Vector2Int gridPosition)
        {
            // Set grid position
            this.gridPosition = gridPosition;

            // Set object name
            gameObject.name = $"({gridPosition.x}, {gridPosition.y})";

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
            gamePresenter.AnimatingViews.Add(gameObject);

            // Set grid position
            this.gridPosition = gridPosition;

            // Set position
            var newPosition = gridPresenter.GridPositionToWorldPosition(gridPosition);
            await transform.DOMove(newPosition, MOVE_TWEEN_TIME);

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(gameObject);
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
    }
}