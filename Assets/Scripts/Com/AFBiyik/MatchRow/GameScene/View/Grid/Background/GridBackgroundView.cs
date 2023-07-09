using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene
{
    /// <summary>
    /// Initializes grid background.
    /// </summary>
    public class GridBackgroundView : MonoBehaviour
    {
        // Serialkize Fields
        [SerializeField]
        private Transform gridCellContent;
        [SerializeField]
        private GridBackgroundCell gridBackgroundCellPrefab;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;
        [Inject]
        private IFactory<GridBackgroundCell, Transform, GridBackgroundCell> gridBackgroundCellFactory;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize background
        /// </summary>
        private void Initialize()
        {
            // For each column
            for (int x = 0; x < gridPresenter.Colums; x++)
            {
                // For each row
                for (int y = 0; y < gridPresenter.Rows; y++)
                {
                    // Create and initialize cell
                    var cell = gridBackgroundCellFactory.Create(gridBackgroundCellPrefab, gridCellContent);
                    cell.Initialize(new Vector2Int(x,y));
                }
            }
        }
    }
}