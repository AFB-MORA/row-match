using System.Collections.Generic;
using System.Linq;
using Com.AFBiyik.MatchRow.LevelSystem;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene
{
    /// <inheritdoc/>
    public class GridPresenter : IGridPresenter
    {
        // Private Readonly Fields
        private readonly float cellSize;
        private readonly int rows;
        private readonly int colums;
        private readonly List<ItemType> grid;
        private readonly Vector2 origin;

        /// <inheritdoc/>
        public List<ItemType> Grid => grid;

        /// <inheritdoc/>
        public int Rows => rows;

        /// <inheritdoc/>
        public int Colums => colums;

        /// <inheritdoc/>
        public float CellSize => cellSize;

        /// <summary>
        /// Creates grid presenter with level model and bounding rect
        /// </summary>
        /// <param name="levelModel"></param>
        public GridPresenter(LevelModel levelModel, Rect rect)
        {
            // Initialize grid
            grid = levelModel.Grid
                .Select(i => i.ToItemType())
                .ToList();

            // Get columns and rows
            colums = levelModel.GridWidth;
            rows = levelModel.GridHeight;

            // Calculate cell size
            float cellWidth = rect.size.x / colums;
            float cellHeight = rect.size.y / rows;
            cellSize = Mathf.Min(cellWidth, cellHeight);

            // Calculate origin
            float width = colums * cellSize;
            float height = colums * cellSize;
            origin = rect.position - new Vector2(width / 2f, height / 2f);
        }

        /// <inheritdoc/>
        public Vector2 GridPositionToWorldPosition(Vector2Int gridPosition)
        {
            float x = gridPosition.x * cellSize + origin.x;
            float y = gridPosition.y * cellSize + origin.y;

            return new Vector2(x, y);
        }
    }
}