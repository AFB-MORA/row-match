using System.Collections.Generic;
using System.Linq;
using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using Com.AFBiyik.MatchRow.LevelScene.View;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.LevelScene.Factory
{
    /// <summary>
    /// Factory for items.
    /// <br/>
    /// Install with:
    /// <code>
    /// List&lt;ItemView&gt;
    /// </code>
    /// <br/>
    /// Inject:
    /// <code>
    /// IFactory&lt;ItemType, Transform, ItemView&gt;
    /// </code>
    /// </summary>
    public class ItemFactory : IFactory<ItemType, Transform, ItemView>
    {
        // Private Readonly Fields
        private readonly DiContainer container;
        private readonly Dictionary<ItemType, ItemView> items;

        public ItemFactory(DiContainer container, List<ItemView> items)
        {
            this.container = container;
            this.items = items.ToDictionary(i => i.ItemType);
        }

        public ItemView Create(ItemType itemType, Transform parent)
        {
            ItemView view = container.InstantiatePrefabForComponent<ItemView>(items[itemType].gameObject, parent);
            return view;
        }
    }
}
