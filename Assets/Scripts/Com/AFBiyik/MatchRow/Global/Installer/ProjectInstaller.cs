using Com.AFBiyik.AssetSystem;
using Com.AFBiyik.Factory;
using Com.AFBiyik.PopupSystem;
using Zenject;

namespace Com.AFBiyik.MatchRow.Global.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AssetLoader>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<AtlasLoader>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<AssetManager>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<PrefabFactory>()
                .AsSingle();

            Container.BindInterfacesTo<PopupController>()
                .AsSingle()
                .NonLazy();
        }
    }
}