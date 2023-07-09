using System;
using Com.AFBiyik.MatchRow.GameScene.Input;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Com.AFBiyik.MatchRow.GameScene.View;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.GameScene.Controller
{
    public class GameController : IDisposable
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly IGridPresenter gridPresenter;

        public GameController(ISwipeEvent swipeEvent, IGridPresenter gridPresenter)
        {
            this.gridPresenter = gridPresenter;

            swipeEvent.OnSwipe
                .Subscribe(OnSwipe)
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        private void OnSwipe(SwipeModel input)
        {
            var hit = Physics2D.Raycast(input.StartPosition, Vector2.zero);

            if (hit.collider != null)
            {
                var item = hit.collider.GetComponent<ItemView>();
                SwipeItem(item, input.Direction);
            }
        }

        private void SwipeItem(ItemView item, SwipeDirection direction)
        {
            Vector2Int gridPosition = item.GridPosition;
            Vector2Int nextPosition = new Vector2Int(-1, -1);

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

            if (nextPosition.x < 0 || nextPosition.x >= gridPresenter.Colums ||
                nextPosition.y < 0 || nextPosition.y >= gridPresenter.Rows)
            {
                return;
            }

            int itemIndex = gridPresenter.GetItemIndex(gridPosition);
            int nextIndex = gridPresenter.GetItemIndex(nextPosition);

            gridPresenter.Grid.Move(itemIndex, nextIndex);

            int movedNextIndex;
            if (nextIndex > itemIndex)
            {
                movedNextIndex = nextIndex - 1;
            }
            else
            {
                movedNextIndex = nextIndex + 1;
            }

            gridPresenter.Grid.Move(movedNextIndex, itemIndex);
        }
    }
}