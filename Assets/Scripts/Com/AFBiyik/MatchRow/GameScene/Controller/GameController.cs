using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Input;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Com.AFBiyik.MatchRow.GameScene.View;
using Com.AFBiyik.MatchRow.Global.Popup;
using Com.AFBiyik.PopupSystem;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene.Controller
{
    /// <summary>
    /// Controls gameplay
    /// </summary>
    public class GameController : IDisposable
    {
        // Private Readonly Fields
        private readonly IGridPresenter gridPresenter;
        private readonly IGamePresenter gamePresenter;
        private readonly IPopupController popupController;

        // Private Fields
        private IDisposable swipeDisposable;

        /// <summary>
        /// Creates game controller
        /// </summary>
        /// <param name="swipeEvent">Swipe Event</param>
        /// <param name="gridPresenter">Grid Presenter</param>
        public GameController(ISwipeEvent swipeEvent, IGridPresenter gridPresenter, IGamePresenter gamePresenter, IPopupController popupController)
        {
            // Set grid presenter
            this.gridPresenter = gridPresenter;

            // Set game presenter
            this.gamePresenter = gamePresenter;

            // Set popup controller
            this.popupController = popupController;

            // Subscribe swipe event
            swipeDisposable = swipeEvent.OnSwipe
                .Subscribe(OnSwipe);
        }

        public void Dispose()
        {
            swipeDisposable?.Dispose();
        }

        /// <summary>
        /// Called when swipe
        /// </summary>
        /// <param name="input"></param>
        private void OnSwipe(SwipeModel input)
        {
            // Get gits
            var hit = Physics2D.Raycast(input.StartPosition, Vector2.zero);

            // If hit
            if (hit.collider != null)
            {
                // Swipe items
                var item = hit.collider.GetComponent<ItemView>();
                SwipeItem(item, input.Direction);
            }
        }

        /// <summary>
        /// Swipes item 
        /// </summary>
        /// <param name="item">Item to swipe</param>
        /// <param name="direction">Swipe direction</param>
        private void SwipeItem(ItemView item, SwipeDirection direction)
        {
            // Check if completed
            if (item.ItemType == ItemType.Completed)
            {
                return;
            }

            // Get positions
            Vector2Int gridPosition = item.GridPosition;
            Vector2Int nextPosition = new Vector2Int(-1, -1);

            // Get next position
            switch (direction)
            {
                case SwipeDirection.Up:
                    nextPosition = gridPosition + new Vector2Int(0, 1);
                    break;
                case SwipeDirection.Down:
                    nextPosition = gridPosition + new Vector2Int(0, -1);
                    break;
                case SwipeDirection.Left:
                    nextPosition = gridPosition + new Vector2Int(-1, 0);
                    break;
                case SwipeDirection.Right:
                    nextPosition = gridPosition + new Vector2Int(1, 0);
                    break;
            }

            // Check next position
            if (nextPosition.x < 0 || nextPosition.x >= gridPresenter.Colums ||
                nextPosition.y < 0 || nextPosition.y >= gridPresenter.Rows)
            {
                return;
            }

            // Get indicies
            int itemIndex = gridPresenter.GetItemIndex(gridPosition);
            int nextIndex = gridPresenter.GetItemIndex(nextPosition);

            // Check if next completed
            ItemType nextItem = gridPresenter.Grid[nextIndex];
            if (nextItem == ItemType.Completed)
            {
                return;
            }

            // Swap grid items
            gridPresenter.SwapItems(itemIndex, nextIndex);

            // Check Rows
            CheckRows();

            // Check Move Count
            if (!MakeMove())
            {
                // Game Over
                GameOver();
            }

            // Check Has Moves
            if (!HasMoves())
            {
                // Game Over
                GameOver();
            }

        }

        /// <summary>
        /// Ends the game
        /// </summary>
        private void GameOver()
        {
            swipeDisposable?.Dispose();
            swipeDisposable = null;

            // Open game over popup
            Hashtable args = new Hashtable();
            args["isHighScore"] = gamePresenter.IsHighScore;
            args["highScore"] = gamePresenter.Score.Value;
            args["score"] = gamePresenter.HighScore.Value;
            popupController.Open(PopupConstants.GAME_OVER_POPUP, args);
        }

        /// <summary>
        /// Decreases move count.
        /// </summary>
        /// <returns>True if has move; otherwise false.</returns>
        private bool MakeMove()
        {
            // Make move
            return gamePresenter.MakeMove();
        }

        /// <summary>
        /// Checks grid for completed rows
        /// </summary>
        private void CheckRows()
        {
            // For each row
            for (int y = 0; y < gridPresenter.Rows; y++)
            {
                // Get fist item
                int firstIndex = gridPresenter.GetItemIndex(new Vector2Int(0, y));
                var firstItem = gridPresenter.Grid[firstIndex];

                // If not completed and row is completed
                if (firstItem != ItemType.Completed && CheckRow(y, firstItem))
                {
                    // Complete row
                    CompleteRow(y, firstItem);
                }
            }
        }

        /// <summary>
        /// Checks all row has same item as first item.
        /// </summary>
        /// <param name="y">Row index</param>
        /// <param name="firstItem">Fist item in the row</param>
        /// <returns></returns>
        private bool CheckRow(int y, ItemType firstItem)
        {
            // For each column
            // Check item
            return gridPresenter.GetItemsAtRow(y).All(i => i == firstItem);
        }

        /// <summary>
        /// Complete row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="firstItem"></param>
        private void CompleteRow(int row, ItemType firstItem)
        {
            // Set row completed
            gridPresenter.CompleteRow(row);

            // Update score
            gamePresenter.UpdateScore(firstItem, gridPresenter.Colums);
        }


        /// <summary>
        /// Chech is there any move
        /// </summary>
        /// <returns>True if has moves; otherwise false.</returns>
        private bool HasMoves()
        {
            // Get isolated items
            var isolatedItems = GetIsolatedItems();

            // Check has enough items to complete row
            if (!HasEnoughItems(isolatedItems))
            {
                return false;
            }

            // Check can move items to row with row count
            if (!CanCompleteRow(isolatedItems))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks is there enough items to complete the row.
        /// </summary>
        /// <returns>True if there is enough item; otherwise false.</returns>
        private bool HasEnoughItems(List<List<ItemType>> isolatedItems)
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
                    list => list.Any(group => group.count >= gridPresenter.Colums)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanCompleteRow(List<List<ItemType>> isolatedItems)
        {
            int minMove = int.MaxValue;

            int columns = gridPresenter.Colums;

            var solutionGroups = isolatedItems
                // Map item list and item type to count
                .Select(list =>
                    (list, counts:
                    // Group by item type
                    list.GroupBy(item => item)
                    // Map item type to count in isolated group
                    .Select(group => (item: group.Key, count: group.Count())))
                    )
                .Where(enumerated => enumerated.counts.Any(group => group.count >= columns));

            // For each isolated group that has solution
            foreach (var solutionGroup in solutionGroups)
            {
                // Calculate rows (at least 2)
                int rows = solutionGroup.list.Count / columns;

                // Initialize sub grid
                List<List<ItemType>> subGrid = new List<List<ItemType>>(rows);
                for (int y = 0; y < rows; y++)
                {
                    subGrid.Add(new List<ItemType>(columns));

                    for (int x = 0; x < columns; x++)
                    {
                        subGrid[y].Add(solutionGroup.list[x + y * columns]);
                    }
                }

                // Find items that can be solution
                var solutionItems = solutionGroup.counts.Where(group => group.count >= columns)
                    .Select(tuple => tuple.item);

                // For each items
                // Find min number of moves to complete the row
                foreach (var item in solutionItems)
                {
                    List<List<ItemType>> cloneSubGrid = subGrid.Select(list => list.Select(item => item).ToList()).ToList();

                    // Find row item counts
                    var rowItemCounts = cloneSubGrid
                        .Select((row, index) => (itemCount:
                            row.Count(rowItem => rowItem == item),
                            index)
                        );

                    // Find max item count
                    var maxItemCount = rowItemCounts.Max(tuple => tuple.itemCount);

                    // Find row with max item count
                    int y = rowItemCounts
                        .First(tuple => tuple.itemCount == maxItemCount).index;


                    int totalMoves = 0;
                    // Foreach missing piece
                    for (int x = 0; x < columns; x++)
                    {
                        // If item at column is not target item
                        if (cloneSubGrid[y][x] != item)
                        {
                            // Find closest point
                            totalMoves += GetClosestItem(cloneSubGrid, item, x, y, rows, columns);
                        }

                    }

                    // If new min moves
                    if (totalMoves < minMove)
                    {
                        minMove = totalMoves;
                    }

                    // We find the solution within the move count
                    if (minMove <= gamePresenter.MoveCount.Value)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        private int GetClosestItem(List<List<ItemType>> subGrid, ItemType target, int x, int y, int rows, int columns)
        {

            List<List<bool>> visited = new List<List<bool>>(rows);

            for (int yy = 0; yy < rows; yy++)
            {
                visited.Add(new List<bool>(columns));

                for (int xx = 0; xx < columns; xx++)
                {
                    visited[yy].Add(false);
                }
            }

            GetClosestItemRecursive(subGrid, visited, target, x, y, y, rows, columns, out int count);
            return count;
        }

        private bool GetClosestItemRecursive(List<List<ItemType>> subGrid, List<List<bool>> visited, ItemType target, int x, int y, int notY, int rows, int columns, out int count)
        {
            count = 0;

            if (visited[y][x])
            {
                return false;
            }

            visited[y][x] = true;

            if (subGrid[y][x] == target)
            {
                subGrid[y][x] = ItemType.Completed;
                return true;
            }

            // up
            if (y + 1 < rows && y + 1 != notY && GetClosestItemRecursive(subGrid, visited, target, x, y + 1, notY, rows, columns, out count))
            {
                count++;
                return true;
            }
            // down
            else if (y - 1 >= 0 && y + 1 != notY && GetClosestItemRecursive(subGrid, visited, target, x, y - 1, notY, rows, columns, out count))
            {
                count++;
                return true;
            }
            // right
            else if (x + 1 < columns && y != notY && GetClosestItemRecursive(subGrid, visited, target, x + 1, y, notY, rows, columns, out count))
            {
                count++;
                return true;
            }
            // left
            else if (x - 1 >= 0 && y != notY && GetClosestItemRecursive(subGrid, visited, target, x - 1, y, notY, rows, columns, out count))
            {
                count++;
                return true;
            }

            return false;
        }

        private List<List<ItemType>> GetIsolatedItems()
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
    }
}