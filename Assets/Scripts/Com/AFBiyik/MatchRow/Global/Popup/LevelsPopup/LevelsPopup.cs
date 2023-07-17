using System.Collections;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.PopupSystem;
using Com.AFBiyik.UIComponents;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.Global.Popup
{
    /// <summary>
    /// Popup that shows levels
    /// </summary>
    public class LevelsPopup : BaseAnimatedPopup, IRecycleViewDataSource
    {
        //Serialize Fields
        [SerializeField]
        private RecycleView recycleView;
        [SerializeField]
        private GameObject cellPrefab;

        // Dependencies
        [Inject]
        private ILevelManager levelManager;
        [Inject]
        private IFactory<GameObject, GameObject> prefabFactory;

        // Protected Properties
        protected override string CloseState => null;

        /// <inheritdoc/>
        public int NumberOfItems => levelManager.NumberOfLevels;

        /// <inheritdoc/>
        public float ItemHeight => ((RectTransform)cellPrefab.transform).rect.size.y;

        private void Awake()
        {
            // Initialize recycle view
            recycleView.Initialize(this);
        }

        /// <inheritdoc/>
        public override void OnOpened(Hashtable args)
        {
            base.OnOpened(args);

            int level = levelManager.GetLastLevel();

            // Reset content position
            recycleView.SetFistItemIndex(level - 1);
        }

        /// <inheritdoc/>
        public GameObject CreateView()
        {
            // Create cell
            return prefabFactory.Create(cellPrefab);
        }

        /// <inheritdoc/>
        public void SetView(GameObject view, int index)
        {
            // Get cell
            var cell = view.GetComponent<LevelCell>();
            // Set level
            cell.SetLevel(index + 1);
        }

        /// <summary>
        /// Called to close popup
        /// </summary>
        public void CloseClick()
        {
            ClosePopup();
        }
    }
}
