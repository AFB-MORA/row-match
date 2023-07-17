using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Base interface for any audio controller.
    /// </summary>
    public interface IAudioController {
        // Properties
        /// <summary>
        /// Enabled state of the controller. Saves to <see cref="UnityEngine.PlayerPrefs"/>.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsLoaded { get; }

        // Methods
        /// <summary>
        /// Plays sound
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="cancellationToken"></param>
        void PlaySound(Sound sound, CancellationToken cancellationToken = default);

        /// <summary>
        /// Plays sound and returns source that plays the sound.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask<IFxSource> PlayAndGetSource(Sound sound, CancellationToken cancellationToken = default);
    }
}
