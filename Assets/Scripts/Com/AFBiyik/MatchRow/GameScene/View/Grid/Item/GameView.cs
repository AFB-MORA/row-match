using System.Collections.Generic;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    public class GameView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private Transform itemsContent;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;
        [Inject]
        private IFactory<ItemType, Transform, ItemView> itemFactory;

        // Private Fields
        private List<ItemView> items;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize items
        /// </summary>
        private void Initialize()
        {
            items = new List<ItemView>();

            // Foreach item
            for (int i = 0; i < gridPresenter.Grid.Count; i++)
            {
                // Create and initialize item
                ItemType itemType = gridPresenter.Grid[i];
                var item = itemFactory.Create(itemType, itemsContent);
                item.Initialize(gridPresenter.GetItemGridPosition(i));
                items.Add(item);
            }

            gridPresenter.Grid.ObserveMove()
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnMove);

            gridPresenter.Grid.ObserveReplace()
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnReplace);
        }

        /// <summary>
        /// Called when list moves
        /// </summary>
        /// <param name="moveEvent"></param>
        private void OnMove(CollectionMoveEvent<ItemType> moveEvent)
        {
            // Get new grid position
            Vector2Int newGridPosition = gridPresenter.GetItemGridPosition(moveEvent.NewIndex);

            // Move item to new grid position
            items[moveEvent.OldIndex].MoveTo(newGridPosition);

            // Move item view in the list
            var val = items[moveEvent.OldIndex];
            items.RemoveAt(moveEvent.OldIndex);
            items.Insert(moveEvent.NewIndex, val);
        }

        /// <summary>
        /// Called when list item replaces
        /// </summary>
        /// <param name="event"></param>
        private void OnReplace(CollectionReplaceEvent<ItemType> replaceEvent)
        {
            items[replaceEvent.Index].UpdateType(replaceEvent.NewValue);
        }
    }
}
