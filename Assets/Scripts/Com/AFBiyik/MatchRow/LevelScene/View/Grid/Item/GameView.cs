using System.Collections.Generic;
using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using Com.AFBiyik.MatchRow.LevelScene.Presenter;
using Com.AFBiyik.MatchRow.LevelScene.Util;
using Com.AFBiyik.MatchRow.LevelScene.VFX;
using Com.AFBiyik.UIComponents;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.LevelScene.View
{
    public class GameView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private Transform scoreView;
        [SerializeField]
        private Transform itemsContent;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;
        [Inject]
        private IFactory<ItemType, Transform, ItemView> itemFactory;
        [Inject]
        private IGamePresenter gamePresenter;
        [Inject]
        private IMemoryPool<CompletedEffect> completedEffectPool;
        [Inject]
        private BoundsView boundsView;

        // Private Fields
        private List<ItemView> items;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Updates grid
        /// </summary>
        private void UpdateGrid()
        {
            // Update items
            foreach (var item in items)
            {
                item.UpdateSize();
            }
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

            // Subscribe to grid
            gridPresenter.Grid.ObserveMove()
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnMove);

            gridPresenter.Grid.ObserveReplace()
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnReplace);

            // Subscribe dimention change
            boundsView.onBoundsChange.AddListener(UpdateGrid);
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
        private async void OnReplace(CollectionReplaceEvent<ItemType> replaceEvent)
        {
            // Get item
            var item = items[replaceEvent.Index];
            // Update item
            item.UpdateType(replaceEvent.NewValue);

            // Get grid position
            Vector2Int gridPos = gridPresenter.GetItemGridPosition(replaceEvent.Index);

            // Get position
            float size2 = gridPresenter.CellSize / 2f;
            Vector2 pos = gridPresenter.GridPositionToWorldPosition(gridPos) + new Vector2(size2, size2);

            // Get effect
            var effect = completedEffectPool.Spawn();

            // Get old item type
            ItemType itemType = replaceEvent.OldValue;

            // Get colors
            (Color from, Color to) = GameConstants.GetColors(itemType);

            // Get score
            int score = GameConstants.GetScore(itemType);

            // Start effect
            await effect.Show(from, to, pos, scoreView.position, score);

            // Despawn effect
            completedEffectPool.Despawn(effect);
        }
    }
}
