using System.Threading;
using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Controls 3d sound.
    /// </summary>
    public class SoundController3d : SoundController2d, ISoundController3d {
        // Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetLoader"></param>
        /// <param name="sourcePool"></param>
        public SoundController3d(IAssetLoader assetLoader, IMemoryPool<IFxSource> sourcePool) : base(assetLoader, sourcePool) {
        }

        // Public Methods
        /// <inheritdoc/>
        public virtual void PlaySound(Sound sound, Transform parent) {
            _ = PlayAndGetSource(sound, parent);
        }

        /// <inheritdoc/>
        public virtual async UniTask<IFxSource> PlayAndGetSource(Sound sound, Transform parent, CancellationToken cancellationToken = default) {
            if (Enabled) {
                var clip = await GetClip(sound.name, cancellationToken);

                if (clip != null) {
                    var source = GetFxSource();
                    source.transform.SetParent(parent);
                    source.transform.localPosition = Vector3.zero;
                    source.Play(sound, soundDict[sound.name], cancellationToken);
                    return source;
                }
            }

            return null;
        }
    }
}