using System.Collections.Generic;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene
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
        List<ItemType> Grid { get; }

        /// <summary>
        /// Converts grid position to world position
        /// </summary>
        /// <param name="gridPosition">Grid position (0,0) is bottom left</param>
        /// <returns>World position</returns>
        Vector2 GridPositionToWorldPosition(Vector2Int gridPosition);
    }
}