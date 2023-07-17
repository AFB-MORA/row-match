using System.Collections.Generic;
using Com.AFBiyik.Factory;
using Com.AFBiyik.MatchRow.LevelScene.Controller;
using Com.AFBiyik.MatchRow.LevelScene.Factory;
using Com.AFBiyik.MatchRow.LevelScene.Input;
using Com.AFBiyik.MatchRow.LevelScene.Presenter;
using Com.AFBiyik.MatchRow.LevelScene.VFX;
using Com.AFBiyik.MatchRow.LevelScene.View;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.MatchRow.Global.Manager;
using Com.AFBiyik.Pool;
using Com.AFBiyik.UIComponents;
using UnityEngine;
using Zenject;
using System;

namespace Com.AFBiyik.MatchRow.LevelScene.Installer
{
    /// <summary>
    /// Install level scene dependencies
    /// </summary>
    public class LevelSceneInstaller : MonoInstaller
    {
        // Serialize Fields
        [SerializeField]
        private BoundsView gridBounds;
        [SerializeField]
        private List<ItemView> itemPrefabs;
        [SerializeField]
        private CompletedEffect completedEffectPrefab;
        [SerializeField]
        private Transform completedEffectParent;

        public override void InstallBindings()
        {
            BindBounds();
            BindLevelModel();
            BindPresenters();
            BindFactories();
            BindPools();
            BindInput();
            BindGameController();
        }

        private void BindBounds()
        {
            Container.Bind<BoundsView>()
                .FromInstance(gridBounds)
                .AsSingle();
        }

        private void BindGameController()
        {
            // Game Controller
            Container.BindInterfacesTo<GameController>()
                .AsSingle()
                .NonLazy();
        }

        private void BindInput()
        {
            // ISwipeEvent
            Container.BindInterfacesTo<SwipeManager>()
                .AsSingle();
        }

        private void BindPools()
        {
            // Install pool
            MonoMemoryPoolInstaller<CompletedEffect>.Install(Container, completedEffectPrefab, completedEffectParent,
                new MemoryPoolSettings(
                    // Initial size
                    9,
                    // Max size
                    int.MaxValue,
                    // Expand Mode
                    PoolExpandMethods.OneAtATime));
        }

        private void BindFactories()
        {
            // GridBackgroundCell Factory
            Container.BindInterfacesTo<GenericPrefabFactory<GridBackgroundCell>>()
                .AsSingle();

            // ItemView Factory
            Container.BindInterfacesTo<ItemFactory>()
                .AsSingle()
                .WithArguments(itemPrefabs);
        }

        private void BindPresenters()
        {
            // GridPresenter
            Container.BindInterfacesTo<GridPresenter>()
                .AsSingle();

            // GridPresenter
            Container.BindInterfacesTo<GamePresenter>()
                .AsSingle();
        }

        private void BindLevelModel()
        {
            // Level Model
            Container.Bind<LevelModel>()
                    .FromMethod(GetLevelModel)
                    .AsSingle();
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