using System.Collections.Generic;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using UniRx;
using UnityEngine;
using Zenject;
using static UnityEditor.Progress;

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
        }

        private void OnMove(CollectionMoveEvent<ItemType> moveEvent)
        {
            Vector2Int newGridPosition = gridPresenter.GetItemGridPosition(moveEvent.NewIndex);
            items[moveEvent.OldIndex].MoveTo(newGridPosition);

            var val = items[moveEvent.OldIndex];
            items.RemoveAt(moveEvent.OldIndex);
            items.Insert(moveEvent.NewIndex, val);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].transform.SetSiblingIndex(i);
            }

        }
    }
}
