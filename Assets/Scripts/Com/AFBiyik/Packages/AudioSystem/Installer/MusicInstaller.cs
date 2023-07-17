using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Installs music controller.
    ///
    /// <example>
    /// <code>
    /// public class ProjectInstaller : MonoInstaller {
    ///     [SerializeField]
    ///     private AudioConfig audioConfig;
    /// 
    ///     public override void InstallBindings() {
    ///         AudioInstaller&lt;MusicInstaller&gt;.Install(Container, audioConfig);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// 
    /// </summary>
    public class MusicInstaller : BaseAudioInstaller<MusicInstaller> {
        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="globalAudioParent"></param>
        /// <param name="audioConfig"></param>
        public MusicInstaller(Transform globalAudioParent, AudioConfig audioConfig) : base(globalAudioParent, audioConfig) {
        }

        /// <summary>
        /// Installs bindings.
        /// </summary>
        public override void InstallBindings() {
            Container.Bind<IMusicController>()
                .FromSubContainerResolve()
                .ByMethod(InstallController)
                .AsCached()
                .NonLazy();
        }

        /// <summary>
        /// Isolates controller.
        /// </summary>
        /// <param name="subContainer"></param>
        protected virtual void InstallController(DiContainer subContainer) {
            Transform parent = CreateSourceParent("MusicParent");

            subContainer.Bind<IFactory<IFxSource>>()
                .To<FxSourceFactory>()
                .FromNew()
                .AsCached()
                .WithArguments(parent, audioConfig.MusicSource);

            subContainer.Bind<IMusicController>()
                .To<MusicController>()
                .FromNew()
                .AsCached()
                .WithArguments(audioConfig.MusicConfig)
                .NonLazy();
        }
    }
}