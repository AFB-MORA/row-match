using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Installs sound 2d controller.
    ///
    /// <example>
    /// <code>
    /// public class ProjectInstaller : MonoInstaller {
    ///     [SerializeField]
    ///     private AudioConfig audioConfig;
    /// 
    ///     public override void InstallBindings() {
    ///         AudioInstaller&lt;Sound2dInstaller&gt;.Install(Container, audioConfig);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// 
    /// </summary>
    public class Sound2dInstaller : BaseAudioInstaller<Sound2dInstaller> {
        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="globalAudioParent"></param>
        /// <param name="audioConfig"></param>
        public Sound2dInstaller(Transform globalAudioParent, AudioConfig audioConfig) : base(globalAudioParent, audioConfig) {
        }

        /// <summary>
        /// Installs bindings.
        /// </summary>
        public override void InstallBindings() {
            Container.Bind<ISoundController2d>()
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
            Transform parent = CreateSourceParent("Sound2dPool");

            subContainer.Bind<IFactory<IFxSource>>()
                .To<FxSourceFactory>()
                .FromNew()
                .AsCached()
                .WithArguments(parent, audioConfig.SoundSource2d);

            subContainer.Bind<IMemoryPool<IFxSource>>()
                .To<SoundFxPool>()
                .FromNew()
                .AsCached()
                .WithArguments(
                    new MemoryPoolSettings(
                        // Initial size
                        8,
                        // Max size
                        int.MaxValue,
                        PoolExpandMethods.OneAtATime)
                );

            subContainer.Bind<ISoundController2d>()
                .To<SoundController2d>()
                .FromNew()
                .AsCached()
                .NonLazy();
        }
    }
}