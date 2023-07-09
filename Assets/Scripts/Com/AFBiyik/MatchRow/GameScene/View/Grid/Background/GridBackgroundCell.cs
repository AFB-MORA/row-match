using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene
{
    /// <summary>
    /// Background cell in the grid.
    /// </summary>
    public class GridBackgroundCell : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private SpriteRenderer image;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;

        /// <summary>
        /// Initialized cell with grid position.
        /// <br/>
        /// Calculates position according to grid position.
        /// </summary>
        /// <param name="gridPosition">Bottom left (0,0)</param>
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
