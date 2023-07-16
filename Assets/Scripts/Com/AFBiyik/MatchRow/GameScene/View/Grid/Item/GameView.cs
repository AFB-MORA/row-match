using System.Collections.Generic;
using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Com.AFBiyik.MatchRow.GameScene.Util;
using Com.AFBiyik.MatchRow.GameScene.VFX;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.View
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

            // Subscribe to grid
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
        private async void OnReplace(CollectionReplaceEvent<ItemType> replaceEvent)
        {
            // Add to animating views
            gamePresenter.AnimatingViews.Add(gameObject);

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

            // Remove from animating views
            gamePresenter.AnimatingViews.Remove(gameObject);
        }
    }
}
