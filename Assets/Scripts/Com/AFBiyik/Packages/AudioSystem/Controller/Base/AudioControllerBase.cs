using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Base audio controller.
    /// </summary>
    public abstract class AudioControllerBase : IAudioController, IDisposable {
        // Protected Readonly Properties
        /// <summary>
        /// Asset loader to load sound files.
        /// </summary>
        protected readonly IAssetLoader assetLoader;

        /// <summary>
        /// True if all sounds loaded; otherwise false.
        /// </summary>
        protected readonly ReactiveProperty<bool> isLoaded = new ReactiveProperty<bool>(false);

        /// <summary>
        /// Dictionary for loaded sounds.
        /// </summary>
        protected readonly Dictionary<string, AudioClip> soundDict = new Dictionary<string, AudioClip>();

        /// <summary>
        /// 
        /// </summary>
        protected readonly CompositeDisposable disposables = new CompositeDisposable();

        // Protected Abstract Properties
        /// <summary>
        /// Label for sounds.
        /// </summary>
        protected abstract string Label { get; }

        /// <summary>
        /// <see cref="PlayerPrefs"/> to save <see cref="Enabled"/> state.
        /// </summary>
        protected abstract string Prefs { get; }

        // Public Properties
        /// <inheritdoc/>
        public bool Enabled {
            get {
                return PlayerPrefs.GetInt(Prefs, 1) == 1;
            }
            set {
                PlayerPrefs.SetInt(Prefs, value ? 1 : 0);
                OnStateChanged(value);
            }
        }

        /// <inheritdoc/>
        public IReadOnlyReactiveProperty<bool> IsLoaded => isLoaded;

        // Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetLoader">Asset loader to load sound files.</param>
        public AudioControllerBase(IAssetLoader assetLoader) {
            this.assetLoader = assetLoader;
        }

        // Protected Methods
        /// <summary>
        /// Initializes controller.
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask InitializeAsync() {
            _ = Enabled;

            await LoadPreloadSounds();
        }

        /// <summary>
        /// Gets clip with given key.
        /// </summary>
        /// <param name="key">Addressable key</param>
        /// <returns>Clip</returns>
        protected virtual async UniTask<AudioClip> GetClip(string key, CancellationToken cancellationToken = default) {
            if (soundDict.ContainsKey(key)) {
                return soundDict[key];
            }
            else {
                return await LoadSound(key, cancellationToken);
            }
        }

        /// <summary>
        /// Loads sound with <see cref="assetLoader"/>.
        /// </summary>
        /// <param name="key">Addressable key</param>
        /// <returns>Clip</returns>
        protected virtual async UniTask<AudioClip> LoadSound(string key, CancellationToken cancellationToken = default) {
            var sound = await assetLoader.LoadAsset<AudioClip>(key, cancellationToken);
            if (sound != null) {
                soundDict[key] = sound;
            }

            return sound;
        }

        /// <summary>
        /// Preloads sounds with <see cref="Label"/>
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask LoadPreloadSounds() {
            var sounds = await assetLoader.LoadAssets<AudioClip>(Label);

            foreach (var sound in sounds) {
                soundDict[sound.name] = sound;
            }
        }

        /// <summary>
        /// Called when <see cref="Enabled"/> states changed.
        /// </summary>
        /// <param name="enabled"></param>
        protected virtual void OnStateChanged(bool enabled) { }

        // Protected Abstract Methods
        /// <summary>
        /// Called to get FxSource in <see cref="PlayAndGetSource"/>
        /// </summary>
        /// <returns></returns>
        protected abstract IFxSource GetFxSource();

        // Public Methods
        /// <summary>
        /// Disposes controller
        /// </summary>
        public virtual void Dispose() {
            disposables.Dispose();
        }

        /// <inheritdoc/>
        public virtual void PlaySound(Sound sound, CancellationToken cancellationToken = default) {
            _ = PlayAndGetSource(sound, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async UniTask<IFxSource> PlayAndGetSource(Sound sound, CancellationToken cancellationToken = default) {
            if (Enabled) {
                var clip = await GetClip(sound.name, cancellationToken);

                if (clip != null) {
                    var source = GetFxSource();
                    source.Play(sound, soundDict[sound.name], cancellationToken);
                    return source;
                }
            }

            return null;
        }
    }
}
