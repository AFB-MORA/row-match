using System;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class FxSource : MonoBehaviour, IFxSource {
        /// <summary>
        /// 
        /// </summary>
        protected readonly Subject<IFxSource> onFinished = new Subject<IFxSource>();

        /// <summary>
        /// 
        /// </summary>
        protected bool isActive;

        /// <summary>
        /// Souns that playing.
        /// </summary>
        protected Sound sound;

        /// <summary>
        /// Source to play sound.
        /// </summary>
        protected AudioSource source;

        /// <summary>
        /// Sound timer.
        /// </summary>
        protected IDisposable timer;

        /// <summary>
        /// Pool to despawn
        /// </summary>
        protected IMemoryPool<IFxSource> sourcePool;

        /// <inheritdoc/>
        public virtual AudioSource Source => source;

        /// <inheritdoc/>
        public IObservable<IFxSource> OnFinished => onFinished;

        /// <inheritdoc/>
        public virtual void Initialize(IMemoryPool<IFxSource> sourcePool) {
            this.sourcePool = sourcePool;

            source = gameObject.GetComponent<AudioSource>();
            source.playOnAwake = false;
        }

        /// <summary>
        /// Called when spawned.
        /// </summary>
        public virtual void OnSpawned() {
        }

        /// <summary>
        /// Called when despawned.
        /// </summary>
        public virtual void OnDespawned() {
            Source.clip = null;
        }

        /// <inheritdoc/>
        public virtual void Play(Sound sound, AudioClip clip, CancellationToken cancellationToken = default) {
            Source.clip = clip;
            this.sound = sound;
            isActive = true;
            SetSound();
            if (cancellationToken != null) {
                cancellationToken.Register(() => {
                    if (isActive) {
                        Stop();
                    }
                });
            }
        }

        /// <inheritdoc/>
        public virtual void Stop() {
            timer?.Dispose();
            timer = null;
            Source.Stop();
            Source.clip = null;

            if (isActive) {
                isActive = false;
                sourcePool?.Despawn(this);
            }
        }

        /// <inheritdoc/>
        public virtual void Pause(bool pause) {
            timer?.Dispose();
            timer = null;

            if (pause) {
                Source.Pause();
            }
            else {
                float start = Source.time;

                float length = Source.clip.length;
                if (sound.length > 0) {
                    length = Mathf.Min(length, sound.length);
                }

                length = length - start;

                Source.Play();
                timer = GetCounter(length).DoOnCompleted(SoundCompleted).Subscribe();
            }
        }

        /// <summary>
        /// Sets sound.
        /// </summary>
        protected virtual void SetSound() {
            Source.loop = sound.loop;
            Source.volume = sound.volume;

            float length = SetSoundTime();

            Source.Play();

            SubscribeComplete(length);
        }

        /// <summary>
        /// Sets souds time.
        /// </summary>
        /// <returns>The length to play.</returns>
        protected virtual float SetSoundTime() {
            Source.time = sound.startTime;

            float length = Source.clip.length;
            if (sound.length > 0) {
                length = Mathf.Min(length, sound.length);
            }

            return length;
        }

        /// <summary>
        /// Called when sound completed.
        /// </summary>
        protected virtual void SoundCompleted() {
            timer = null;

            if (sound.autoDespawn) {
                Stop();
            }
            else {
                if (sound.loop) {
                    float length = SetSoundTime();
                    SubscribeComplete(length);
                }
            }

            onFinished.OnNext(this);
        }

        /// <summary>
        /// Subscribes to timer.
        /// </summary>
        /// <param name="length"></param>
        protected virtual void SubscribeComplete(float length) {
            timer?.Dispose();
            timer = GetCounter(length).DoOnCompleted(SoundCompleted).Subscribe();
        }

        /// <summary>
        /// Gets timer.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        protected virtual IObservable<float> GetCounter(float seconds) {
            float secondsLeft = seconds;

            return Observable.EveryGameObjectUpdate()
                .TakeWhile(_ => secondsLeft > 0)
                .Select(_ => {
                    secondsLeft -= Time.unscaledDeltaTime;
                    return secondsLeft;
                });
        }
    }
}
