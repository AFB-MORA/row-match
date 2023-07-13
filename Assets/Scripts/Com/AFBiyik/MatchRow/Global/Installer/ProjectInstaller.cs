using Com.AFBiyik.AssetSystem;
using Com.AFBiyik.Factory;
using Com.AFBiyik.MatchRow.Global.LevelSystem;
using Com.AFBiyik.PopupSystem;
using Zenject;

namespace Com.AFBiyik.MatchRow.Global.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            AssetSystemInstaller.Install(Container, null);

            Container.BindInterfacesTo<PrefabFactory>()
                .AsSingle();

            Container.BindInterfacesTo<PopupController>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<LevelManager>()
                .AsSingle()
                .NonLazy();
        }
    }
}