using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Controls music.
    /// </summary>
    public class MusicController : AudioControllerBase, IMusicController {
        // Protected Readonly Properties
        /// <summary>
        /// Source to play music.
        /// </summary>
        protected readonly IFxSource musicSource;

        /// <summary>
        /// 
        /// </summary>
        protected readonly MusicConfig musicConfig;

        // Protected Override Properties
        /// <inheritdoc/>
        protected override string Label => "music";
        /// <inheritdoc/>
        protected override string Prefs => "enableMusic";

        // Protected Properties
        /// <summary>
        /// If true, plays music in given order; otherwise in random order.
        /// </summary>
        protected bool sequential;

        /// <summary>
        /// Musics to play.
        /// </summary>
        protected Sound[] musics;

        /// <summary>
        /// Index of the playing music.
        /// </summary>
        protected int musicIndex;

        // Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetLoader"></param>
        /// <param name="musicSourceFactory"></param>
        /// <param name="musicConfig"></param>
        public MusicController(IAssetLoader assetLoader, IFactory<IFxSource> musicSourceFactory, MusicConfig musicConfig) : base(assetLoader) {
            musicSource = musicSourceFactory.Create();
            this.musicConfig = musicConfig;
            musicSource.Initialize(null);
            sequential = false;

            _ = InitializeAsync();
        }

        // Protected Methods
        /// <inheritdoc/>
        protected override async UniTask InitializeAsync() {
            await base.InitializeAsync();

            musicSource.OnFinished.Subscribe(OnMusicFinished).AddTo(disposables);

            isLoaded.SetValueAndForceNotify(true);
        }

        /// <summary>
        /// Called when music finishes.
        /// </summary>
        /// <param name="source"></param>
        protected virtual async void OnMusicFinished(IFxSource source) {
            source.Stop();

            if (musicConfig.MusicGap > 0) {
                await UniTask.Delay((int)(musicConfig.MusicGap * 1000f));
            }

            ChangeMusic();
        }

        /// <summary>
        /// Changes music.
        /// </summary>
        protected virtual void ChangeMusic() {
            if (musics == null || musics.Length == 0) {
                return;
            }

            if (sequential) {
                musicIndex = (musicIndex + 1) % musics.Length;
            }
            else {
                musicIndex = Random.Range(0, musics.Length);
            }

            Sound nextMusic = musics[musicIndex];
            nextMusic.loop = false;
            nextMusic.autoDespawn = false;

            // Not interrupt music with same music
            if (musicSource.Source.clip == null || !musicSource.Source.isPlaying ||
                    (musicSource.Source.isPlaying && nextMusic.name != musicSource.Source.clip.name)) {

                musicSource.Stop();
                PlaySound(nextMusic);
            }
        }

        /// <inheritdoc/>
        protected override void OnStateChanged(bool enabled) {
            musicSource.gameObject.SetActive(enabled);

            if (enabled) {
                ChangeMusic();
            }
        }

        /// <inheritdoc/>
        protected override IFxSource GetFxSource() {
            return musicSource;
        }

        // Public Methods
        /// <inheritdoc/>
        public virtual void PlayMusic(bool sequential = false, params Sound[] musics) {
            this.sequential = sequential;
            this.musics = musics;
            musicIndex = -1;

            ChangeMusic();
        }

        /// <inheritdoc/>
        public virtual void StopMusic() {
            musicSource.Stop();
        }

        /// <inheritdoc/>
        public virtual void PauseMusic(bool pause) {
            musicSource.Pause(pause);
        }
    }
}
