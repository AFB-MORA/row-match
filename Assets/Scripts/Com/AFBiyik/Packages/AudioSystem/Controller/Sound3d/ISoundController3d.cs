using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Controls music.
    /// <example>
    /// <code>
    /// [Inject]
    /// private ISoundController3d soundController3d;
    ///
    /// private void FooAction() {
    ///     soundController3d.PlaySound(new Sound(Sounds.Foo_SOUND), foo.transfom);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public interface ISoundController3d : ISoundController2d {
        // Methods
        /// <summary>
        /// Plays sound in the parent.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="parent"></param>
        void PlaySound(Sound sound, Transform parent);

        /// <summary>
        /// Plays sound and returns source that plays the sound in the parent.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="parent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask<IFxSource> PlayAndGetSource(Sound sound, Transform parent, CancellationToken cancellationToken = default);
    }
}
