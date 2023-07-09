using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene
{
    public class ItemView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private ItemType itemType;
        [SerializeField]
        private SpriteRenderer image;
 
        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;

        public ItemType ItemType => itemType;

        public void Initialize(Vector2Int gridPosition)
        {
            // Set object name
            gameObject.name = $"({gridPosition.x}, {gridPosition.y})";

            // Set position
            transform.position = gridPresenter.GridPositionToWorldPosition(gridPosition);

            // Set cell size
            image.size = new Vector2(gridPresenter.CellSize, gridPresenter.CellSize);
        }
    }
}