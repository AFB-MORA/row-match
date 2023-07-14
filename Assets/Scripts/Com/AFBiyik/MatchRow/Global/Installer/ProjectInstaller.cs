using Com.AFBiyik.AssetSystem;
using Com.AFBiyik.Factory;
using Com.AFBiyik.LanguageSystem;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.PopupSystem;
using Zenject;

namespace Com.AFBiyik.MatchRow.Global.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAssetSystem();
            BindFactories();
            BindPopupSystem();
            BindLevelSystem();
            BinLanguageSystem();
        }

        private void BinLanguageSystem()
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