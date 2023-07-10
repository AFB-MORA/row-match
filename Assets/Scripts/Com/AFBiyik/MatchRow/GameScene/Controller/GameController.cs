using System;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Input;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Com.AFBiyik.MatchRow.GameScene.View;
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
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly IGridPresenter gridPresenter;
        private readonly IGamePresenter gamePresenter;

        /// <summary>
        /// Creates game controller
        /// </summary>
        /// <param name="swipeEvent">Swipe Event</param>
        /// <param name="gridPresenter">Grid Presenter</param>
        public GameController(ISwipeEvent swipeEvent, IGridPresenter gridPresenter, IGamePresenter gamePresenter)
        {
            // Set grid presenter
            this.gridPresenter = gridPresenter;

            // Set game presenter
            this.gamePresenter = gamePresenter;

            // Subscribe swipe event
            swipeEvent.OnSwipe
                .Subscribe(OnSwipe)
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
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

            // Check Has Moves
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
            for (int x = 1; x < gridPresenter.Colums; x++)
            {
                // Get item
                int index = gridPresenter.GetItemIndex(new Vector2Int(x, y));
                var item = gridPresenter.Grid[index];

                // Check item
                if (item != firstItem)
                {
                    return false;
                }
            }

            // All rows same
            return true;
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
            gamePresenter.UpdateScore(firstItem);
        }

    }
}