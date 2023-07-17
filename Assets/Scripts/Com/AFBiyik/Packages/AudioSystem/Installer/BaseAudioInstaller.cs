using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Base installer for audio controllers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseAudioInstaller<T> : Installer<Transform, AudioConfig, T> where T : BaseAudioInstaller<T> {
        /// <summary>
        /// Parent for all controllers.
        /// </summary>
        protected readonly Transform globalAudioParent;

        /// <summary>
        /// Config.
        /// </summary>
        protected readonly AudioConfig audioConfig;

        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="globalAudioParent"></param>
        /// <param name="audioConfig"></param>
        public BaseAudioInstaller(Transform globalAudioParent, AudioConfig audioConfig) {
            this.globalAudioParent = globalAudioParent;
            this.audioConfig = audioConfig;
        }

        /// <summary>
        /// Creates parent for pool.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual Transform CreateSourceParent(string name) {
            GameObject go = new GameObject(name);
            go.transform.SetParent(globalAudioParent);
            return go.transform;
        }
    }
}