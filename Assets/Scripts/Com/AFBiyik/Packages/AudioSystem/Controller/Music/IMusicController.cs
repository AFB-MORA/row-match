using UnityEngine;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Controls music.
    /// <example>
    /// <code>
    /// [Inject]
    /// private IMusicController musicController;
    ///
    /// private void Awake() {
    ///     musicController.IsLoaded.Subscribe(OnMusicLoaded);
    /// }
    ///
    /// private void OnMusicLoaded(bool loaded) {
    ///     if (loaded) {
    ///         musicController.PlayMusic(musics: new Sound[] { new Sound(Sounds.MUSIC1), new Sound(Sounds.MUSIC2), new Sound(Sounds.MUSIC3), new Sound(Sounds.MUSIC4) });         
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public interface IMusicController : IAudioController {
        // Methods
        /// <summary>
        /// Plays given musics.
        /// </summary>
        /// <param name="sequential">If true, plays music in given order; otherwise in random order.</param>
        /// <param name="musics">Musics to play.</param>
        void PlayMusic(bool sequential = false, params Sound[] musics);

        /// <summary>
        /// Pauses music.
        /// </summary>
        /// <param name="pause"></param>
        void PauseMusic(bool pause);

        /// <summary>
        /// Stops music.
        /// </summary>
        void StopMusic();
    }
}
