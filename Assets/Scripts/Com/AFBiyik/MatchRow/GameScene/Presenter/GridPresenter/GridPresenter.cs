using System.Linq;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Util;
using Com.AFBiyik.MatchRow.LevelSystem;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene.Presenter
{
    /// <inheritdoc/>
    public class GridPresenter : IGridPresenter
    {
        // Private Readonly Fields
        private readonly float cellSize;
        private readonly int rows;
        private readonly int columns;
        private readonly ReactiveCollection<ItemType> grid;
        private readonly Vector2 origin;

        /// <inheritdoc/>
        public ReactiveCollection<ItemType> Grid => grid;

        /// <inheritdoc/>
        public int Rows => rows;

        /// <inheritdoc/>
        public int Colums => columns;

        /// <inheritdoc/>
        public float CellSize => cellSize;

        /// <summary>
        /// Creates grid presenter with level model and bounding rect
        /// </summary>
        /// <param name="levelModel"></param>
        public GridPresenter(LevelModel levelModel, Rect rect)
        {
            // Initialize grid
            grid = new ReactiveCollection<ItemType>(levelModel.Grid
                .Select(i => i.ToItemType()));

            // Get columns and rows
            columns = levelModel.GridWidth;
            rows = levelModel.GridHeight;

            // Calculate cell size
            float cellWidth = rect.size.x / columns;
            float cellHeight = rect.size.y / rows;
            cellSize = Mathf.Min(cellWidth, cellHeight);

            // Calculate origin
            float width = columns * cellSize;
            float height = rows * cellSize;
            origin = rect.position - new Vector2(width / 2f, height / 2f);
        }

        /// <inheritdoc/>
        public Vector2 GridPositionToWorldPosition(Vector2Int gridPosition)
        {
            float x = gridPosition.x * cellSize + origin.x;
            float y = gridPosition.y * cellSize + origin.y;

            return new Vector2(x, y);
        }

        /// <inheritdoc/>
        public int GetItemIndex(Vector2Int gridPosition)
        {
            return gridPosition.y * columns + gridPosition.x;
        }

        /// <inheritdoc/>
        public Vector2Int GetItemGridPosition(int index)
        {
            int x = index % columns;
            int y = index / columns;
            return new Vector2Int(x, y);
        }
    }
}