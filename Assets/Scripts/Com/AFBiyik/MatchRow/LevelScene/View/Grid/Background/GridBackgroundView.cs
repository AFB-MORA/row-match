using System.Collections.Generic;
using Com.AFBiyik.MatchRow.LevelScene.Presenter;
using Com.AFBiyik.UIComponents;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

namespace Com.AFBiyik.MatchRow.LevelScene.View
{
    /// <summary>
    /// Initializes grid background.
    /// </summary>
    public class GridBackgroundView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private Transform gridCellContent;
        [SerializeField]
        private GridBackgroundCell gridBackgroundCellPrefab;
        [SerializeField]
        private SpriteShapeController border;
        [SerializeField]
        private float borderPadding = 0.04f;

        // Dependencies
        [Inject]
        private IGridPresenter gridPresenter;
        [Inject]
        private IFactory<GridBackgroundCell, Transform, GridBackgroundCell> gridBackgroundCellFactory;
        [Inject]
        private BoundsView boundsView;

        // Private Fields
        private GridBackgroundCell[,] gridBackgroundCells;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize background
        /// </summary>
        private void Initialize()
        {
            gridBackgroundCells = new GridBackgroundCell[gridPresenter.Columns, gridPresenter.Rows];

            // For each column
            for (int x = 0; x < gridPresenter.Columns; x++)
            {
                // For each row
                for (int y = 0; y < gridPresenter.Rows; y++)
                {
                    // Create and initialize cell
                    var cell = gridBackgroundCellFactory.Create(gridBackgroundCellPrefab, gridCellContent);
                    gridBackgroundCells[x, y] = cell;
                }
            }

            // Set size
            UpdateGrid();

            // Subscribe dimention change
            boundsView.onBoundsChange.AddListener(UpdateGrid);
        }

        /// <summary>
        /// Update background
        /// </summary>
        private void UpdateGrid()
        {
            // For each column
            for (int x = 0; x < gridPresenter.Columns; x++)
            {
                // For each row
                for (int y = 0; y < gridPresenter.Rows; y++)
                {
                    // Create and initialize cell
                    var cell = gridBackgroundCells[x, y];
                    cell.Initialize(new Vector2Int(x, y));
                }
            }


            // Set border
            SetBorder();
        }

        /// <summary>
        /// Sets border
        /// </summary>
        private void SetBorder()
        {
            // Get rect
            var rect = gridPresenter.Rect;

            // Get displacement
            Vector2 displacement = new Vector2(borderPadding, borderPadding);

            // Get corners
            Vector3 topRight = rect.max + displacement;
            Vector3 bottomLeft = rect.min - displacement;
            Vector3 topLeft = new Vector3(bottomLeft.x, topRight.y, 0);
            Vector3 bottomRight = new Vector3(topRight.x, bottomLeft.y, 0);

            // Set corners
            border.spline.SetPosition(0, topLeft);
            border.spline.SetPosition(1, topRight);
            border.spline.SetPosition(2, bottomRight);
            border.spline.SetPosition(3, bottomLeft);
        }
    }
}