using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Base installer for audio system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseAudioSystemInstaller<T> : Installer<AudioConfig, T> where T : BaseAudioSystemInstaller<T> {
        /// <summary>
        /// Parent for all controllers.
        /// </summary>
        protected Transform globalAudioParent;

        /// <summary>
        /// Config.
        /// </summary>
        protected readonly AudioConfig audioConfig;

        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="audioConfig"></param>
        public BaseAudioSystemInstaller(AudioConfig audioConfig) {
            this.audioConfig = audioConfig;
            globalAudioParent = CreateGlobalAudioParent("AudioParent");
        }

        /// <summary>
        /// Installs controller.
        /// </summary>
        /// <typeparam name="TController"><see cref="BaseAudioInstaller"/></typeparam>
        protected virtual void InstallAudio<TController>() where TController : BaseAudioInstaller<TController> {
            BaseAudioInstaller<TController>.Install(Container, globalAudioParent, audioConfig);
        }

        /// <summary>
        /// Creates parent for all controllers. See <see cref="globalAudioParent"/>.
        /// </summary>
        /// <param name="name">Name of the parent.</param>
        /// <returns></returns>
        protected virtual Transform CreateGlobalAudioParent(string name) {
            GameObject parent = new GameObject(name);
            GameObject.DontDestroyOnLoad(parent);
            return parent.transform;
        }
    }
}