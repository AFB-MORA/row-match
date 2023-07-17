using Cysharp.Threading.Tasks;

namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Initializer for asset system.
    /// </summary>
    public interface IAssetInitializer
    {
        /// <summary>
        /// Initialize asset system.
        /// </summary>
        /// <returns></returns>
        UniTask Initialize();
    }
}