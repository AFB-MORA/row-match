using UniRx;

namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Main controller for asset system.
    /// </summary>
    public class AssetManager : IAssetManager
    {
        // Protected readonly properties
        /// <summary>
        /// Reactive property for initialization.
        /// </summary>
        protected readonly ReactiveProperty<bool> isAssetInitialized = new ReactiveProperty<bool>(false);

        /// <summary>
        /// Asset initializer.
        /// </summary>
        protected readonly IAssetInitializer assetInitializer;

        /// <summary>
        /// Atlas loader.
        /// </summary>
        protected readonly IAtlasLoader atlasLoader;

        // Public Properties
        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<bool> IsAssetInitialized => isAssetInitialized;

        /// <summary>
        /// Creates asset systemm.
        /// </summary>
        /// <param name="assetInitializer">Asset initializer.</param>
        /// <param name="atlasLoader">Atlas loader.</param>
        public AssetManager(IAssetInitializer assetInitializer, IAtlasLoader atlasLoader)
        {
            this.assetInitializer = assetInitializer;
            this.atlasLoader = atlasLoader;
            Initialize();
        }

        /// <summary>
        /// Initializes asset system.
        /// </summary>
        protected virtual async void Initialize()
        {
            await assetInitializer.Initialize();
            isAssetInitialized.Value = true;
        }
    }
}
