using System.Collections.Generic;
using Com.AFBiyik.MatchRow.Factory;
using Com.AFBiyik.MatchRow.GameScene.Controller;
using Com.AFBiyik.MatchRow.GameScene.Factory;
using Com.AFBiyik.MatchRow.GameScene.Input;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Com.AFBiyik.MatchRow.GameScene.View;
using Com.AFBiyik.MatchRow.LevelSystem;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.Installer
{
    /// <summary>
    /// Install game scene dependencies
    /// </summary>
    public class GameSceneInstaller : MonoInstaller
    {
        // Serialize Fields
        [SerializeField]
        private GridBoundsView gridBounds;
        [SerializeField]
        private List<ItemView> itemPrefabs;

        public override void InstallBindings()
        {
            LevelModel level = new LevelModel();
            level.GridWidth = 5;
            level.GridHeight = 7;
            level.LevelNumber = 1;
            level.MoveCount = 40;
            level.Grid = new string[] { "b", "b", "y", "b", "b", "g", "y", "g", "r", "b", "y", "g", "r", "g", "g", "b", "b", "g", "b", "y", "r", "r", "g", "g", "y", "g", "g", "y", "y", "b", "y", "b", "b", "y", "b" };

            // GridPresenter
            Container.BindInterfacesTo<GridPresenter>()
                .AsSingle()
                .WithArguments(level, gridBounds.Rect);

            // GridBackgroundCell Factory
            Container.BindInterfacesTo<GenericPrefabFactory<GridBackgroundCell>>()
                .AsSingle();

            // ItemView Factory
            Container.BindInterfacesTo<ItemFactory>()
                .AsSingle()
                .WithArguments(itemPrefabs);

            // ISwipeEvent
            Container.BindInterfacesTo<SwipeManager>()
                .AsSingle();

            // Game Controller
            Container.BindInterfacesTo<GameController>()
                .AsSingle()
                .NonLazy();
        }
    }
}