using System.Collections.Generic;
using System.Linq;
using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using Com.AFBiyik.MatchRow.LevelScene.Util;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using UniRx;
using UnityEngine;
using Com.AFBiyik.UIComponents;

namespace Com.AFBiyik.MatchRow.LevelScene.Presenter
{
    /// <inheritdoc/>
    public class GridPresenter : IGridPresenter
    {
        // Private Readonly Fields
        private readonly int rows;
        private readonly int columns;
        private readonly ReactiveCollection<ItemType> grid;
        private readonly BoundsView boundsView;

        // Private Fields
        private float cellSize;
        private Vector2 origin;

        // Public properties
        /// <inheritdoc/>
        public IReadOnlyReactiveCollection<ItemType> Grid => grid;

        /// <inheritdoc/>
        public int Rows => rows;

        /// <inheritdoc/>
        public int Columns => columns;

        /// <inheritdoc/>
        public float CellSize => cellSize;

        /// <inheritdoc/>
        public Rect Rect => new Rect(origin, new Vector2(Columns * CellSize, Rows * CellSize));

        /// <summary>
        /// Creates grid presenter with level model and bounding rect
        /// </summary>
        /// <param name="levelModel"></param>
        /// <param name="boundsView"></param>
        public GridPresenter(LevelModel levelModel, BoundsView boundsView)
        {
            // Set bounds view
            this.boundsView = boundsView;
            // Subscribe bound change
            boundsView.onBoundsChange.AddListener(SetRect);

            // Initialize grid
            grid = new ReactiveCollection<ItemType>(levelModel.Grid
                .Select(i => i.ToItemType()));

            // Get columns and rows
            columns = levelModel.GridWidth;
            rows = levelModel.GridHeight;

            // Set rect
            SetRect();
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
            for (int x = 0; x < Columns; x++)
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
            for (int x = 0; x < Columns; x++)
            {
                // Get item
                int index = GetItemIndex(new Vector2Int(x, row));
                var item = Grid[index];
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Sets rect
        /// </summary>
        private void SetRect()
        {
            // Get rect
            var rect = boundsView.Rect;

            // Calculate cell size
            float cellWidth = rect.size.x / columns;
            float cellHeight = rect.size.y / rows;
            cellSize = Mathf.Min(cellWidth, cellHeight);

            // Calculate origin
            float width = columns * cellSize;
            float height = rows * cellSize;
            origin = rect.position - new Vector2(width / 2f, height / 2f);
        }
    }
}