using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene.Presenter
{
    /// <summary>
    /// Contains grid properties and grid methods
    /// </summary>
    public interface IGridPresenter
    {
        /// <summary>
        /// Number of rows
        /// </summary>
        int Rows { get; }

        /// <summary>
        /// Number of columns
        /// </summary>
        int Colums { get; }

        /// <summary>
        /// Grid cell size in world units
        /// </summary>
        float CellSize { get; }

        /// <summary>
        /// Grid items starting from bottom left
        /// </summary>
        IReadOnlyReactiveCollection<ItemType> Grid { get; }

        /// <summary>
        /// Converts grid position to world position
        /// </summary>
        /// <param name="gridPosition">Grid position (0,0) is bottom left</param>
        /// <returns>World position</returns>
        Vector2 GridPositionToWorldPosition(Vector2Int gridPosition);

        /// <summary>
        /// Get item index from grid position
        /// </summary>
        /// <param name="gridPosition">Grid position</param>
        /// <returns>Item index</returns>
        int GetItemIndex(Vector2Int gridPosition);

        /// <summary>
        /// Get item grid position from item index
        /// </summary>
        /// <param name="index">Item index</param>
        /// <returns>Grid position</returns>
        Vector2Int GetItemGridPosition(int index);

        /// <summary>
        /// Swaps grid items.
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        void SwapItems(int oldIndex, int newIndex);

        /// <summary>
        /// Completes row items in the grid.
        /// </summary>
        /// <param name="row">Row index</param>
        void CompleteRow(int row);
    }
}