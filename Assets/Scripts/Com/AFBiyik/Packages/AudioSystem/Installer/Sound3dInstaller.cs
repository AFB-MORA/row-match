using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Installs sound 3d controller.
    ///
    /// <example>
    /// <code>
    /// public class ProjectInstaller : MonoInstaller {
    ///     [SerializeField]
    ///     private AudioConfig audioConfig;
    /// 
    ///     public override void InstallBindings() {
    ///         AudioInstaller&lt;Sound3dInstaller&gt;.Install(Container, audioConfig);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// 
    /// </summary>
    public class Sound3dInstaller : BaseAudioInstaller<Sound3dInstaller> {
        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="globalAudioParent"></param>
        /// <param name="audioConfig"></param>
        public Sound3dInstaller(Transform globalAudioParent, AudioConfig audioConfig) : base(globalAudioParent, audioConfig) {
        }

        /// <summary>
        /// Installs bindings.
        /// </summary>
        public override void InstallBindings() {
            Container.Bind<ISoundController3d>()
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
            Transform parent = CreateSourceParent("Sound3dPool");

            subContainer.Bind<IFactory<IFxSource>>()
                .To<FxSourceFactory>()
                .FromNew()
                .AsCached()
                .WithArguments(parent, audioConfig.SoundSource3d);

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

            subContainer.Bind<ISoundController3d>()
                .To<SoundController3d>()
                .FromNew()
                .AsCached()
                .NonLazy();
        }
    }
}