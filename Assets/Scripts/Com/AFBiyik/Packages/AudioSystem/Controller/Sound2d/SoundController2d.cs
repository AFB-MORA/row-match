using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Controls 2d sound.
    /// </summary>
    public class SoundController2d : AudioControllerBase, ISoundController2d {
        // Protected Readonly Properties
        /// <summary>
        /// Sound source pool.
        /// </summary>
        protected readonly IMemoryPool<IFxSource> sourcePool;

        // Protected Properties
        /// <inheritdoc/>
        protected override string Label => "sound";
        /// <inheritdoc/>
        protected override string Prefs => "enableSound";

        // Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetLoader"></param>
        /// <param name="sourcePool"></param>
        public SoundController2d(IAssetLoader assetLoader, IMemoryPool<IFxSource> sourcePool) : base(assetLoader) {
            this.sourcePool = sourcePool;

            _ = InitializeAsync();
        }

        // Protected Methods
        /// <inheritdoc/>
        protected override async UniTask InitializeAsync() {
            await base.InitializeAsync();
            isLoaded.SetValueAndForceNotify(true);
        }

        /// <inheritdoc/>
        protected override IFxSource GetFxSource() {
            return sourcePool.Spawn();
        }
    }
}