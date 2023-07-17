using System;
using Com.AFBiyik.AssetSystem;
using Com.AFBiyik.AudioSystem;
using Com.AFBiyik.Factory;
using Com.AFBiyik.LanguageSystem;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.MatchRow.Global.Manager;
using Com.AFBiyik.PopupSystem;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.Global.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField]
        private AudioConfig audioConfig;

        public override void InstallBindings()
        {
            BindAssetSystem();
            BindFactories();
            BindPopupSystem();
            BindLevelSystem();
            BindLanguageSystem();
            BindProjectManager();
            BindAudio();
        }

        private void BindAudio()
        {
            AudioSystemInstaller<MusicInstaller, Sound2dInstaller>.Install(Container, audioConfig);
        }

        private void BindProjectManager()
        {
            Container.BindInterfacesTo<ProjectManager>()
                .AsSingle()
                .NonLazy();
        }

        private void BindLanguageSystem()
        {
            Container.BindInterfacesTo<LanguageController>()
                .AsSingle()
                .NonLazy();
        }

        private void BindLevelSystem()
        {
            Container.BindInterfacesTo<LevelManager>()
                .AsSingle()
                .NonLazy();
        }

        private void BindPopupSystem()
        {
            Container.BindInterfacesTo<PopupController>()
                .AsSingle()
                .NonLazy();
        }

        private void BindFactories()
        {
            Container.BindInterfacesTo<PrefabFactory>()
                .AsSingle();
        }

        private void BindAssetSystem()
        {
            AssetSystemInstaller.Install(Container, null);
        }
    }
}