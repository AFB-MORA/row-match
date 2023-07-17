using UnityEngine;
using Zenject;

namespace Com.AFBiyik.Pool
{
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoMemoryPoolInstaller&lt;PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn();
    /// </code>
    /// </summary>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoMemoryPoolInstaller<TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoMemoryPoolInstaller<TPrefab>> where TPrefab : Component
    {
        /// <inheritdoc/>
        public MonoMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <summary>
        /// Installs factory and pool in sub container.
        /// </summary>
        /// <param name="subContainer">Isolated container</param>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TPrefab>>()
                .To<MonoMemoryPool<TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
}