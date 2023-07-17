using System;
using System.Collections;
using System.Linq;
using Com.AFBiyik.AudioSystem;
using Com.AFBiyik.MatchRow.Global.Popup;
using Com.AFBiyik.MatchRow.Global.Util;
using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using Com.AFBiyik.MatchRow.LevelScene.Input;
using Com.AFBiyik.MatchRow.LevelScene.Presenter;
using Com.AFBiyik.MatchRow.LevelScene.Util;
using Com.AFBiyik.MatchRow.LevelScene.View;
using Com.AFBiyik.PopupSystem;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.LevelScene.Controller
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
        private readonly ISoundController2d soundController;

        // Private Fields
        private IDisposable swipeDisposable;

        /// <summary>
        /// Creates game controller
        /// </summary>
        /// <param name="swipeEvent">Swipe Event</param>
        /// <param name="gridPresenter">Grid Presenter</param>
        public GameController(ISwipeEvent swipeEvent, IGridPresenter gridPresenter, IGamePresenter gamePresenter, IPopupController popupController, ISoundController2d soundController)
        {
            // Set grid presenter
            this.gridPresenter = gridPresenter;

            // Set game presenter
            this.gamePresenter = gamePresenter;

            // Set popup controller
            this.popupController = popupController;

            // Set sound controller
            this.soundController = soundController;

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
            if (nextPosition.x < 0 || nextPosition.x >= gridPresenter.Columns ||
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

            // Play sound
            soundController.PlaySound(new Sound(SoundConstants.SWIPE, volume: 0.2f));

            // Check Rows
            CheckRows();

            // Check Move Count
            if (!MakeMove())
            {
                // Game Over
                GameOver();
            }

            // Check Has Moves
            if (!GridMoveChecker.HasMoves(gridPresenter, gamePresenter.MoveCount.Value))
            {
                // Game Over
                GameOver();
            }

        }

        /// <summary>
        /// Ends the game
        /// </summary>
        private async void GameOver()
        {
            // Dispose swipe
            swipeDisposable?.Dispose();
            swipeDisposable = null;

            // Play sound
            soundController.PlaySound(new Sound(SoundConstants.GAME_OVER, volume: 0.2f));

            // Set game over
            gamePresenter.GameOver();

            // Open game over popup
            Hashtable args = new Hashtable();
            args["isHighScore"] = gamePresenter.IsHighScore;
            args["highScore"] = gamePresenter.HighScore.Value;
            args["score"] = gamePresenter.Score.Value;

            // Wait until animations complete
            if (gamePresenter.AnimatingViews.Count > 0)
            {
                await UniTask.WaitUntil(() => gamePresenter.AnimatingViews.Count == 0);
            }

            // Open popup
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
            // Play sound
            soundController.PlaySound(new Sound(SoundConstants.POP, volume: 0.2f));

            // Set row completed
            gridPresenter.CompleteRow(row);

            // Update score
            gamePresenter.UpdateScore(firstItem, gridPresenter.Columns);
        }
    }
}