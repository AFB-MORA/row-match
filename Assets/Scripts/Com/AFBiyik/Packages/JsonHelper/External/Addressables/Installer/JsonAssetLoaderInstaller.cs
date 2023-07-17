using Zenject;

namespace Com.AFBiyik.JsonHelper
{
    /// <summary>
    /// Initializes <see cref="JsonLoaderAddressables"/>.
    /// <br/>
    /// <example>
    /// Use this installer in the same context with <see cref="IAssetLoader"/> binding.
    /// <code>
    /// JsonAssetLoaderInstaller.Install(Container);
    /// </code>
    /// </example>
    /// </summary>
    public class JsonAssetLoaderInstaller : Installer<JsonAssetLoaderInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<JsonAssetLoaderInitializer>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}

