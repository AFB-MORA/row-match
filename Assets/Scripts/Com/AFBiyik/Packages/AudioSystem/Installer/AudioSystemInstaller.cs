using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Installs a controller.
    /// <example>
    /// <code>
    /// public class ProjectInstaller : MonoInstaller {
    ///     [SerializeField]
    ///     private AudioConfig audioConfig;
    ///     
    ///     public override void InstallBindings() {
    ///     AudioSystemInstaller&lt;MusicInstaller&gt;.Install(Container, audioConfig);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T1">Type of controller to install</typeparam>
    public class AudioSystemInstaller<T1> : BaseAudioSystemInstaller<AudioSystemInstaller<T1>>
        where T1 : BaseAudioInstaller<T1> {

        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="audioConfig"></param>
        public AudioSystemInstaller(AudioConfig audioConfig) : base(audioConfig) {
        }

        /// <summary>
        /// Installs bindings.
        /// </summary>
        public override void InstallBindings() {
            InstallAudio<T1>();
        }
    }

    /// <summary>
    /// Installs two controller.
    /// <example>
    /// <code>
    /// public class ProjectInstaller : MonoInstaller {
    ///     [SerializeField]
    ///     private AudioConfig audioConfig;
    ///     
    ///     public override void InstallBindings() {
    ///         AudioSystemInstaller&lt;MusicInstaller, Sound2dInstaller&gt;.Install(Container, audioConfig);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T1">Type of controller to install</typeparam>
    /// <typeparam name="T2">Type of controller to install</typeparam>
    public class AudioSystemInstaller<T1, T2> : BaseAudioSystemInstaller<AudioSystemInstaller<T1, T2>>
        where T1 : BaseAudioInstaller<T1>
        where T2 : BaseAudioInstaller<T2> {

        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="audioConfig"></param>
        public AudioSystemInstaller(AudioConfig audioConfig) : base(audioConfig) {
        }

        /// <summary>
        /// Installs bindings.
        /// </summary>
        public override void InstallBindings() {
            InstallAudio<T1>();
            InstallAudio<T2>();
        }
    }

    /// <summary>
    /// Installs three controller.
    /// <example>
    /// <code>
    /// public class ProjectInstaller : MonoInstaller {
    ///     [SerializeField]
    ///     private AudioConfig audioConfig;
    /// 
    ///     public override void InstallBindings() {
    ///         AudioSystemInstaller&lt;MusicInstaller, Sound2dInstaller, Sound3dInstaller&gt;.Install(Container, audioConfig);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T1">Type of controller to install</typeparam>
    /// <typeparam name="T2">Type of controller to install</typeparam>
    /// <typeparam name="T3">Type of controller to install</typeparam>
    public class AudioSystemInstaller<T1, T2, T3> : BaseAudioSystemInstaller<AudioSystemInstaller<T1, T2, T3>>
        where T1 : BaseAudioInstaller<T1>
        where T2 : BaseAudioInstaller<T2>
        where T3 : BaseAudioInstaller<T3> {

        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="audioConfig"></param>
        public AudioSystemInstaller(AudioConfig audioConfig) : base(audioConfig) {
        }

        /// <summary>
        /// Installs bindings.
        /// </summary>
        public override void InstallBindings() {
            InstallAudio<T1>();
            InstallAudio<T2>();
            InstallAudio<T3>();
        }
    }
}
