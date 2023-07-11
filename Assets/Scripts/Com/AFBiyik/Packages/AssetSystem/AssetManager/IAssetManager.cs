using UniRx;

namespace Com.AFBiyik.AssetSystem {
    /// <summary>
    /// Main controller for asset system.
    /// </summary>
    public interface IAssetManager {
        /// <summary>
        /// Reactive property for initialization.
        /// <br/>
        /// True if initialization completed; otherwise false.
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsAssetInitialized { get; }
    }
}
