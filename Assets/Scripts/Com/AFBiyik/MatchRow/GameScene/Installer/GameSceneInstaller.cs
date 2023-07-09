using Com.AFBiyik.MatchRow.Factory;
using Com.AFBiyik.MatchRow.LevelSystem;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene
{
    /// <summary>
    /// Install game scene dependencies
    /// </summary>
    public class GameSceneInstaller : MonoInstaller
    {
        // Serialize Fields
        [SerializeField]
        private GridBoundsView gridBounds;

        public override void InstallBindings()
        {
            LevelModel level = new LevelModel();
            level.GridWidth = 4;
            level.GridHeight = 4;
            level.LevelNumber = 1;
            level.MoveCount = 40;
            level.Grid = new string[0];

            // GridPresenter
            Container.BindInterfacesTo<GridPresenter>()
                .AsSingle()
                .WithArguments(level, gridBounds.Rect);

            // GridBackgroundCell Factory
            Container.BindInterfacesTo<GenericPrefabFactory<GridBackgroundCell>>()
                .AsSingle();

        }
    }
}