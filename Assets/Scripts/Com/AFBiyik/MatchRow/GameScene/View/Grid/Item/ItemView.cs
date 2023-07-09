using System;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    public class ItemView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private ItemType itemType;
        [SerializeField]
        private SpriteRenderer image;
        [SerializeField]
        private CircleCollider2D circleCollider;

        // Private fields
        private Vector2Int gridPosition;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;

        // Public Properties
        public ItemType ItemType => itemType;
        public Vector2Int GridPosition => gridPosition;

        public void Initialize(Vector2Int gridPosition)
        {
            // Set grid position
            this.gridPosition = gridPosition;

            // Set object name
            gameObject.name = $"({gridPosition.x}, {gridPosition.y})";

            // Set position
            transform.position = gridPresenter.GridPositionToWorldPosition(gridPosition);

            // Set cell size
            image.size = new Vector2(gridPresenter.CellSize, gridPresenter.CellSize);
            circleCollider.radius = gridPresenter.CellSize / 2f;
            circleCollider.offset = new Vector2(circleCollider.radius, circleCollider.radius);
        }

        public void MoveTo(Vector2Int gridPosition)
        {
            // Set grid position
            this.gridPosition = gridPosition;

            // Set object name
            //gameObject.name = $"({gridPosition.x}, {gridPosition.y})";

            // Set position
            transform.position = gridPresenter.GridPositionToWorldPosition(gridPosition);
        }
    }
}