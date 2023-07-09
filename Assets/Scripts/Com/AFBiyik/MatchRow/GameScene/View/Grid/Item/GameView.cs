using Com.AFBiyik.MatchRow.GameScene.Enumeration;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
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

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize background
        /// </summary>
        private void Initialize()
        {
            int columns = gridPresenter.Colums;

            for (int i = 0; i < gridPresenter.Grid.Count; i++)
            {
                ItemType itemType = gridPresenter.Grid[i];
                var item = itemFactory.Create(itemType, itemsContent);

                int x = i % columns;
                int y = i / columns;
                item.Initialize(new Vector2Int(x, y));
            }
        }
    }
}
