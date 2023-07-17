using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Source to play sounds.
    /// </summary>
    public interface IFxSource : IPoolable {
        /// <summary>
        /// 
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        /// 
        /// </summary>
        Transform transform { get; }

        /// <summary>
        /// Source to play sound.
        /// </summary>
        AudioSource Source { get; }

        /// <summary>
        /// Called when sound finishes.
        /// </summary>
        IObservable<IFxSource> OnFinished { get; }

        /// <summary>
        /// Initializes the source.
        /// </summary>
        /// <param name="sourcePool"></param>
        void Initialize(IMemoryPool<IFxSource> sourcePool);

        /// <summary>
        /// Plays sound.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <param name="clip">Clip to play.</param>
        /// <param name="cancellationToken"></param>
        void Play(Sound sound, AudioClip clip, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops and despawns sound.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the sound.
        /// </summary>
        /// <param name="pause"></param>
        void Pause(bool pause);
    }
}
