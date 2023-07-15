using System.Collections.Generic;
using Com.AFBiyik.Factory;
using Com.AFBiyik.MatchRow.GameScene.Controller;
using Com.AFBiyik.MatchRow.GameScene.Factory;
using Com.AFBiyik.MatchRow.GameScene.Input;
using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Com.AFBiyik.MatchRow.GameScene.View;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.MatchRow.Global.Manager;
using Com.AFBiyik.UIComponents;
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
        private BoundsView gridBounds;
        [SerializeField]
        private List<ItemView> itemPrefabs;

        public override void InstallBindings()
        {
            // Level Model
            Container.Bind<LevelModel>()
                .FromMethod(GetLevelModel)
                .AsSingle();

            // GridPresenter
            Container.BindInterfacesTo<GridPresenter>()
                .AsSingle()
                .WithArguments(gridBounds.Rect);

            // GridPresenter
            Container.BindInterfacesTo<GamePresenter>()
                .AsSingle();

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

        private LevelModel GetLevelModel()
        {
            var levelModel = Container.Resolve<IProjectManager>().CurrentLevel;

#if UNITY_EDITOR
            if (levelModel == null)
            {
                _ = Container.Resolve<ILevelManager>().GetLevel(1);
                levelModel = new LevelModel();
                levelModel.GridWidth = 5;
                levelModel.GridHeight = 7;
                levelModel.LevelNumber = 1;
                levelModel.MoveCount = 40;
                levelModel.Grid = new string[] { "b", "b", "y", "b", "b", "g", "y", "g", "r", "b", "y", "g", "r", "g", "g", "b", "b", "g", "b", "y", "r", "r", "g", "g", "y", "g", "g", "y", "y", "b", "y", "b", "b", "y", "b" };
                levelModel.HighScore = 0;
                levelModel.IsLocked = false;
            }
#endif

            return levelModel;

        }
    }
}