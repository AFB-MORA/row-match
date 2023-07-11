using System.Collections.Generic;
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

        // Public properties
        /// <inheritdoc/>
        public IReadOnlyReactiveCollection<ItemType> Grid => grid;

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

        /// <inheritdoc/>
        public void SwapItems(int oldIndex, int newIndex)
        {
            // Move first item
            grid.Move(oldIndex, newIndex);

            // Get move index
            int movedNextIndex;
            // If new index is bigger than old index
            if (newIndex > oldIndex)
            {
                // List is moved forward
                movedNextIndex = newIndex - 1;
            }
            else
            {
                // List is moved backward
                movedNextIndex = newIndex + 1;
            }

            // Move second item
            grid.Move(movedNextIndex, oldIndex);
        }

        /// <inheritdoc/>
        public void CompleteRow(int row)
        {
            // For each column
            for (int x = 0; x < Colums; x++)
            {
                // Get index
                int index = GetItemIndex(new Vector2Int(x, row));
                // Complete item
                grid[index] = ItemType.Completed;
            }
        }

        /// <inheritdoc/>
        public List<ItemType> GetItemsAtRow(int row)
        {
            List<ItemType> items = new List<ItemType>();

            // For each column
            for (int x = 0; x < Colums; x++)
            {
                // Get item
                int index = GetItemIndex(new Vector2Int(x, row));
                var item = Grid[index];
                items.Add(item);
            }

            return items;
        }
    }
}