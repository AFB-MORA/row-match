using Cysharp.Threading.Tasks;

namespace Com.AFBiyik.PopupSystem
{
    /// <summary>
    /// Base animated popup interface for open/close animations
    /// </summary>
    public interface IAnimatedPopup : IPopup
    {
        /// <summary>
        /// Plays open animation and awaits
        /// </summary>
        /// <returns>UniTask</returns>
        UniTask PlayOpenAnimation();

        /// <summary>
        /// Plays close animation and awaits
        /// </summary>
        /// <returns>UniTask</returns>
        UniTask PlayCloseAnimation();
    }
}