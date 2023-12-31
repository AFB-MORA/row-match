using System.Collections.Generic;
using System.Linq;
using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using Com.AFBiyik.MatchRow.LevelScene.Presenter;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.LevelScene.Util
{
    public static class GridMoveChecker
    {
        /// <summary>
        /// Chech is there any move
        /// </summary>
        /// <returns>True if has moves; otherwise false.</returns>
        public static bool HasMoves(IGridPresenter gridPresenter, int moveCount)
        {
            // Get isolated items
            var isolatedItems = GetIsolatedItems(gridPresenter);

            // Check has enough items to complete row
            if (!HasEnoughItems(isolatedItems, gridPresenter))
            {
                return false;
            }

            // Check can move items to row with row count
            if (!CanCompleteRow(isolatedItems, gridPresenter, moveCount))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Split isolated groups by completed items
        /// </summary>
        /// <param name="gridPresenter"></param>
        /// <returns></returns>
        private static List<List<ItemType>> GetIsolatedItems(IGridPresenter gridPresenter)
        {
            // Isolate groups divided by completed rows
            List<List<ItemType>> isolatedItems = new List<List<ItemType>>();
            bool hasGroup = false;
            int lastIsolatedGroup = -1;

            // For each row
            for (int y = 0; y < gridPresenter.Rows; y++)
            {
                // Get fist item
                int firstIndex = gridPresenter.GetItemIndex(new Vector2Int(0, y));
                var firstItem = gridPresenter.Grid[firstIndex];

                // if not completed
                if (firstItem != ItemType.Completed)
                {
                    // if no current group
                    if (!hasGroup)
                    {
                        hasGroup = true;
                        isolatedItems.Add(new List<ItemType>());
                        lastIsolatedGroup++;
                    }

                    // add row to group
                    isolatedItems[lastIsolatedGroup].AddRange(gridPresenter.GetItemsAtRow(y));
                }
                else
                {
                    hasGroup = false;
                }
            }

            return isolatedItems;
        }

        /// <summary>
        /// Checks is there enough items to complete the row.
        /// </summary>
        /// <returns>True if there is enough item; otherwise false.</returns>
        private static bool HasEnoughItems(List<List<ItemType>> isolatedItems, IGridPresenter gridPresenter)
        {
            // For each isolated group
            return isolatedItems.Select(
                    // Group by item type
                    list => list.GroupBy(item => item)
                    // Map item type to count in isolated group
                    .Select(group => (item: group.Key, count: group.Count()))
                // For each isolated group
                ).Any(
                    // Check is there enough item to complete row
                    list => list.Any(group => group.count >= gridPresenter.Columns)
                );
        }

        /// <summary>
        /// Check can complete the row within move count
        /// </summary>
        /// <returns>True if can complete; otherwise false</returns>
        private static bool CanCompleteRow(List<List<ItemType>> isolatedItems, IGridPresenter gridPresenter, int moveCount)
        {
            // Get columns
            int columns = gridPresenter.Columns;

            // Get solution groups with item count
            List<(List<ItemType> list, Dictionary<ItemType, int> counts)> solutionGroups = new List<(List<ItemType> list, Dictionary<ItemType, int> counts)>();
            for (int i = 0; i < isolatedItems.Count; i++)
            {
                // Map item list and item type to count
                var list = isolatedItems[i];

                // Group by item type
                Dictionary<ItemType, int> counts = new Dictionary<ItemType, int>();
                for (int j = 0; j < list.Count; j++)
                {
                    // Map item type to count in isolated group
                    var item = list[j];
                    if (!counts.ContainsKey(item))
                    {
                        counts[item] = 0;
                    }

                    counts[item]++;
                }

                // Check can row be made
                bool found = false;
                foreach (var pair in counts)
                {
                    if (pair.Value >= columns)
                    {
                        found = true;
                        break;
                    }
                }

                // If solution
                if (found)
                {
                    solutionGroups.Add((list, counts));
                }
            }


            // For each isolated group that has solution
            foreach (var solutionGroup in solutionGroups)
            {
                // Check solution group
                if (CanCompleteSolutionGroup(solutionGroup, columns, moveCount))
                {
                    // If solution
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check can complete solution group
        /// </summary>
        /// <param name="solutionGroup"></param>
        /// <param name="columns"></param>
        /// <param name="moveCount"></param>
        /// <returns>True if can complete; otherwise false</returns>
        private static bool CanCompleteSolutionGroup((List<ItemType> list, Dictionary<ItemType, int> counts) solutionGroup, int columns, int moveCount)
        {
            // Calculate rows (at least 2)
            int rows = solutionGroup.list.Count / columns;

            // Initialize sub grid
            ItemType[,] subGrid = new ItemType[columns, rows];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    subGrid[x, y] = solutionGroup.list[x + y * columns];
                }
            }

            // Find items that can be solution
            List<ItemType> solutionItems = new List<ItemType>();
            foreach (var pair in solutionGroup.counts)
            {
                if (pair.Value >= columns)
                {
                    solutionItems.Add(pair.Key);
                }
            }

            // For each items
            // Find min number of moves to complete the row
            foreach (var item in solutionItems)
            {
                // Check for item
                if (CanCompleteSolutionGroupForItem(item, subGrid, rows, columns, moveCount))
                {
                    // If solution
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check can complete solution group for item type
        /// </summary>
        /// <param name="item">Item type</param>
        /// <param name="subGrid">Sub grid</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="moveCount">Move count</param>
        /// <returns>True if can complete; otherwise false</returns>
        private static bool CanCompleteSolutionGroupForItem(ItemType item, ItemType[,] subGrid, int rows, int columns, int moveCount)
        {
            // Copy sub grid
            // Find row item counts
            ItemType[,] cloneSubGrid = new ItemType[columns, rows];
            List<(int itemCount, int index)> rowItemCounts = new List<(int itemCount, int index)>(rows);
            for (int y = 0; y < rows; y++)
            {
                int count = 0;

                for (int x = 0; x < columns; x++)
                {
                    cloneSubGrid[x, y] = subGrid[x, y];

                    if (cloneSubGrid[x, y] == item)
                    {
                        count++;
                    }
                }

                rowItemCounts.Add((count, y));
            }

            // Find max item count
            var maxItemCount = rowItemCounts.Max(tuple => tuple.itemCount);

            // Find row with max item count
            int targetY = rowItemCounts
                .First(tuple => tuple.itemCount == maxItemCount).index;


            int totalMoves = 0;
            // Foreach missing piece
            for (int x = 0; x < columns; x++)
            {
                // If item at column is not target item
                if (cloneSubGrid[x, targetY] != item)
                {
                    // Find closest point
                    totalMoves += GetClosestItemDistance(cloneSubGrid, item, x, targetY, rows, columns);
                }
            }

            // We find the solution within the move count
            return totalMoves <= moveCount;
        }

        /// <summary>
        /// Get closest item distance with type target starting from x and y.
        /// </summary>
        /// <param name="subGrid">Subgrid</param>
        /// <param name="target">Item target</param>
        /// <param name="x">Starting x position</param>
        /// <param name="y">Starting y position</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of coulumns</param>
        /// <returns>Distance</returns>
        private static int GetClosestItemDistance(ItemType[,] subGrid, ItemType target, int x, int y, int rows, int columns)
        {
            // Get item psoition
            var closestPosition = GetClosestItemPosition(subGrid, target, x, y, rows, columns);

            // Check position
            if (closestPosition.x < 0 || closestPosition.y < 0)
            {
                return int.MaxValue;
            }

            // Update point
            subGrid[closestPosition.x, closestPosition.y] = ItemType.Completed;

            // Get distance
            return Mathf.Abs(x - closestPosition.x) + Mathf.Abs(y - closestPosition.y);

        }

        /// <summary>
        /// Get closest item position type target starting from x and y.
        /// </summary>
        /// <param name="subGrid">Subgrid</param>
        /// <param name="target">Item target</param>
        /// <param name="x">Starting x position</param>
        /// <param name="y">Starting y position</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of coulumns</param>
        /// <returns>Position</returns>
        private static Vector2Int GetClosestItemPosition(ItemType[,] subGrid, ItemType target, int x, int y, int rows, int columns)
        {
            // Get max distance
            int maxDistance = rows + columns - 1;
            // Not move row
            int notY = y;

            // Until max distance circle
            for (int d = 1; d < maxDistance; d++)
            {
                // Define closest points
                int xClosest;
                int yClosest;

                // For distance
                for (int i = 0; i < d + 1; i++)
                {
                    // Test point
                    if (TestPoint(x - d + i, y - i, notY, subGrid, target, rows, columns, out xClosest, out yClosest))
                    {
                        // Found
                        return new Vector2Int(xClosest, yClosest);
                    }

                    // Test point
                    if (TestPoint(x + d - i, y + i, notY, subGrid, target, rows, columns, out xClosest, out yClosest))
                    {
                        // Found
                        return new Vector2Int(xClosest, yClosest);
                    }
                }

                // For distance
                for (int i = 1; i < d; i++)
                {
                    // Test point
                    if (TestPoint(x - i, y + d - i, notY, subGrid, target, rows, columns, out xClosest, out yClosest))
                    {
                        // Found
                        return new Vector2Int(xClosest, yClosest);
                    }

                    // Test point
                    if (TestPoint(x + i, y - d + i, notY, subGrid, target, rows, columns, out xClosest, out yClosest))
                    {
                        // Found
                        return new Vector2Int(xClosest, yClosest);
                    }
                }
            }

            // Not found
            // Algorithm should not come here
            // We guarantee there is a solution
            Debug.LogWarning("Should not come here");
            return new Vector2Int(-1, -1);
        }

        /// <summary>
        /// Test point is target point
        /// </summary>
        /// <param name="xTest">X position to test</param>
        /// <param name="yTest">Y position to test</param>
        /// <param name="notY">Not select items in this row</param>
        /// <param name="subGrid">Subgrid</param>
        /// <param name="target">Item target</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of coulumns</param>
        /// <param name="xClosest">Closest point x position</param>
        /// <param name="yClosest">Closest point y position</param>
        /// <returns>True if point found; otherwise false</returns>
        private static bool TestPoint(int xTest, int yTest, int notY, ItemType[,] subGrid, ItemType target, int rows, int columns, out int xClosest, out int yClosest)
        {
            // Assign closest point
            xClosest = -1;
            yClosest = -1;

            // Check point
            if (IsValidPosition(xTest, yTest, notY, rows, columns) && CheckPoint(subGrid, target, xTest, yTest))
            {
                // Found
                xClosest = xTest;
                yClosest = yTest;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check point is target
        /// </summary>
        /// <param name="subGrid">Subgrid</param>
        /// <param name="target">Item target</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>True if item is target item; otherwise false.</returns>
        private static bool CheckPoint(ItemType[,] subGrid, ItemType target, int x, int y)
        {
            return subGrid[x, y] == target;
        }

        /// <summary>
        /// Check is position valid
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="notY">Not select items in this row</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of coulumns</param>
        /// <returns>True if position is valid; otherwise false.</returns>
        private static bool IsValidPosition(int x, int y, int notY, int rows, int columns)
        {
            return x >= 0 && x < columns && y != notY && y >= 0 && y < rows;
        }
    }
}