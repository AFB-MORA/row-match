using UnityEngine;
using Zenject;

namespace Com.AFBiyik.Pool
{
    /// <summary>
    /// Base class for Mono Poolable Memory Pool Installers.
    /// </summary>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public abstract class MonoPoolableMemoryPoolInstallerBase<TPrefab, TDerived> : Installer<TPrefab, Transform, MemoryPoolSettings, TDerived> where TPrefab : Component where TDerived : MonoPoolableMemoryPoolInstallerBase<TPrefab, TDerived>
    {
        /// <summary>
        /// Parent for created objects
        /// </summary>
        protected readonly Transform poolParent;

        /// <summary>
        /// Prefab to create
        /// </summary>
        protected readonly TPrefab prefab;

        /// <summary>
        /// Pool settings
        /// </summary>
        protected readonly MemoryPoolSettings memoryPoolSettings;

        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="prefab">Prefab to create</param>
        /// <param name="poolParent">Parent for created objects</param>
        /// <param name="memoryPoolSettings">Pool settings</param>
        public MonoPoolableMemoryPoolInstallerBase(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings)
        {
            this.prefab = prefab;
            this.poolParent = poolParent;
            this.memoryPoolSettings = memoryPoolSettings;
        }

        /// <summary>
        /// Installs pool from sub container.
        /// </summary>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <summary>
        /// Installs factory and pool in sub container.
        /// </summary>
        /// <param name="subContainer">Isolated container</param>
        protected abstract void InstallPool(DiContainer subContainer);

        /// <summary>
        /// Installs factory in sub countainer.
        /// </summary>
        /// <param name="subContainer">Isolated container</param>
        protected virtual void InstallFactory(DiContainer subContainer)
        {
            subContainer.Bind<IFactory<TPrefab>>()
                .To<PoolFactory<TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(prefab, poolParent);
        }
    }
}