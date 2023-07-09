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

        /// <summary>
        /// Creates game controller
        /// </summary>
        /// <param name="swipeEvent">Swipe Event</param>
        /// <param name="gridPresenter">Grid Presenter</param>
        public GameController(ISwipeEvent swipeEvent, IGridPresenter gridPresenter)
        {
            // Set grid presenter
            this.gridPresenter = gridPresenter;

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

            // Move first item
            gridPresenter.Grid.Move(itemIndex, nextIndex);

            // Get next index
            int movedNextIndex;
            // If next index is bigger than old index
            if (nextIndex > itemIndex)
            {
                // List is moved forward
                movedNextIndex = nextIndex - 1;
            }
            else
            {
                // List is moved backward
                movedNextIndex = nextIndex + 1;
            }

            // Move second item
            gridPresenter.Grid.Move(movedNextIndex, itemIndex);

            // Check Rows
            CheckRows();
        }

        private void CheckRows()
        {
            for (int y = 0; y < gridPresenter.Rows; y++)
            {
                int firstIndex = gridPresenter.GetItemIndex(new Vector2Int(0, y));
                var firstItem = gridPresenter.Grid[firstIndex];

                if (firstItem != ItemType.Completed)
                {
                    if (CheckRow(y, firstItem))
                    {
                        CompleteRow(y, firstItem);
                    }
                }
            }
        }

        private bool CheckRow(int y, ItemType firstItem)
        {
            for (int x = 1; x < gridPresenter.Colums; x++)
            {
                int index = gridPresenter.GetItemIndex(new Vector2Int(x, y));
                var item = gridPresenter.Grid[index];

                if (item != firstItem)
                {
                    return false;
                }
            }

            return true;
        }

        private void CompleteRow(int y, ItemType firstItem)
        {
            for (int x = 0; x < gridPresenter.Colums; x++)
            {
                int index = gridPresenter.GetItemIndex(new Vector2Int(x, y));
                gridPresenter.Grid[index] = ItemType.Completed;
            }

            // TODO score
        }

    }
}