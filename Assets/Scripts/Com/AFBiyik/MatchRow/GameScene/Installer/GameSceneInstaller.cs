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
                .FromMethod(() => Container.Resolve<IProjectManager>().CurrentLevel)
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
    }
}